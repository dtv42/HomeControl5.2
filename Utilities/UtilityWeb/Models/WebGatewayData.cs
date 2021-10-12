namespace UtilityWeb.Models
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class WebGatewayData
    {
        public List<PostData> Posts { get; set; } = new List<PostData> { };
        public List<CommentData> Comments { get; set; } = new List<CommentData> { };
        public List<AlbumData> Albums { get; set; } = new List<AlbumData> { };
        public List<PhotoData> Photos { get; set; } = new List<PhotoData> { };
        public List<TodoData> Todos { get; set; } = new List<TodoData> { };
        public List<UserData> Users { get; set; } = new List<UserData> { };
    }
}
