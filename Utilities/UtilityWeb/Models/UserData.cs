namespace UtilityWeb.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    public class UserData
    {
        public class GeoData
        {
            [JsonPropertyName("lat")]
            public string Latitude { get; set; } = string.Empty;

            [JsonPropertyName("lng")]
            public string Longitude { get; set; } = string.Empty;
        }

        public class AddressData
        {
            [JsonPropertyName("street")]
            public string Street { get; set; } = string.Empty;

            [JsonPropertyName("suite")]
            public string Suite { get; set; } = string.Empty;

            [JsonPropertyName("city")]
            public string City { get; set; } = string.Empty;

            [JsonPropertyName("zipcode")]
            public string Zipcode { get; set; } = string.Empty;

            [JsonPropertyName("geo")]
            public GeoData Location { get; set; } = new GeoData();
        }

        public class CompanyData
        {

            [JsonPropertyName("name")]
            public string Street { get; set; } = string.Empty;

            [JsonPropertyName("catchPhrase")]
            public string CatchPhrase { get; set; } = string.Empty;

            [JsonPropertyName("bs")]
            public string BS { get; set; } = string.Empty;
        }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;
        
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public AddressData Address { get; set; } = new AddressData();

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
        
        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;

        [JsonPropertyName("company")]
        public CompanyData Company { get; set; } = new CompanyData();
    }
}
