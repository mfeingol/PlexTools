// (c) 2022 Max Feingold

using System.Text.Json;
using PlexNet;
using CommandLine;
using System.Globalization;

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

            Console.WriteLine($"Getting playlists...");

            JsonDocument doc = await plex.GetDocumentAsync($"/playlists?playlistType=audio&includeCollections=1&includeExternalMedia=1&includeAdvanced=1&includeMeta=1");
            JsonElement mediaContainer = doc.RootElement.GetProperty("MediaContainer");

            JsonElement metadata = mediaContainer.GetProperty("Metadata");

            JsonElement playlist;
            string playlistId;

            if (options.PlaylistId.HasValue)
            {
                playlistId = options.PlaylistId.Value.ToString(CultureInfo.InvariantCulture);
                playlist = metadata.EnumerateArray().FirstOrDefault(e => e.GetProperty("ratingKey").GetString() == playlistId);
            }
            else
            {                
                playlist = metadata.EnumerateArray().FirstOrDefault(e => e.GetProperty("title").GetString() == "❤️ Tracks");
                // We can't be sure that ❤️ Tracks exists
                if (!playlist.ValueKind.Equals(JsonValueKind.Undefined))
                {
                    playlistId = playlist.GetProperty("ratingKey").GetString() ?? String.Empty;
                    string title = playlist.GetProperty("title").GetString() ?? String.Empty;

                    Console.WriteLine($"Getting playlist {title}...");

                    doc = await plex.GetDocumentAsync($"/playlists/{playlistId}/items");

                    using FileStream file = File.OpenWrite(options.FilePath);
                    var writerOptions = new JsonWriterOptions
                    {
                        Indented = true
                    };
                    using Utf8JsonWriter writer = new(file, options: writerOptions);
                    doc.WriteTo(writer);

                    Console.WriteLine($"Wrote playlist {title} to {options.FilePath}");
                }
                else
                {
                    Console.WriteLine($"ERROR: unable to find playlist");
                }
            }
        }
    }
}
