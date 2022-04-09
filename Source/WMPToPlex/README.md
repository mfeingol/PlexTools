# What is it?

This program identifies favorite audio tracks in the local Windows Media Player or [Zune](https://support.xbox.com/id-ID/zune-software/download) (RIP) library, and adds them to the specified [Plex](https://www.plex.tv) playlist.

You probably don't need this program, but if you do, you do.

# Usage

WMPToPlex 1.0.0.0
(c) 2019 Max Feingold

  -t, --token            Required. Token used to access Plex server

  -p, --server           Required. Plex server hostname

  -r, --rating           (Default: 75) WMP rating threshold

  -s, --section          Required. Plex library section

  -l, --playlist         Required. Plex playlist id

  -x, --local-prefix     Required. Local file path prefix

  -y, --server-prefix    Required. Server file path prefix
  
# Example
  
WmpToPlex.exe -t ABc4UE1GKMGF1T4G4ws5 -p https://10-0-0-100.854032948508f830dca34044895698437.plex.direct:32400 -s 14 -l 215486 -x "\\\\Server" -y "D:\Media"

# Where to find input parameters

Launch plex.tv in your favorite browser and bring up the F12 debugging tools.

The token is the X-Plex-Token parameter used in every request. The server is the plex.direct URL used by the browser.

Browse to your music library section and observe the request URL. The section is the parameter used in the /library/sections/X/all request.

Browse to your playlist and observe the request URL. The playlist is the parameter used in the /playlists/X/items request.

If your server is running on a different machine, it may consider paths differently than your client machine. To match tracks between WMP and Plex, the tool needs to know which parts of the paths to strip out in order to have identical paths to compare.

E.g. if a track is at D:\Media\Music\Artist\Album\01.mp3 on the server, and your client machine running WMP knows it as \\\\Server\Music\Artist\Album\01.mp3, then "\\Server" and "D:\Media" would be the local and server prefixes to specify, respectively.
