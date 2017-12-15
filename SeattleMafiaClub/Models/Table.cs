using System;
using Newtonsoft.Json;

namespace SeattleMafiaClub
{
    public enum PlayerStatusOnTable
    {
        QUEUED, SEATED, NON_QUEUED
    }

    public enum GameStatusValue
    {
        PENDING_START, IN_PROGRESS, FINISHED, NONE
    }

    public class Table
    {
        private string statusValue;

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status 
        { 
            get
            {
                return this.statusValue;
            } 

            set
            {
                this.statusValue = value;
                switch(value)
                {
                    case "PENDING_START":
                        this.GameStatus = GameStatusValue.PENDING_START;
                        return;
                    case "IN_PROGRESS":
                        this.GameStatus = GameStatusValue.IN_PROGRESS;
                        return;
                    case "FINISHED":
                        this.GameStatus = GameStatusValue.FINISHED;
                        return;
                    case "NONE":
                        this.GameStatus = GameStatusValue.NONE;
                        return;
                    default:
                        throw new ArgumentException("incorrect value of game status");
                }
            } 
        }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "host")]
        public Host Host { get; set; }

        public PlayerStatusOnTable PlayerStatusOnTable { get; set; }
        public GameStatusValue GameStatus { get; set; }
    }

    public class Host
    {
        // json values
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "pictureurl")]
        public string Picture { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }
    }
}
