﻿using Newtonsoft.Json;

namespace MediaResource.Web.Models.ViewModels
{
	public class ZTreeNode
    {
        [JsonProperty("id")]
		public int Id
		{
			get;
			set;
		}

		[JsonProperty("name")]
		public string Name
		{
			get;
			set;
		}

        [JsonProperty("isParent")]
        public bool IsParent
        {
            get;
            set;
        }

        [JsonProperty("open")]
        public bool Open
        {
            get;
            set;
        }

        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }

		[JsonProperty("href")]
		public string Href
		{
			get;
			set;
		}
	}
}