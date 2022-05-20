using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.EmployeeInfo;

namespace WebApi.Services
{
    public class CityService
    {
        private readonly string ConnectionString;

        public CityService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<List<City>> GetAllAsync()
        {
            const string sql = @"SELECT Id, Name FROM City";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var cities = await connection.QueryAsync<City>(sql);

            return cities.AsList();
        }

        public async Task<City> GetByIdAsync(int id)
        {
            const string sql = @"SELECT Id, Name FROM City WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var cities = await connection.QueryAsync<City>(sql, new { id });

            return cities.FirstOrDefault();
        }

        public async Task CreateAsync(City city)
        {
            const string sql = @"INSERT INTO City (Name, CreateDateTime)
                                 VALUES (@Name, @CreateDateTime)";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, new { city.Name, CreateDateTime = DateTime.Now });
        }

        public async Task UpdateAsync(City city)
        {
            const string sql = @"UPDATE City 
                                 SET Name = @Name,
                                     UpdateDateTime = @UpdateDateTime
                                 WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, new { city.Id, city.Name, UpdateDateTime = DateTime.Now });
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM City WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
