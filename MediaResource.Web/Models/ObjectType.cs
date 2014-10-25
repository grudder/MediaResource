namespace MediaResource.Web.Models
{
    public enum ObjectType
    {
        /// <summary>
        /// 影视成片。
        /// </summary>
        Film = 0,

        /// <summary>
        /// 纸质媒体。
        /// </summary>
        Graphic = 1,

        /// <summary>
        /// 音频素材。
        /// </summary>
        Music = 2,

        /// <summary>
        /// 电视新闻。
        /// </summary>
        News = 3,

        /// <summary>
        /// 照片素材。
        /// </summary>
        Photo = 4,

        /// <summary>
        /// 视频集。
        /// </summary>
        Video = 5,

        /// <summary>
        /// 个人上传照片。
        /// </summary>
        UserPhoto = 6,

        /// <summary>
        /// 个人上传视频。
        /// </summary>
        UserVideo = 7,

        /// <summary>
        /// 图文设计成品。
        /// </summary>
        GraphicDesign = 9
    }
}