using System.Collections.Generic;
using Newtonsoft.Json;

namespace MediaResource.Web.Models.ViewModels
{
	public class TreeNode
	{
		[JsonIgnore]
		public int Id
		{
			get;
			set;
		}

		[JsonProperty("text")]
		public string Text
		{
			get;
			set;
		}

		[JsonProperty("icon")]
		public string Icon
		{
			get;
			set;
		}

		[JsonProperty("color")]
		public string Color
		{
			get;
			set;
		}

		[JsonProperty("backColor")]
		public string BackColor
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

		[JsonProperty("tags")]
		public List<string> Tags
		{
			get;
			set;
		}

		[JsonProperty("nodes")]
		public List<TreeNode> Nodes
		{
			get;
			set;
		}
	}
}