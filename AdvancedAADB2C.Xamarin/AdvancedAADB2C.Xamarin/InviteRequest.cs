using Newtonsoft.Json;

namespace AdvancedAADB2C.Xamarin
{
    class InviteRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
