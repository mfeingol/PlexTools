# What is it?

This program rates as 5 stars all the specified [Plex](https://www.plex.tv) music playlist.

Why would you need this? You have a playlist of favorite tracks, and you'd like to rate them instead so they show up in the ❤️ Tracks playlist.

# Usage

PlaylistToHearts 1.0.0
Copyright (C) 2022 Max Feingold

  -t, --token       Required. Token used to access Plex server

  -p, --server      Required. Plex server URL

  -l, --playlist    Required. Plex playlist id

  --help            Display this help screen.

  --version         Display version information.
  
# Example
  
PlaylistToHearts.exe -t ABc4UE1GKMGF1T4G4ws5 -p https://10-0-0-100.854032948508f830dca34044895698437.plex.direct:32400 -l 215486

# Where to find input parameters

Launch plex.tv in your favorite browser and bring up the F12 debugging tools.

The token is the X-Plex-Token parameter used in every request. The server is the plex.direct URL used by the browser.

Browse to your playlist and observe the request URL. The playlist is the parameter used in the /playlists/X/items request.
