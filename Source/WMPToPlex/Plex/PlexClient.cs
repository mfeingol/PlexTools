// (c) 2019 Max Feingold

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace WMPToPlex
{
    enum MetadataType
    {
        Track = 10,
    }

    class PlexClient
    {
        HttpClient client;

        public PlexClient(string address, string token)
        {
            if (!address.EndsWith("/"))
                address += "/";

            this.client = new HttpClient { BaseAddress = new Uri(address) };
            client.DefaultRequestHeaders.Add("X-Plex-Token", token);
        }

        public async Task<Root.MediaContainer> GetRootAsync()
        {
            using (Stream rootStream = await client.GetStreamAsync(String.Empty))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Root.MediaContainer));
                return (Root.MediaContainer)serializer.Deserialize(rootStream);
            }
        }

        public async Task<Section.MediaContainer> GetMetadataItemsAsync(uint sectionId, MetadataType type)
        {
            string url = $"library/sections/{sectionId}/all?type={(int)type}";

            using (Stream allStream = await client.GetStreamAsync(url))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Section.MediaContainer));
                return (Section.MediaContainer)serializer.Deserialize(allStream);
            }
        }

        public async Task AddToPlaylistAsync(string machineIdentifier, uint playlistId, uint metadataId)
        {
            string urlParam = $"server://{machineIdentifier}/com.plexapp.plugins.library/library/metadata/{metadataId}";
            string url = $"playlists/{playlistId}/items?uri={HttpUtility.UrlEncode(urlParam)}";

            using (HttpResponseMessage response = await client.PutAsync(url, null))
                response.EnsureSuccessStatusCode();
        }
    }
}
