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

            JsonDocument doc = await plex.GetDocumentAsync("/library/sections/all");
            JsonElement sectionContainer = doc.RootElement.GetProperty("MediaContainer");
            JsonElement sections = sectionContainer.GetProperty("Directory");

            Dictionary<string, string> lookup = new();

            foreach (JsonElement section in sections.EnumerateArray().Where(e => e.GetProperty("type").GetString() == "artist"))
            {
                Console.WriteLine($"Reading tracks from library section {section.GetProperty("title").GetString()}...");

                string key = section.GetProperty("key").GetString() ?? String.Empty;
                doc = await plex.GetDocumentAsync($"/library/sections/{key}/all?type={(uint)MetadataType.Track}");

                JsonElement trackContainer = doc.RootElement.GetProperty("MediaContainer");
                JsonElement tracks = trackContainer.GetProperty("Metadata");

                foreach (JsonElement track in tracks.EnumerateArray())
                {
                    string guid = track.GetProperty("guid").GetString() ?? String.Empty;
                    string ratingKey = track.GetProperty("ratingKey").GetString() ?? String.Empty;

                    lookup[guid] = ratingKey;
                }
            }

            Console.WriteLine($"Opening file {options.FilePath}");

            using FileStream stream = File.OpenRead(options.FilePath);
            JsonDocument file = await JsonDocument.ParseAsync(stream);

            JsonElement fileTracksContainer = file.RootElement.GetProperty("MediaContainer");
            JsonElement fileTracks = fileTracksContainer.GetProperty("Metadata");

            var fileTrackEnumerator = fileTracks.EnumerateArray();

            int count = 1;
            int total = fileTrackEnumerator.Count();

            foreach (JsonElement track in fileTrackEnumerator)
            {
                string? grandparentTitle = track.GetProperty("grandparentTitle").GetString();
                string? parentTitle = track.GetProperty("parentTitle").GetString();
                string? title = track.GetProperty("title").GetString();

                if (!String.IsNullOrEmpty(grandparentTitle) && !String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(title))
                    Console.WriteLine($"{count} of {total}: rating {grandparentTitle} / {parentTitle} / {title} ...");

                string guid = track.GetProperty("guid").GetString() ?? String.Empty;
                decimal rating = track.GetProperty("userRating").GetDecimal();

                if (lookup.TryGetValue(guid, out string? key))
                {
                    await plex.RateAsync(key, rating);
                    count++;
                }
                else
                {
                    Console.Write("Ignoring {0}", title ?? track.GetProperty("guid").GetString() ?? "track");
                }
            }

            Console.WriteLine($"Rated {count} track(s)");
        }
    }
}
