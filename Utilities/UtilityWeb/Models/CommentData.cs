namespace UtilityWeb.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    public class CommentData
    {
        [JsonPropertyName("postId")]
        public int PostID { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;
    }
}
