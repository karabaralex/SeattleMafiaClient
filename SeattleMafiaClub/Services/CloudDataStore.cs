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
        private string requestProviderArg;
        private string token;

        private const string PROVIDER_FACEBOOK = "facebook";
        private Location location;

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

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

        public async Task<IEnumerable<Table>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"club-status/0/{this.requestProviderArg}/{this.token}");
                return await parsePlayerStatus(json);
            }

            return new List<Table>();
        }

        private async Task<IEnumerable<Table>> parsePlayerStatus(string json)
        {
            return await Task.Run(() =>
            {
                IList<Table> searchResults = new List<Table>();
                JObject response = JObject.Parse(json);

                if (response["queued"] != null)
                {
                    foreach (JToken result in response["queued"].Children())
                    {
                        // JToken.ToObject is a helper method that uses JsonSerializer internally
                        Table item = result["table"].ToObject<Table>();
                        item.PlayerStatusOnTable = PlayerStatusOnTable.QUEUED;
                        searchResults.Add(item);
                    }
                }

                if (response["non-queued"] != null)
                {
                    foreach (JToken result in response["non-queued"].Children())
                    {
                        // JToken.ToObject is a helper method that uses JsonSerializer internally
                        Table item = result.ToObject<Table>();
                        item.PlayerStatusOnTable = PlayerStatusOnTable.NON_QUEUED;
                        searchResults.Add(item);
                    }
                }

                if (response["seated"] != null)
                {
                    //foreach (JToken result in response["seated"].Children())
                    {
                        // JToken.ToObject is a helper method that uses JsonSerializer internally
                        Table item = response["seated"]["table"].ToObject<Table>();
                        item.PlayerStatusOnTable = PlayerStatusOnTable.SEATED;
                        searchResults.Add(item);
                    }
                }

                return searchResults;
            });
        }

        public async Task<Table> JoinGame(Table table)
        {
            if (table != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"join/{table.Id}/{this.requestProviderArg}/{this.token}");
                IEnumerable<Table> results = await parsePlayerStatus(json);
                foreach (Table resultTable in results)
                {
                    if (resultTable.Id.Equals(table.Id))
                    {
                        return resultTable;
                    }
                }
            }

            return table;
        }

        public async Task<Table> LeaveGame(Table table)
        {
            if (table != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"leave/{table.Id}/{this.requestProviderArg}/{this.token}");
                IEnumerable<Table> results = await parsePlayerStatus(json);
                foreach (Table resultTable in results)
                {
                    if (resultTable.Id.Equals(table.Id))
                    {
                        return resultTable;
                    }
                }
            }

            return table;
        }

        public async Task<Table> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Table>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Table item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Table item)
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
