using BlogWebsiteAPI.Requests.UserRequests;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace BlogWebsiteAPI.Services
{
    public interface IUserDataService
    {
        public int InsertNewUser(CreateUser.Request request, byte[] salt, string hashedPassword);
    }

    public class SqlUserDataService : IUserDataService
    {
        private readonly IConfiguration _config;

        public SqlUserDataService(IConfiguration config)
        {
            _config = config;
        }
        public int InsertNewUser(CreateUser.Request request, byte[] salt, string hashedPassword)
        {
            var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "EXEC dbo.CreateUser @Username, @Firstname, @Lastname, @Role, @Password, @Salt;";
                cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 30).Value = request.Username;
                cmd.Parameters.Add("@Firstname", System.Data.SqlDbType.NVarChar, 50).Value = request.FirstName;
                cmd.Parameters.Add("@Lastname", System.Data.SqlDbType.NVarChar, 50).Value = request.LastName;
                cmd.Parameters.Add("@Role", System.Data.SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 1000).Value = hashedPassword;
                cmd.Parameters.Add("@Salt", System.Data.SqlDbType.VarBinary, 1000).Value = salt;
                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return 0;
                }
            }
        }
    }
}
