using Dapper;
using Microsoft.Extensions.Configuration;
using MVCJustEatClone.Extensions;
using MVCJustEatClone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            Connection = configuration.GetConnectionString("DefaultConnection");
        }

        public string Connection { get; set; }

        public async Task<bool> CheckUserExistsAsync(string email)
        {
            var parameters = new { Email = email };

            using (var conn = new SqlConnection(Connection))
            {
                var query = @" SELECT COUNT(UserId) as Count
                  FROM Users 
                  WHERE Email = @Email";
                var count = await conn.ExecuteScalarAsync<int>(query, parameters);

                return (count == 1);
            }
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            var parameters = new 
            {
                Email = email ,
                Password = password.Sha256()
            };

            using (var conn = new SqlConnection(Connection))
            {
                var query = @"  SELECT  UserId, Email, FirstName, Surname
                      FROM Users
                      WHERE Email = @Email
                      AND Password = @Password";
                return await conn.QueryFirstOrDefaultAsync<User>(query, parameters);
            }
        }

        public async Task<User> RegisterUser(User user)
        {
            var parameters = new
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Password = user.Password.Sha256(),
                DateRegistered = DateTime.Now.ToString("s")
            };

            using (var conn = new SqlConnection(Connection))
            {
                var query = @"INSERT INTO Users (Email, FirstName, Surname, Password, DateRegistered)
                    OUTPUT INSERTED.UserId
                    VALUES (@Email, @FirstName, @Surname, @Password, @DateRegistered);";
                var result = await conn.ExecuteScalarAsync<int>(query, parameters);
                user.UserId = result;
            };

            return user;
        }
    }
}
