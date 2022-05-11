using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Models;

namespace WebApi.Services
{
    public class IndividualTrainingService
    {
        private readonly string ConnectionString;
        private readonly MembershipReceiptService _membershipReceiptService;
        private readonly CoachService _coachService;

        public IndividualTrainingService(IConfiguration configuration, MembershipReceiptService membershipReceiptService, CoachService coachService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _membershipReceiptService = membershipReceiptService;
            _coachService = coachService;
        }

        public async Task<List<IndividualTraining>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[MembershipReceipt]
                                       ,[PayedHours]
                                       ,[Price]
                                       ,[IndividualCoach]
                                       ,[PayementDateTime]
                                 FROM [IndividualTraining]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var trainings = await connection.QueryAsync<int, int, int, decimal, int, DateTime, IndividualTraining>(
                sql,
                (id, memReceiptId, payedHours, price, individualCoachId, payementDateTime) =>
                {
                    return new IndividualTraining
                    {
                        Id = id,
                        MembershipReceipt = _membershipReceiptService.GetByIdAsync(memReceiptId).Result,
                        PayedHours = payedHours,
                        Price = price,
                        Coach = _coachService.GetIndividualCoachByIdAsync(individualCoachId).Result,
                        PayementDateTime = payementDateTime
                    };
                },
                splitOn: "MembershipReceipt,PayedHours,Price,IndividualCoach,PayementDateTime");

            return trainings.AsList();
        }

        public async Task<IndividualTraining> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [MembershipReceipt]
                                       ,[PayedHours]
                                       ,[Price]
                                       ,[IndividualCoach]
                                       ,[PayementDateTime]
                                 FROM [IndividualTraining]
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var trainings = await connection.QueryAsync<int, int, decimal, int, DateTime, IndividualTraining>(
                sql,
                (memReceiptId, payedHours, price, individualCoachId, payementDateTime) =>
                {
                    return new IndividualTraining
                    {
                        Id = id,
                        MembershipReceipt = _membershipReceiptService.GetByIdAsync(memReceiptId).Result,
                        PayedHours = payedHours,
                        Price = price,
                        Coach = _coachService.GetIndividualCoachByIdAsync(individualCoachId).Result,
                        PayementDateTime = payementDateTime
                    };
                },
                param: new { id },
                splitOn: "PayedHours,Price,IndividualCoach,PayementDateTime");


            return trainings.FirstOrDefault();
        }

        public async Task<IndividualTraining> CreateAsync(IndividualTrainingDto individualTraining)
        {
            const string insertSql = @"INSERT INTO IndividualTraining (MembershipReceipt, PayedHours, Price, IndividualCoach, PayementDateTime, CreateDateTime)
                                       VALUES (@MembershipReceiptId, @PayedHours, @Price, @IndividualCoachId, @PayementDateTime, @CreateDateTime)";

            const string getIdSql = @"SELECT MAX(Id) FROM IndividualTraining";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                insertSql,
                new
                {
                    individualTraining.MembershipReceiptId,
                    individualTraining.PayedHours,
                    individualTraining.Price,
                    individualTraining.IndividualCoachId,
                    individualTraining.PayementDateTime,
                    CreateDateTime = DateTime.Now
                });

            if (affectedRows != 1)
            {
                return null;
            }

            int createdId = await connection.ExecuteScalarAsync<int>(getIdSql);

            return await GetByIdAsync(createdId);
        }

        public async Task<IndividualTraining> UpdateAsync(IndividualTrainingDto individualTraining)
        {
            const string updateSql = @"UPDATE IndividualTraining
                                       SET MembershipReceipt = @MembershipReceiptId,
                                       	   PayedHours = @PayedHours,
                                       	   Price = @Price,
                                       	   IndividualCoach = @IndividualCoachId,
                                       	   PayementDateTime = @PayementDateTime,
                                       	   UpdateDateTime = @UpdateDateTime
                                       WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                updateSql,
                new
                {
                    individualTraining.MembershipReceiptId,
                    individualTraining.PayedHours,
                    individualTraining.Price,
                    individualTraining.IndividualCoachId,
                    individualTraining.PayementDateTime,
                    UpdateDateTime = DateTime.Now,
                    individualTraining.Id
                });

            if (affectedRows != 1)
            {
                return null;
            }

            return await GetByIdAsync(individualTraining.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteSql = @"DELETE FROM IndividualTraining WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(deleteSql, new { id });

            return affectedRows == 1;
        }
    }
}
