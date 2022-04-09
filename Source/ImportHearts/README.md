# What is it?

This program reads the tracks stored in a [Plex](https://www.plex.tv) playlist JSON file, and copies the ratings to the corresponding tracks on the specified server.

Why would you need this? You have ❤️ Tracks playlist on one media server, and you want to transfer those ratings to a second media server. This is step two of two.

# Usage

ImportHearts 1.0.0
Copyright (C) 2022 Max Feingold

  -t, --token     Required. Token used to access Plex server

  -p, --server    Required. Plex server URL

  -f, --file      Required. Path to file to import

  --help          Display this help screen.

  --version       Display version information.
  
# Example
  
ImportHearts.exe -t ABc4UE1GKMGF1T4G4ws5 -p https://10-0-0-100.854032948508f830dca34044895698437.plex.direct:32400 -f hearts.json

# Where to find input parameters

Launch plex.tv in your favorite browser and bring up the F12 debugging tools.

The token is the X-Plex-Token parameter used in every request. The server is the plex.direct URL used by the browser.

Browse to your playlist and observe the request URL. The playlist is the parameter used in the /playlists/X/items request.
