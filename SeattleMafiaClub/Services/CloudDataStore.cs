using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using SeattleMafiaClub.Services;

namespace SeattleMafiaClub
{
    public class CloudDataStore
    {
        HttpClient client;
        IEnumerable<Item> items;
        private string requestProviderArg;
        private string token;

        private const string PROVIDER_FACEBOOK = "facebook";
        private Location location;

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Item>();
            setCredentials(PROVIDER_FACEBOOK, AuthService.getInstance().getToken());
            setLocation(new Location(47.87f, -122.31f));
        }

        public void setCredentials(string provider, string token)
        {
            if (PROVIDER_FACEBOOK.Equals(provider))
                this.requestProviderArg = "FB";
            this.token = token;
        }

        public void setLocation(Location location)
        {
            this.location = location; 
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"status/{this.location.latitude}/{this.location.longitude}/{this.requestProviderArg}/{this.token}");
                items = await Task.Run(() =>
                {
                    JObject googleSearch = JObject.Parse(json);
                    // get JSON result objects into a list
                    IEnumerable<JToken> results = googleSearch["games"].Children();
                    // serialize JSON results into .NET objects
                    IList<Item> searchResults = new List<Item>();
                    foreach (JToken result in results)
                    {
                        // JToken.ToObject is a helper method that uses JsonSerializer internally
                        Item searchResult = result.ToObject<Item>();
                        searchResults.Add(searchResult);
                    }

                    return searchResults;
                });
            }

            return items;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Item>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item == null || item.Id == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/item/{id}");

            return response.IsSuccessStatusCode;
        }
    }

    public class Location
    {
        internal float latitude;
        internal float longitude;

        public Location(float latitude, float longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}
