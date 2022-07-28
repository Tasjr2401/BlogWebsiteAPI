using Microsoft.Graph;
using System;

namespace BlogWebsiteAPI.Models
{
	public class Blog
	{
		public Blog(string title, string author, string content, DateTime creationTime)
		{
			Title = title;
			Author = author;
			Content = content;
			CreationTime = creationTime;
		}

		public string Title { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public DateTime CreationTime { get; set; }
	}
}
