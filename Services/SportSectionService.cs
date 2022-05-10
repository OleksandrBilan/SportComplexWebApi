using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Models.GroupTrainingSubscription;

namespace WebApi.Services
{
    public class SportSectionService
    {
        private readonly string ConnectionString;
        private readonly SportTypeService _sportTypeService;

        public SportSectionService(IConfiguration configuration, SportTypeService sportTypeService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _sportTypeService = sportTypeService;
        }

        public async Task<List<SportSection>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[Name]
                                       ,[SportType]
                                       ,[Description]
                                 FROM [SportSection]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sportSections = await connection.QueryAsync<int, string, int, string, SportSection>(
                sql,
                (id, name, sportTypeId, description) =>
                {
                    return new SportSection
                    {
                        Id = id,
                        Name = name,
                        Description = description,
                        SportType = _sportTypeService.GetByIdAsync(sportTypeId).Result
                    };
                },
                splitOn: "Name,SportType,Description");

            return sportSections.AsList();
        }

        public async Task<SportSection> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [Id]
                                       ,[Name]
                                       ,[SportType]
                                       ,[Description]
                                 FROM [SportSection]
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sportSections = await connection.QueryAsync<int, string, int, string, SportSection>(
                sql,
                (id, name, sportTypeId, description) =>
                {
                    return new SportSection
                    {
                        Id = id,
                        Name = name,
                        Description = description,
                        SportType = _sportTypeService.GetByIdAsync(sportTypeId).Result
                    };
                },
                param: new { id },
                splitOn: "Name,SportType,Description");

            return sportSections.FirstOrDefault();
        }

        public async Task<SportSection> CreateAsync(SportSectionApiModel sportSection)
        {
            const string insertSql = @"INSERT INTO SportSection (Name, Description, SportType, CreateDateTime)
                                       VALUES (@Name, @Description, @SportTypeId, @CreateDateTime);";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql,
                                                             new
                                                             {
                                                                 sportSection.Name,
                                                                 sportSection.Description,
                                                                 sportSection.SportTypeId,
                                                                 CreateDateTime = DateTime.Now
                                                             });

            if (affectedRows != 1)
            {
                return null;
            }

            const string getSql = @"SELECT MAX(Id) FROM SportSection;";

            int createdId = await connection.ExecuteScalarAsync<int>(getSql);

            return await GetByIdAsync(createdId);
        }
        
        public async Task<SportSection> UpdateAsync(SportSectionApiModel sportSection)
        {
            const string updateSql = @"UPDATE SportSection 
                                      SET Name = @Name,
	                                      Description = @Description,
	                                      SportType = @SportTypeId,
	                                      UpdateDateTime = @UpdateDateTime
                                      WHERE Id = @Id;";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(updateSql,
                                          new
                                          {
                                              sportSection.Name,
                                              sportSection.Description,
                                              sportSection.SportTypeId,
                                              UpdateDateTime = DateTime.Now,
                                              sportSection.Id
                                          });

            return await GetByIdAsync(sportSection.Id);
        }      
        
        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM SportSection WHERE Id = @id;";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }
    }
}
