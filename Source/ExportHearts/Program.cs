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

            Console.WriteLine($"Getting playlists...");

            JsonDocument doc = await plex.GetDocumentAsync($"/playlists?playlistType=audio&includeCollections=1&includeExternalMedia=1&includeAdvanced=1&includeMeta=1");
            JsonElement mediaContainer = doc.RootElement.GetProperty("MediaContainer");

            JsonElement metadata = mediaContainer.GetProperty("Metadata");
            JsonElement hearts = metadata.EnumerateArray().FirstOrDefault(e => e.GetProperty("guid").GetString() == "com.plexapp.agents.none://54d52a9b-6a93-4625-acbd-43d7cf7fe674");
            string playlistId = hearts.GetProperty("ratingKey").GetString() ?? String.Empty;
            string title = hearts.GetProperty("title").GetString() ?? String.Empty;

            Console.WriteLine($"Getting playlist {title}...");

            doc = await plex.GetDocumentAsync($"/playlists/{playlistId}/items");

            using FileStream file = File.OpenWrite(options.FilePath);
            using Utf8JsonWriter writer = new(file);
            doc.WriteTo(writer);

            Console.WriteLine($"Wrote playlist {title} to {options.FilePath}");
        }
    }
}
