// (c) 2019 Max Feingold

using CommandLine;

namespace WMPToPlex
{
    class Options
    {
        [Option('t', "token", Required = true, HelpText = "Token used to access Plex server")]
        public string Token { get; set; }

        [Option('p', "server", Required = true, HelpText = "Plex server hostname")]
        public string Server { get; set; }

        [Option('r', "rating", Required = false, HelpText = "WMP rating threshold", Default = 75)]
        public int Rating { get; set; }

        [Option('s', "section", Required = true, HelpText = "Plex library section")]
        public uint SectionId { get; set; }

        [Option('l', "playlist", Required = true, HelpText = "Plex playlist id")]
        public uint PlaylistId { get; set; }

        [Option('x', "local-prefix", Required = true, HelpText = "Local file path prefix")]
        public string LocalPrefix { get; set; }

        [Option('y', "server-prefix", Required = true, HelpText = "Server file path prefix")]
        public string ServerPrefix { get; set; }
    }
}
