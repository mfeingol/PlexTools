# What is it?

This program reads the tracks stored in a [Plex](https://www.plex.tv) playlist JSON file, and copies the ratings to the corresponding tracks on the specified server. If a playlist id is provided, it will also add the tracks to the playlist.

Why would you need this? You have ❤️ Tracks playlist on one media server, and you want to transfer those ratings to a second media server. This is step two of two.

# Usage

ImportPlaylist 1.1.0
Copyright (C) 2022 Max Feingold

  -t, --token     Required. Token used to access Plex server

  -p, --server    Required. Plex server URL

  -f, --file      Required. Path to file to import

  -l, --playlist  Plex playlist id. Defaults to ❤️ Tracks playlist

  --help          Display this help screen.

  --version       Display version information.
  
# Examples

ImportPlaylist.exe -t ABc4UE1GKMGF1T4G4ws5 -p https://10-0-0-100.854032948508f830dca34044895698437.plex.direct:32400 -f hearts.json
ImportPlaylist.exe -t ABc4UE1GKMGF1T4G4ws5 -p https://10-0-0-100.854032948508f830dca34044895698437.plex.direct:32400 -f favorites.json -l 32768

# Where to find input parameters

Follow these instructions to find your Plex token: https://support.plex.tv/articles/204059436-finding-an-authentication-token-x-plex-token/

Launch plex.tv in your favorite browser and bring up the F12 debugging tools. Browse to your playlist and observe the request URL. The playlist is the parameter used in the GET /playlists/X/items request.
