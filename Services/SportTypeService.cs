using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public class SportTypeService
    {
        private readonly string ConnectionString;

        public SportTypeService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<List<SportType>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[Name]
                                       ,[CreateDateTime]
                                       ,[UpdateDateTime]
                                 FROM [SportType]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sportTypes = await connection.QueryAsync<SportType>(sql);
            return sportTypes.AsList();
        }

        public async Task<SportType> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [Id]
                                       ,[Name]
                                       ,[CreateDateTime]
                                       ,[UpdateDateTime]
                                 FROM [SportType]
                                 WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sportTypes = await connection.QueryAsync<SportType>(sql, new { id });
            return sportTypes.FirstOrDefault();
        }

        public async Task<SportType> CreateAsync(string name)
        {
            const string insertSql = @"INSERT INTO SportType (Name, CreateDateTime) VALUES (@name, @CreateDateTime);";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql, new { name, CreateDateTime = DateTime.Now });

            if (affectedRows != 1)
            {
                return null;
            }

            const string getSql = @"SELECT MAX(Id) FROM SportType;";

            int createdId = await connection.ExecuteScalarAsync<int>(getSql);

            return await GetByIdAsync(createdId);
        }

        public async Task<SportType> UpdateAsync(SportType sportType)
        {
            const string sql = @"UPDATE SportType SET Name = @Name WHERE Id = @Id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, sportType);

            return await GetByIdAsync(sportType.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"DELETE FROM SportType WHERE Id = @id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }
    }
}
