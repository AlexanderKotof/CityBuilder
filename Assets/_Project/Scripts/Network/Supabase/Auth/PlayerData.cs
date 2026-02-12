using System;
using Postgrest.Models;

namespace CityBuilder.Network.SupabaseApi
{
    [Serializable]
    public class PlayerData : BaseModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}