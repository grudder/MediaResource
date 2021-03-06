﻿namespace MediaResource.Web.Models
{
    public enum ObjectType
    {
        /// <summary>
        /// 影视成片。
        /// </summary>
        Film = 0,

        /// <summary>
        /// 纸媒选登。
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
        /// 视频素材。
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
        /// 设计成品。
        /// </summary>
        GraphicDesign = 9,

        /// <summary>
        /// 专题文献报告。
        /// </summary>
        TopicText = 10,

        /// <summary>
        /// 专题图片。
        /// </summary>
        TopicImage = 11,

        /// <summary>
        /// 专题视频。
        /// </summary>
        TopicVideo = 12,

        /// <summary>
        /// 专题媒体报导。
        /// </summary>
        TopicNews = 13
    }
}