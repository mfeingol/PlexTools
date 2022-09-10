// (c) 2022 Max Feingold

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
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

            Console.WriteLine($"Getting playlists...");

            JsonDocument doc = await plex.GetDocumentAsync($"/playlists?playlistType=audio&includeCollections=1&includeExternalMedia=1&includeAdvanced=1&includeMeta=1");

            JsonElement mediaContainer = doc.RootElement.GetProperty("MediaContainer");
            JsonElement metadata = mediaContainer.GetProperty("Metadata");

            string? playlistId = options.PlaylistId?.ToString(CultureInfo.InvariantCulture);

            JsonElement playlist;
            if (!String.IsNullOrEmpty(playlistId))
                playlist = metadata.EnumerateArray().FirstOrDefault(e => e.GetProperty("ratingKey").GetString() == playlistId);
            else
                playlist = metadata.EnumerateArray().FirstOrDefault(e => e.GetProperty("title").GetString() == "❤️ Tracks");

            // We can't be sure that the playlist exists
            if (playlist.ValueKind == JsonValueKind.Undefined)
            {
                Console.WriteLine($"ERROR: unable to find playlist");
                return;
            }

            string title = playlist.GetProperty("title").GetString() ?? String.Empty;
            playlistId = playlist.GetProperty("ratingKey").GetString() ?? String.Empty;

            Console.WriteLine($"Reading playlist {title}...");

            List<JsonElement> tracks = new();
            JsonElement pagedPlaylistMediaContainer;

            while (true)
            {
                const int size = 120;

                doc = await plex.GetDocumentAsync($"/playlists/{playlistId}/items", tracks.Count, size);
                pagedPlaylistMediaContainer = doc.RootElement.GetProperty("MediaContainer");

                int recvdSize = pagedPlaylistMediaContainer.GetProperty("size").GetInt32();
                int totalSize = pagedPlaylistMediaContainer.GetProperty("totalSize").GetInt32();

                metadata = pagedPlaylistMediaContainer.GetProperty("Metadata");
                tracks.AddRange(metadata.EnumerateArray());

                Console.CursorLeft = 0;
                Console.Write($"Read {tracks.Count} of {totalSize} tracks from playlist {title}...");

                if (recvdSize < size)
                    break;
            }
            Console.WriteLine();

            JsonObject aggregateContainer = new();

            // Copy media container properties from last page
            foreach (JsonProperty property in pagedPlaylistMediaContainer.EnumerateObject())
            {
                if (property.Name == "Metadata" || property.Name == "size" || property.Name == "totalSize" || property.Name == "offset" || property.Name == "duration")
                    continue;

                aggregateContainer.Add(property.Name, ToJsonNode(property.Value));
            }

            // Set media container properties for aggregate playlist
            aggregateContainer.Add("duration", playlist.GetProperty("duration").GetInt32());
            aggregateContainer.Add("totalSize", tracks.Count);
            aggregateContainer.Add("Metadata", new JsonArray(tracks.Select(t => ToJsonNode(t)).ToArray()));

            // Serialize aggregate document
            using FileStream file = File.OpenWrite(options.FilePath);
            file.SetLength(0);

            using Utf8JsonWriter writer = new(file, options: new() { Indented = true });
            new JsonObject() { { "MediaContainer", aggregateContainer } }.WriteTo(writer);
            await writer.FlushAsync();

            Console.WriteLine($"Wrote playlist {title} to {options.FilePath}");
        }

        static JsonNode? ToJsonNode(JsonElement element)
        {
            // This is awful but there does not appear to be a better way to copy properties over... until .NET 7
            using MemoryStream mem = new();
            using Utf8JsonWriter writer = new(mem);

            element.WriteTo(writer);
            writer.Flush();

            mem.Position = 0;

            return JsonNode.Parse(mem);
        }
    }
}
