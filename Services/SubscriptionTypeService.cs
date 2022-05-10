using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels.GroupTrainingSubscription;
using WebApi.Models.GroupTrainingSubscription;

namespace WebApi.Services
{
    public class SubscriptionTypeService
    {
        private readonly string ConnectionString;
        private readonly SportSectionService _sportSectionService;

        public SubscriptionTypeService(IConfiguration configuration, SportSectionService sportSectionService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _sportSectionService = sportSectionService;
        }

        public async Task<List<SubscriptionType>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[SportSection]
                                       ,[AvailableTrainingsCount]
                                       ,[Price]
                                 FROM [SubscriptionType]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var subscriptionTypes = await connection.QueryAsync<int, int, int, decimal, SubscriptionType>(
                sql,
                (id, sportSectionId, availableTrainingsCount, price) =>
                {
                    return new SubscriptionType
                    {
                        Id = id,
                        AvailableTrainingsCount = availableTrainingsCount,
                        Price = price,
                        SportSection = _sportSectionService.GetByIdAsync(sportSectionId).Result
                    };
                },
                splitOn: "SportSection,AvailableTrainingsCount,Price");

            return subscriptionTypes.AsList();
        }

        public async Task<SubscriptionType> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [Id]
                                       ,[SportSection]
                                       ,[AvailableTrainingsCount]
                                       ,[Price]
                                 FROM [SubscriptionType]
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var subscriptionTypes = await connection.QueryAsync<int, int, int, decimal, SubscriptionType>(
                sql,
                (id, sportSectionId, availableTrainingsCount, price) =>
                {
                    return new SubscriptionType
                    {
                        Id = id,
                        AvailableTrainingsCount = availableTrainingsCount,
                        Price = price,
                        SportSection = _sportSectionService.GetByIdAsync(sportSectionId).Result
                    };
                },
                param: new { id },
                splitOn: "SportSection,AvailableTrainingsCount,Price");

            return subscriptionTypes.FirstOrDefault();
        }

        public async Task<SubscriptionType> CreateAsync(SubscriptionTypeApiModel subscriptionType)
        {
            const string insertSql = @"INSERT INTO SubscriptionType (SportSection, AvailableTrainingsCount, Price, CreateDateTime)
                                       VALUES (@SportSectionId, @AvailableTrainingsCount, @Price, @CreateDateTime);";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql,
                                                             new
                                                             {
                                                                 subscriptionType.SportSectionId,
                                                                 subscriptionType.AvailableTrainingsCount,
                                                                 subscriptionType.Price,
                                                                 CreateDateTime = DateTime.Now
                                                             });

            if (affectedRows == 1)
            {
                const string getSql = @"SELECT MAX(Id) FROM SubscriptionType;";

                int createdId = (int)await connection.ExecuteScalarAsync(getSql);

                return await GetByIdAsync(createdId);
            }
            else
            {
                return null;
            }
        }

        public async Task<SubscriptionType> UpdateAsync(SubscriptionTypeApiModel subscriptionType)
        {
            var updateSql = @"UPDATE SubscriptionType
                              SET SportSection = @SportSectionId,
	                              Price = @Price,
	                              AvailableTrainingsCount = @AvailableTrainingsCount,
	                              UpdateDateTime = @UpdateDateTime
                              WHERE Id = @Id;";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(updateSql,
                                          new
                                          {
                                              subscriptionType.Id,
                                              subscriptionType.SportSectionId,
                                              subscriptionType.Price,
                                              subscriptionType.AvailableTrainingsCount,
                                              UpdateDateTime = DateTime.Now
                                          });

            return await GetByIdAsync(subscriptionType.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"DELETE FROM SubscriptionType WHERE Id = @id";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }
    }
}
