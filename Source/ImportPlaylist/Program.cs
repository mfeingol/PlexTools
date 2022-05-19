// (c) 2022 Max Feingold

using System.Text.Json;
using PlexNet;
using CommandLine;

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
                doc = await plex.GetDocumentAsync($"/library/sections/{key}/all?type={(uint)MetadataType.Track}");

                JsonElement trackContainer = doc.RootElement.GetProperty("MediaContainer");
                JsonElement tracks = trackContainer.GetProperty("Metadata");

                foreach (JsonElement track in tracks.EnumerateArray())
                {
                    string guid = track.GetProperty("guid").GetString() ?? String.Empty;
                    targetLookup[guid] = (track, key);
                }

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
                    Console.WriteLine($"{count + 1} of {total}: rating {grandparentTitle} / {parentTitle} / {title} ...");
                }
                else
                {
                    Console.WriteLine("ERROR: skipping track without title");
                    continue;
                }

                string destRatingKey = GetDestRatingKey(sourceTrack, targetLookup, libraryLookup);
                if (String.IsNullOrEmpty(destRatingKey))
                {
                    Console.Write("ERROR: unable to get rating key for track {0}", title ?? sourceTrack.GetProperty("guid").GetString() ?? "track");
                    continue;
                }

                // Always copy rating over
                if (sourceTrack.GetProperty("userRating").TryGetDecimal(out decimal rating))
                    await plex.RateAsync(destRatingKey, rating);

                if (options.PlaylistId.HasValue)
                    await plex.AddToPlaylistAsync(machineId, options.PlaylistId.Value, destRatingKey);
            }

            Console.WriteLine($"Imported {count} track(s)");
        }

        static string GetDestRatingKey(JsonElement track, Dictionary<string, (JsonElement, string)> targetLookup, Dictionary<string, JsonElement> libraryLookup)
        {
            string ratingKey = String.Empty;

            string guid = track.GetProperty("guid").GetString() ?? String.Empty;
            if (targetLookup.TryGetValue(guid, out var targetLookupTrack))
            {
                ratingKey = targetLookupTrack.Item1.GetProperty("ratingKey").GetString() ?? String.Empty;
                if (!String.IsNullOrEmpty(ratingKey))
                    return ratingKey;
            }

            string? grandparentTitle = track.GetProperty("grandparentTitle").GetString()?.ToLower();
            string? parentTitle = track.GetProperty("parentTitle").GetString()?.ToLower();
            string? title = track.GetProperty("title").GetString()?.ToLower();

            JsonElement foundTrack = (from targetTrack in targetLookup.Values
                                      where targetTrack.Item1.GetProperty("grandparentTitle").GetString()?.ToLower() == grandparentTitle
                                      where targetTrack.Item1.GetProperty("parentTitle").GetString()?.ToLower() == parentTitle
                                      where targetTrack.Item1.GetProperty("title").GetString()?.ToLower() == title
                                      select targetTrack.Item1).FirstOrDefault();

            if (foundTrack.ValueKind != JsonValueKind.Undefined)
                ratingKey = foundTrack.GetProperty("ratingKey").GetString() ?? String.Empty;

            if (!String.IsNullOrEmpty(ratingKey))
                return ratingKey;

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
                                    ratingKey = targetTrack.Item1.GetProperty("ratingKey").GetString() ?? String.Empty;
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
