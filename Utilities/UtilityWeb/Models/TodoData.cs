namespace UtilityWeb.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    public class TodoData
    {
        [JsonPropertyName("userId")]
        public int UserID { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}
