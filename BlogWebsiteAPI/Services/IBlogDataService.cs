using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Requests.BlogRequest;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BlogWebsiteAPI.Services
{
	public interface IBlogDataService
	{
		public int CreateBlog(string title, int authorId, string content);
		public Blog GetBlogById(int id);
		public BlogPreview SearchForBlogs(string search);
	}

	public class SQLBlogDataService : IBlogDataService
	{
		private readonly string CONNECTIONSTRING;
		private readonly IConfiguration _config;
		private readonly IKeyVaultManagement _keyManager;

		public SQLBlogDataService(IConfiguration config, IKeyVaultManagement keyManager)
		{

			_config = config;
			_keyManager = keyManager;
			CONNECTIONSTRING = _keyManager.GetSecret("BlogWebsiteConnection").Result; //_config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
		}
		public int CreateBlog(string title, int authorId, string content)
		{
			using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
			{
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = "CreateBlog";
					cmd.CommandType = CommandType.StoredProcedure;
					//cmd.CommandText = "EXEC dbo.CreateBlog @Title, @AuthorId, @Content, @CreationDate";
					cmd.Parameters.Add("@Title", SqlDbType.VarChar, 500).Value = title;
					cmd.Parameters.Add("@AuthorId", SqlDbType.Int).Value = authorId;
					cmd.Parameters.Add("@Content", SqlDbType.NVarChar).Value = content;
					cmd.Parameters.Add("@Date", SqlDbType.Date).Value = DateTime.Now.Date;

					try
					{
						var result = cmd.ExecuteNonQuery();
						conn.Close();
						return result;
					}
					catch (Exception ex)
					{
						conn.Close();
						throw new Exception(ex.Message, ex);
					}
				}
			}
		}

		public Blog GetBlogById(int id)
		{
			throw new NotImplementedException();
		}

		public BlogPreview SearchForBlogs(string search)
		{
			throw new System.NotImplementedException();
		}
	}
}
