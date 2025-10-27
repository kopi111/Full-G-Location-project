using GLocation.Domain.DTOs;
using GLocation.Domain.Interface.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace GLocation.Infrastructure.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly ILogger<UserRepositories> _logger;
        private readonly IConfiguration _configuration;

        public UserRepositories(ILogger<UserRepositories> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)await cmd.ExecuteScalarAsync();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists.");
                throw;
            }
        }


        public bool AddUser(CreateUser request)
        {
            string connectionString = "your_connection_string_here";
            string query = @"INSERT INTO Users (Email, Password, FirstName, LastName, Phone)
                     VALUES (@Email, @Password, @FirstName, @LastName, @Phone)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", request.email);
                        cmd.Parameters.AddWithValue("@Password", request.password);
                        cmd.Parameters.AddWithValue("@FirstName", request.firstName);
                        cmd.Parameters.AddWithValue("@LastName", request.lastName);
                        cmd.Parameters.AddWithValue("@Phone", request.phone);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


    }



}