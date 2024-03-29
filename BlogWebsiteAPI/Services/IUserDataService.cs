﻿using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Requests;
using BlogWebsiteAPI.Requests.UserRequests;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BlogWebsiteAPI.Services
{
    public interface IUserDataService
    {
        public int InsertNewUser(CreateUser.Request request, byte[] salt, string hashedPassword);
        public bool UsernameExistsCheck(string username);
        public UserPasswordCheckModel GetPasswordVerificationRequirements(string username);
        public User GetUser(int userId);
        public int GetUserId(string username);
        public string GetUserRole(int userId);
        public int UpdateUserRole(int promoterId, int userId, string newRole);
        public int DeactivateUser(int userId, int deactivatorId, string reasonForDeactivation);
        public int DeleteUser(int userId);
    }

    public class SqlUserDataService : IUserDataService
    {
        private readonly string CONNECTIONSTRING;
        private readonly IConfiguration _config;

        public SqlUserDataService(IConfiguration config)
        {
            _config = config;
            CONNECTIONSTRING = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
        }

        public int DeactivateUser(int userId, int deactivatorId, string reasonForDeactivation)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.DeactivateUser @UserId, @DeactivatorId, @Date, @Reason";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@DeactivatorId", SqlDbType.Int).Value = deactivatorId;
                    cmd.Parameters.Add("@Date", SqlDbType.Date).Value = DateTime.Now.Date;
                    cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 500).Value = reasonForDeactivation;

                    try
                    {
                        var result =  cmd.ExecuteNonQuery();
                        conn.Close();
                        return result;
                    } catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception(ex.Message, ex);
                    }
                }
            }

        }

        public int DeleteUser(int userId)
        {
            using(SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.DeleteUser @UserId";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    try
                    {
                        var result = cmd.ExecuteNonQuery();
                        conn.Close();
                        return result;
                    } catch(Exception ex)
                    {
                        conn.Close();
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        public UserPasswordCheckModel GetPasswordVerificationRequirements(string username)
        {
            UserPasswordCheckModel passwordCheck = new UserPasswordCheckModel();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.UserLogInPasswordCheck @Username";
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 100).Value = username;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                passwordCheck.HashedPassword = (string)reader["Password"];
                                passwordCheck.Salt = (byte[])reader["Salt"];
                                passwordCheck.UserId = (int)reader["UserId"];
                            }
                            reader.Close();
                        } catch (Exception ex)
                        {
                            reader.Close();
                            conn.Close();
                            throw new Exception(ex.Message,ex);
                        }
                    }
                }
                conn.Close();
                return passwordCheck;
            }
        }

        public User GetUser(int userId)
        {
            User user = new User();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.GetUser @UserId";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while(reader.Read())
                            {
                                user.Username = (string)reader["Username"];
                                user.FirstName = (string)reader["FirstName"];
                                user.LastName = (string)reader["LastName"];
                                user.Role = (string)reader["Role"];
                            }
                        } catch (Exception ex)
                        {
                            reader.CloseAsync();
                            conn.Close();
                            throw new Exception(ex.Message,ex);
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }

            return user;
        }

        public int GetUserId(string username)
        {
            //var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
            int result;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.GetUserId @Username";
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                    try
                    {
                        result = (int)cmd.ExecuteScalar();
                        conn.Close();
                    } catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception(ex.Message,ex);
                    }
                }
            }
            return result;
        }

        public string GetUserRole(int userId)
        {
           // var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.GetUserRole @UserId";
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    try
                    {
                        var result = (string)cmd.ExecuteScalar();
                        conn.Close();
                        return result;
                    } catch(Exception ex)
                    {
                        conn.Close();
                        throw new Exception(ex.Message,ex);
                    }
                }
            }
        }

        public int InsertNewUser(CreateUser.Request request, byte[] salt, string hashedPassword)
        {
            //var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.CreateUser @Username, @Firstname, @Lastname, @Role, @Password, @Salt;";
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 30).Value = request.Username;
                    cmd.Parameters.Add("@Firstname", SqlDbType.NVarChar, 50).Value = request.FirstName;
                    cmd.Parameters.Add("@Lastname", SqlDbType.NVarChar, 50).Value = request.LastName;
                    cmd.Parameters.Add("@Role", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 1000).Value = hashedPassword;
                    cmd.Parameters.Add("@Salt", SqlDbType.VarBinary, 1000).Value = salt;
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

        public int UpdateUserRole(int promoterId, int userId, string newRole)
        {
            //var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
            int result;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "EXEC dbo.UpdateUserRole @PromoterId, @PromoteeId, @NewRole, @DateOfChange";
                    cmd.Parameters.Add("@PromoterId", SqlDbType.Int).Value = promoterId;
                    cmd.Parameters.Add("@PromoteeId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@NewRole", SqlDbType.VarChar, 30).Value = newRole;
                    cmd.Parameters.Add("@DateOfChange", SqlDbType.Date).Value = DateTime.Now.Date;

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                        conn.Close();
                        return result;
                    } catch (Exception ex)
                    {
                        conn.Close();
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        public bool UsernameExistsCheck(string username)
        {
           // var connString = _config.GetSection("DataBase").GetSection("SqlConnectionString").Value;
            using(SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "EXEC dbo.UsernameTakenCheck @username";
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;
                int result;
                try
                {
                    result = (int)cmd.ExecuteScalar();
                } catch (Exception ex)
                {
                    conn.Close();
                    throw new Exception(ex.Message, ex);
                }
                conn.Close();
                if(result == 0)
                    return false;
                else
                    return true;
            }
        }
    }
}
