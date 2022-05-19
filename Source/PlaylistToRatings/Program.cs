// (c) 2022 Max Feingold

using System.Text.Json;
using PlexNet;
using CommandLine;

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

            JsonDocument doc = await plex.GetDocumentAsync($"/playlists/{options.PlaylistId}/items");
            JsonElement mediaContainer = doc.RootElement.GetProperty("MediaContainer");
            JsonElement metadata = mediaContainer.GetProperty("Metadata");

            var enumerator = metadata.EnumerateArray();

            int count = 1;
            int total = enumerator.Count();

            foreach (JsonElement track in enumerator)
            {
                string? grandparentTitle = track.GetProperty("grandparentTitle").GetString();
                string? parentTitle = track.GetProperty("parentTitle").GetString();
                string? title = track.GetProperty("title").GetString();

                if (!String.IsNullOrEmpty(grandparentTitle) && !String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(title))
                    Console.WriteLine($"{count} of {total}: rating {grandparentTitle} / {parentTitle} / {title} ...");

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

            Console.WriteLine($"Rated {count} track(s) from playlist {mediaContainer.GetProperty("title").GetString()}");
        }
    }
}
