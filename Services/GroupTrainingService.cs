using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Models;
using WebApi.Models.GroupTrainingSubscription;

namespace WebApi.Services
{
    public class GroupTrainingService
    {
        private readonly string ConnectionString;
        private readonly SubscriptionReceiptService _subscriptionReceiptService;
        private readonly GroupService _groupService;

        public GroupTrainingService(IConfiguration configuration, SubscriptionReceiptService subscriptionReceiptService, GroupService groupService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _subscriptionReceiptService = subscriptionReceiptService;
            _groupService = groupService;
        }

        public async Task<List<GroupTraining>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[StartDateTime]
                                       ,[SubscriptionReceipt]
                                       ,[Group]
                                   FROM [GroupTraining]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var trainings = await connection.QueryAsync<GroupTraining, int, int, GroupTraining>(
                sql,
                (training, receiptId, groupId) =>
                {
                    training.Receipt = new SubscriptionReceipt { Id = receiptId };
                    training.Group = new Group { Id = groupId };

                    return training;
                },
                splitOn: "SubscriptionReceipt,Group");

            foreach (var training in trainings)
            {
                training.Receipt = await _subscriptionReceiptService.GetByIdAsync(training.Receipt.Id);
                training.Group = await _groupService.GetByIdAsync(training.Group.Id);
            }

            return trainings.AsList();
        }

        public async Task<GroupTraining> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [Id]
                                       ,[StartDateTime]
                                       ,[SubscriptionReceipt]
                                       ,[Group]
                                   FROM [GroupTraining]
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var trainings = await connection.QueryAsync<GroupTraining, int, int, GroupTraining>(
                sql,
                (training, receiptId, groupId) =>
                {
                    training.Receipt = new SubscriptionReceipt { Id = receiptId };
                    training.Group = new Group { Id = groupId };

                    return training;
                },
                splitOn: "SubscriptionReceipt,Group");

            foreach (var training in trainings)
            {
                training.Receipt = await _subscriptionReceiptService.GetByIdAsync(training.Receipt.Id);
                training.Group = await _groupService.GetByIdAsync(training.Group.Id);
            }

            return trainings.FirstOrDefault();
        }

        public async Task CreateAsync(GroupTrainingDto training)
        {
            const string sql = @"INSERT INTO GroupTraining (StartDateTime, SubscriptionReceipt, [Group], CreateDateTime)
                                 VALUES (@StartDateTime, @ReceiptId, @GroupId, @CreateDateTime)";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                sql,
                new
                {
                    training.StartDateTime,
                    training.ReceiptId,
                    training.GroupId,
                    CreateDateTime = DateTime.Now
                });
        }

        public async Task UpdateAsync(GroupTrainingDto training)
        {
            const string sql = @"UPDATE GroupTraining
                                 SET StartDateTime = @StartDateTime,
	                                 SubscriptionReceipt = @ReceiptId,
	                                 [Group] = @GroupId,
	                                 UpdateDateTime = @UpdateDateTime
                                 WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                sql,
                new
                {
                    training.StartDateTime,
                    training.ReceiptId,
                    training.GroupId,
                    UpdateDateTime = DateTime.Now,
                    training.Id
                });
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM GroupTraining WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
