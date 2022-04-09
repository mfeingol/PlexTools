// (c) 2022 Max Feingold

using CommandLine;

namespace PlaylistToHearts
{
    class Options
    {
        [Option('t', "token", Required = true, HelpText = "Token used to access Plex server")]
        public string Token { get; set; } = String.Empty;

        [Option('p', "server", Required = true, HelpText = "Plex server URL")]
        public string Server { get; set; } = String.Empty;

        [Option('l', "playlist", Required = true, HelpText = "Plex playlist id")]
        public uint PlaylistId { get; set; }

        //[Option('x', "local-prefix", Required = true, HelpText = "Local file path prefix")]
        //public string? LocalPrefix { get; set; }

        //[Option('y', "server-prefix", Required = true, HelpText = "Server file path prefix")]
        //public string? ServerPrefix { get; set; }
    }
}
