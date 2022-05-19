// (c) 2022 Max Feingold

using CommandLine;

namespace ExportHearts
{
    class Options
    {
        [Option('t', "token", Required = true, HelpText = "Token used to access Plex server")]
        public string Token { get; set; } = String.Empty;

        [Option('p', "server", Required = true, HelpText = "Plex server URL")]
        public string Server { get; set; } = String.Empty;

        [Option('f', "file", Required = true, HelpText = "Path to file to import")]
        public string FilePath { get; set; } = String.Empty;

        [Option('l', "playlist", Required = false, HelpText = "Target Plex playlist id. Defaults to 5-star ratings playlist")]
        public uint? PlaylistId { get; set; }
    }
}
