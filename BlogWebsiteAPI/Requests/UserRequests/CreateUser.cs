using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Services;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class CreateUser
    {
        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IConfiguration _config;

            public Handler(IConfiguration config)
            {
                _config = config;
            }
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
                var salt = RandomNumberGenerator.GetBytes(12);
                var hashedPassword = UserRequestFunctions.PasswordHash(request.Password, salt);

                using(SqlConnection conn = new SqlConnection(connString))
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
                      var rowsAffected = cmd.ExecuteNonQuery();
                        if(rowsAffected > 0)
                        {
                            return Task.FromResult(new Response(true));
                        } else
                        {
                            return Task.FromResult(new Response(false));
                        }
                    } catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public class Response
        {
            public Response(bool success)
            {
                Success = success;
            }
            public bool Success { get; set; }
        }
    }
}
