namespace UtilityWeb.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    public class PhotoData
    {
        [JsonPropertyName("userId")]
        public int UserID { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("thumbnailUrl")]
        public string ThumbnailUrl { get; set; } = string.Empty;
    }
}
