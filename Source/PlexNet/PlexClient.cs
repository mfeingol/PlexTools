// (c) 2022 Max Feingold

using System.Globalization;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace PlexNet
{
    public enum MetadataType : uint
    {
        Track = 10,
    }

    public class PlexClient
    {
        HttpClient client;



        public PlexClient(string address, string token)
        {
            if (!address.EndsWith("/"))
                address += "/";

            this.client = new HttpClient { BaseAddress = new Uri(address) };
            this.client.DefaultRequestHeaders.Add("X-Plex-Token", token);

            // Identify client and product in the header, so PMS knows who we are
            string? XPlexClientIdentifier =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            this.client.DefaultRequestHeaders.Add("X-Plex-Client-Identifier", XPlexClientIdentifier);
            this.client.DefaultRequestHeaders.Add("X-Plex-Product", "PlexTools");
        }

        public async Task<JsonDocument> GetDocumentAsync(string path, int start = -1, int size = -1)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (start >= 0 && size > 0)
            {
                request.Headers.Add("X-Plex-Container-Start", start.ToString(CultureInfo.InvariantCulture));
                request.Headers.Add("X-Plex-Container-Size", size.ToString(CultureInfo.InvariantCulture));
            }

            using HttpResponseMessage response = (await this.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)).EnsureSuccessStatusCode();
            using Stream stream = await response.Content.ReadAsStreamAsync();

            return await JsonDocument.ParseAsync(stream);
        }

        public async Task RateAsync(string key, decimal value)
        {
            (await this.client.PutAsync($"/:/rate?identifier=com.plexapp.plugins.library&key={key}&rating={value}", null)).EnsureSuccessStatusCode();
        }

        public async Task AddToPlaylistAsync(string machineId, uint playlistId, string ratingKey)
        {
            string qs = $"?uri=server%3A%2F%2F{machineId}%2Fcom.plexapp.plugins.library%2Flibrary%2Fmetadata%2F{ratingKey}";
            string path = $"/playlists/{playlistId}/items{qs}";

            (await this.client.PutAsync(path, null)).EnsureSuccessStatusCode();
        }
    }
}
