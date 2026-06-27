// (c) 2022 Max Feingold

using System.Text.Json;
using CommandLine;
using PlexNet;

namespace ExportHearts
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Options? options = null;
            ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);

            if (result.Tag == ParserResultType.Parsed && options != null)
                await RunAsync(options);
        }

        static async Task RunAsync(Options options)
        {
            Console.WriteLine($"Connecting to Plex server at {options.Server}...");

            PlexClient plex = new(options.Server, options.Token);

            JsonDocument root = await plex.GetDocumentAsync("/");
            JsonElement container = root.RootElement.GetProperty("MediaContainer");
            string machineId = container.GetProperty("machineIdentifier").GetString() ?? String.Empty;

            JsonDocument doc = await plex.GetDocumentAsync("/library/sections/all");
            JsonElement sectionContainer = doc.RootElement.GetProperty("MediaContainer");
            JsonElement sections = sectionContainer.GetProperty("Directory");

            Dictionary<string, (JsonElement, string)> targetLookup = new();
            Dictionary<string, JsonElement> libraryLookup = new();

            foreach (JsonElement section in sections.EnumerateArray().Where(e => e.GetProperty("type").GetString() == "artist"))
            {
                Console.WriteLine($"Reading tracks from music library section {section.GetProperty("title").GetString()}...");

                string key = section.GetProperty("key").GetString() ?? String.Empty;

                int totalRecvd = 0;
                while (true)
                {
                    const int size = 120;

                    doc = await plex.GetDocumentAsync($"/library/sections/{key}/all?type={(uint)MetadataType.Track}", totalRecvd, size);

                    JsonElement trackContainer = doc.RootElement.GetProperty("MediaContainer");
                    JsonElement tracks = trackContainer.GetProperty("Metadata");

                    string? title = section.GetProperty("title").GetString();
                    int totalSize = trackContainer.GetProperty("totalSize").GetInt32();

                    int recvd = trackContainer.GetProperty("size").GetInt32();
                    totalRecvd += recvd;

                    foreach (JsonElement track in tracks.EnumerateArray())
                    {
                        string guid = track.GetProperty("guid").GetString() ?? String.Empty;
                        targetLookup[guid] = (track, key);
                    }

                    Console.CursorLeft = 0;
                    Console.Write($"Read {totalRecvd} of {totalSize} tracks from music library section {title}...");

                    if (recvd < size)
                        break;
                }
                Console.WriteLine();

                libraryLookup[key] = section;
            }

            Console.WriteLine($"Opening source playlist file {options.FilePath}");

            using FileStream stream = File.OpenRead(options.FilePath);
            JsonDocument sourceFile = await JsonDocument.ParseAsync(stream);

            JsonElement sourceTracksContainer = sourceFile.RootElement.GetProperty("MediaContainer");
            JsonElement sourceTracks = sourceTracksContainer.GetProperty("Metadata");

            var sourceTrackEnumerator = sourceTracks.EnumerateArray();

            int count = 0;
            int total = sourceTrackEnumerator.Count();

            foreach (JsonElement sourceTrack in sourceTrackEnumerator)
            {
                string? grandparentTitle = sourceTrack.GetProperty("grandparentTitle").GetString();
                string? parentTitle = sourceTrack.GetProperty("parentTitle").GetString();
                string? title = sourceTrack.GetProperty("title").GetString();

                if (!String.IsNullOrEmpty(grandparentTitle) && !String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(title))
                {
                    Console.WriteLine($"{count + 1} of {total}: importing {grandparentTitle} / {parentTitle} / {title}");
                }
                else
                {
                    Console.WriteLine("ERROR: skipping track without title");
                    continue;
                }

                string destRatingKey = GetTargetRatingKey(sourceTrack, targetLookup, libraryLookup);
                if (String.IsNullOrEmpty(destRatingKey))
                {
                    Console.Write("ERROR: unable to get rating key for track {0}", title ?? sourceTrack.GetProperty("guid").GetString() ?? "track");
                    continue;
                }

                if (options.PlaylistId.HasValue)
                {
                    await plex.AddToPlaylistAsync(machineId, options.PlaylistId.Value, destRatingKey);
                }
                else if (sourceTrack.GetProperty("userRating").TryGetDecimal(out decimal rating))
                {
                    await plex.RateAsync(destRatingKey, rating);
                }
                else
                {
                    Console.Write("ERROR: unable to rate track {0}", title ?? sourceTrack.GetProperty("guid").GetString() ?? "track");
                    continue;
                }

                count++;
            }

            Console.WriteLine($"Imported {count} track(s)");
        }

        static string GetTargetRatingKey(JsonElement track, Dictionary<string, (JsonElement, string)> targetLookup, Dictionary<string, JsonElement> libraryLookup)
        {
            //
            // Find by guid
            //

            string guid = track.GetProperty("guid").GetString() ?? String.Empty;
            if (targetLookup.TryGetValue(guid, out (JsonElement, string) targetLookupTrack))
            {
                string ratingKey = targetLookupTrack.Item1.GetProperty("ratingKey").GetString() ?? String.Empty;
                if (!String.IsNullOrEmpty(ratingKey))
                    return ratingKey;
            }

            //
            // Find by artist/album/track title match
            //

            string? grandparentTitle = track.GetProperty("grandparentTitle").GetString()?.ToLower();
            string? parentTitle = track.GetProperty("parentTitle").GetString()?.ToLower();
            string? title = track.GetProperty("title").GetString()?.ToLower();

            JsonElement foundTrack = new();

            foreach ((JsonElement, string) targetTrack in targetLookup.Values)
            {
                if ((targetTrack.Item1.TryGetProperty("grandparentTitle", out JsonElement targetGrandparentTitle) && targetGrandparentTitle.GetString()?.ToLower() == grandparentTitle) &&
                    (targetTrack.Item1.TryGetProperty("parentTitle", out JsonElement targetParentTitle) && targetParentTitle.GetString()?.ToLower() == parentTitle) &&
                    (targetTrack.Item1.TryGetProperty("title", out JsonElement targetTitle) && targetTitle.GetString()?.ToLower() == title))
                {
                    foundTrack = targetTrack.Item1;
                    if (foundTrack.ValueKind != JsonValueKind.Undefined)
                    {
                        string ratingKey = foundTrack.GetProperty("ratingKey").GetString() ?? String.Empty;
                        if (!String.IsNullOrEmpty(ratingKey))
                            return ratingKey;
                    }
                }
            }

            //
            // Find by file path
            //

            string? filePath = track.GetProperty("Media").EnumerateArray().FirstOrDefault().GetProperty("Part").EnumerateArray().Select(e => e.GetProperty("file").GetString()).FirstOrDefault();
            if (!String.IsNullOrEmpty(filePath))
            {
                foreach (var targetTrack in targetLookup.Values)
                {
                    if (libraryLookup.TryGetValue(targetTrack.Item2.ToString(), out JsonElement library))
                    {
                        string?[] paths = library.GetProperty("Location").EnumerateArray().Select(e => e.GetProperty("path").GetString()).ToArray();
                        string? targetFilePath = targetTrack.Item1.GetProperty("Media").EnumerateArray().FirstOrDefault().GetProperty("Part").EnumerateArray().Select(e => e.GetProperty("file").GetString()).FirstOrDefault();

                        if (!String.IsNullOrEmpty(targetFilePath))
                        {
                            string? rootPath = paths.FirstOrDefault(p => !String.IsNullOrEmpty(p) && targetFilePath.StartsWith(p));
                            if (!String.IsNullOrEmpty(rootPath))
                            {
                                string relevantPath = targetFilePath.Substring(rootPath.Length);
                                if (!String.IsNullOrEmpty(relevantPath) && filePath.EndsWith(relevantPath))
                                {
                                    string ratingKey = targetTrack.Item1.GetProperty("ratingKey").GetString() ?? String.Empty;
                                    if (!String.IsNullOrEmpty(ratingKey))
                                        return ratingKey;
                                }
                            }
                        }

                    }
                }
            }

            return String.Empty;
        }
    }
}
