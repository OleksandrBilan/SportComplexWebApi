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
    public class SubscriptionReceiptService
    {
        private readonly string ConnectionString;
        private readonly EmployeeService _employeeService;
        private readonly CustomerService _customerService;
        private readonly SubscriptionTypeService _subscriptionTypeService;

        public SubscriptionReceiptService(IConfiguration configuration, EmployeeService employeeService, 
                                          CustomerService customerService, SubscriptionTypeService subscriptionTypeService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _employeeService = employeeService;
            _customerService = customerService;
            _subscriptionTypeService = subscriptionTypeService;
        }

        public async Task<List<SubscriptionReceipt>> GetAllAsync()
        {
            const string sql = @"SELECT Id
                                       ,Employee
                                       ,Customer
                                       ,SubscriptionType
                                       ,ExpireDate
                                       ,IsPayed
                                       ,IsActive
                                 FROM SubscriptionReceipt";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var receipts = await connection.QueryAsync<int, int, int, int, DateTime, bool, bool, SubscriptionReceipt>(
                sql,
                (id, employeeId, customerId, subscriptionTypeId, expireDate, isPayed, isActive) =>
                {
                    return new SubscriptionReceipt
                    {
                        Id = id,
                        Seller = _employeeService.GetByIdAsync(employeeId).Result,
                        Customer = _customerService.GetByIdAsync(customerId).Result,
                        SubscriptionType = _subscriptionTypeService.GetByIdAsync(subscriptionTypeId).Result,
                        ExpireDate = expireDate,
                        IsPayed = isPayed,
                        IsActive = isActive
                    };
                },
                splitOn: "Employee,Customer,SubscriptionType,ExpireDate,IsPayed,IsActive");

            return receipts.AsList();
        }

        public async Task<SubscriptionReceipt> GetByIdAsync(int id)
        {
            const string sql = @"SELECT Id
                                       ,Employee
                                       ,Customer
                                       ,SubscriptionType
                                       ,ExpireDate
                                       ,IsPayed
                                       ,IsActive
                                 FROM SubscriptionReceipt
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var receipts = await connection.QueryAsync<int, int, int, int, DateTime, bool, bool, SubscriptionReceipt>(
                sql,
                (id, employeeId, customerId, subscriptionTypeId, expireDate, isPayed, isActive) =>
                {
                    return new SubscriptionReceipt
                    {
                        Id = id,
                        Seller = _employeeService.GetByIdAsync(employeeId).Result,
                        Customer = _customerService.GetByIdAsync(customerId).Result,
                        SubscriptionType = _subscriptionTypeService.GetByIdAsync(subscriptionTypeId).Result,
                        ExpireDate = expireDate,
                        IsPayed = isPayed,
                        IsActive = isActive
                    };
                },
                param: new { id },
                splitOn: "Employee,Customer,SubscriptionType,ExpireDate,IsPayed,IsActive");

            return receipts.FirstOrDefault();
        }

        public async Task<SubscriptionReceipt> CreateAsync(SubscriptionReceiptDto subscriptionReceipt)
        {
            const string insertSql = @"INSERT INTO SubscriptionReceipt (Employee, Customer, SubscriptionType, ExpireDate, IsPayed, IsActive, CreateDateTime)
                                       VALUES (@EmployeeId, @CustomerId, @SubscriptionTypeId, @ExpireDate, @IsPayed, @IsActive, @CreateDateTime);";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql,
                                                             new
                                                             {
                                                                 EmployeeId = subscriptionReceipt.SellerId,
                                                                 subscriptionReceipt.CustomerId,
                                                                 subscriptionReceipt.SubscriptionTypeId,
                                                                 subscriptionReceipt.ExpireDate,
                                                                 subscriptionReceipt.IsPayed,
                                                                 subscriptionReceipt.IsActive,
                                                                 CreateDateTime = DateTime.Now
                                                             });

            if (affectedRows != 1)
            {
                return null;
            }

            const string getSql = @"SELECT MAX(Id) FROM SubscriptionReceipt;";

            int createdId = await connection.ExecuteScalarAsync<int>(getSql);

            return await GetByIdAsync(createdId);
        }

        public async Task<SubscriptionReceipt> UpdateAsync(SubscriptionReceiptDto subscriptionReceipt)
        {
            const string updateSql = @"UPDATE SubscriptionReceipt
                                       SET Employee = @EmployeeId,
	                                       Customer = @CustomerId,
	                                       SubscriptionType = @SubscriptionTypeId,
	                                       ExpireDate = @ExpireDate,
	                                       IsPayed = @IsPayed,
	                                       IsActive = @IsActive,
                                           UpdateDateTime = @UpdateDateTime
                                       WHERE Id = @Id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(updateSql,
                                                             new
                                                             {
                                                                 EmployeeId = subscriptionReceipt.SellerId,
                                                                 subscriptionReceipt.CustomerId,
                                                                 subscriptionReceipt.SubscriptionTypeId,
                                                                 subscriptionReceipt.ExpireDate,
                                                                 subscriptionReceipt.IsPayed,
                                                                 subscriptionReceipt.IsActive,
                                                                 UpdateDateTime = DateTime.Now,
                                                                 subscriptionReceipt.Id
                                                             });

            if (affectedRows != 1)
            {
                return null;
            }

            return await GetByIdAsync(subscriptionReceipt.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM SubscriptionReceipt WHERE Id = @id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }
    }
}
