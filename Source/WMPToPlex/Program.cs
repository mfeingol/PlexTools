// (c) 2019 Max Feingold

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMPLib;
using CommandLine;

namespace WMPToPlex
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Options options = null;
            ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);

            if (result.Tag == ParserResultType.Parsed)
                await RunAsync(options);
        }

        static async Task RunAsync(Options options)
        {
            // Get Plex server info
            Console.WriteLine($"Connecting to Plex server at {options.Server}...");

            PlexClient plex = new PlexClient(options.Server, options.Token);
            string machineIdentifier = (await plex.GetRootAsync()).machineIdentifier;

            // Read all tracks from library section
            Console.WriteLine($"Reading all tracks from library section {options.SectionId}...");

            Dictionary<string, uint> metadataIds = new Dictionary<string, uint>();

            foreach (var track in (await plex.GetMetadataItemsAsync(options.SectionId, MetadataType.Track)).Tracks)
            {
                string path = track.Media.Part.file;
                if (path.ToLower().StartsWith(options.ServerPrefix.ToLower()))
                    path = path.Substring(options.ServerPrefix.Length);

                metadataIds[path.ToLower()] = track.ratingKey;
            }

            // Process favorites from local WMP library
            Console.WriteLine($"Connecting to local WMP library...");

            WMPClient wmp = new WMPClient();

            int added = 0;
            foreach (IWMPMedia3 track in wmp.GetAudioTracks().Where(t => wmp.GetUserRating(t) >= options.Rating))
            {
                string path = track.sourceURL;
                if (path.ToLower().StartsWith(options.LocalPrefix.ToLower()))
                    path = path.Substring(options.LocalPrefix.Length);

                if (metadataIds.TryGetValue(path.ToLower(), out uint metadataId))
                {
                    Console.Write($"Adding {path} to playlist...");

                    await plex.AddToPlaylistAsync(machineIdentifier, options.PlaylistId, metadataId);

                    Console.WriteLine($" added");

                    added++;
                }
                else
                {
                    Console.WriteLine($"ERROR: unable to find track {path} in Plex library section!");
                }
            }

            Console.WriteLine($"{added} track(s) added to playlist");
        }
    }
}
