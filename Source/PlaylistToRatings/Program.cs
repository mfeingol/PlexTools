// (c) 2022 Max Feingold

using System.Text.Json;
using CommandLine;
using PlexNet;

namespace PlaylistToHearts
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

            string? playlistTitle;
            int totalRecvd = 0;
            int count = 0;

            while (true)
            {
                const int size = 120;

                JsonDocument doc = await plex.GetDocumentAsync($"/playlists/{options.PlaylistId}/items", totalRecvd, size);

                JsonElement mediaContainer = doc.RootElement.GetProperty("MediaContainer");
                JsonElement metadata = mediaContainer.GetProperty("Metadata");

                playlistTitle = mediaContainer.GetProperty("title").GetString();
                int totalSize = mediaContainer.GetProperty("totalSize").GetInt32();

                int recvd = mediaContainer.GetProperty("size").GetInt32();
                totalRecvd += recvd;

                foreach (JsonElement track in metadata.EnumerateArray())
                {
                    string? grandparentTitle = track.GetProperty("grandparentTitle").GetString();
                    string? parentTitle = track.GetProperty("parentTitle").GetString();
                    string? title = track.GetProperty("title").GetString();

                    if (!String.IsNullOrEmpty(grandparentTitle) && !String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(title))
                        Console.WriteLine($"Rating {count + 1} of {totalSize}: {grandparentTitle} / {parentTitle} / {title} ...");

                    string? ratingKey = track.GetProperty("ratingKey").GetString();
                    if (!String.IsNullOrEmpty(ratingKey))
                    {
                        await plex.RateAsync(ratingKey, 10);
                        count++;
                    }
                    else
                    {
                        Console.Write("Ignoring {0}", title ?? track.GetProperty("guid").GetString() ?? "track");
                    }
                }

                if (recvd < size)
                    break;
            }

            Console.WriteLine($"Rated {count} track(s) from playlist {playlistTitle}");
        }
    }
}
