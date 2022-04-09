// (c) 2019 Max Feingold

using System;
using System.Collections.Generic;
using System.Globalization;
using WMPLib;

namespace WMPToPlex
{
    class WMPClient
    {
        public IEnumerable<IWMPMedia3> GetAudioTracks()
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer();

            IWMPMediaCollection2 collection = (IWMPMediaCollection2)player.mediaCollection;
            IWMPPlaylist playlist = collection.getByAttribute("MediaType", "Audio");

            for (int i = 0; i < playlist.count; i++)
            {
                IWMPMedia3 media = (IWMPMedia3)playlist.get_Item(i);
                yield return media;
            }
        }

        public uint GetUserRating(IWMPMedia3 media)
        {
            string userRating = media.getItemInfo("UserRating");
            if (UInt32.TryParse(userRating, NumberStyles.Any, CultureInfo.InvariantCulture, out uint rating))
                return rating;

            Console.WriteLine($"ERROR: unable to parse UserRating '{userRating}' for track {media.sourceURL}");
            return 0;
        }
    }
}
