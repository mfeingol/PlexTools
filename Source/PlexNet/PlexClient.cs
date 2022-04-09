// (c) 2022 Max Feingold

using System.Net.Http.Headers;
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
            client.DefaultRequestHeaders.Add("X-Plex-Token", token);
        }

        public async Task<JsonDocument> GetDocumentAsync(string path)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            using Stream stream = await response.Content.ReadAsStreamAsync();

            return await JsonDocument.ParseAsync(stream);
        }

        public async Task RateAsync(string key, decimal value)
        {
            (await client.PutAsync($"/:/rate?identifier=com.plexapp.plugins.library&key={key}&rating={value}", null)).EnsureSuccessStatusCode();
        }
    }
}
