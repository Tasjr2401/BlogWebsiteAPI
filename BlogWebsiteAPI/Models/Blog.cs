using Microsoft.Graph;
using System;

namespace BlogWebsiteAPI.Models
{
	public class Blog
	{
		public Blog(int id, string title, int authorId, string content, DateTime creationTime)
		{
			Id = id;
			Title = title;
			AuthorId = authorId;
			Content = content;
			CreationTime = creationTime;
		}
		public int Id { get; set; }
		public string Title { get; set; }
		public int AuthorId { get; set; }
		public string Content { get; set; }
		public DateTime CreationTime { get; set; }
	}

	public class BlogPreview
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string AuthorName { get; set; } 
		public DateTime CreationTime { get; set; }
		public BlogPreview(int id, string title, string authorName, DateTime creationTime)
		{
			Id = id;
			Title = title;
			AuthorName = authorName;
			CreationTime = creationTime;
		}
	}
}
