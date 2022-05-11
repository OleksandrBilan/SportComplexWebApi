using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels.Membership;
using WebApi.Models.Membership;

namespace WebApi.Services
{
    public class MembershipReceiptService
    {
        private readonly string ConnectionString;
        private readonly MembershipTypeService _membershipTypeService;
        private readonly EmployeeService _employeeService;
        private readonly CustomerService _customerService;


        public MembershipReceiptService(IConfiguration configuration, MembershipTypeService membershipTypeService,
                                        EmployeeService employeeService, CustomerService customerService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _membershipTypeService = membershipTypeService;
            _employeeService = employeeService;
            _customerService = customerService;
        }

        public async Task<List<MembershipReceipt>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[Customer]
                                       ,[Employee]
                                       ,[MembershipType]
                                       ,[PayementDateTime]
                                 FROM [MembershipReceipt]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var receipts = await connection.QueryAsync<int, int, int, int, DateTime?, MembershipReceipt>(
                sql,
                (id, customerId, sellerId, memTypeId, payementDateTime) =>
                {
                    return new MembershipReceipt
                    {
                        Id = id,
                        Customer = _customerService.GetByIdAsync(customerId).Result,
                        Seller = _employeeService.GetByIdAsync(sellerId).Result,
                        MembershipType = _membershipTypeService.GetByIdAsync(memTypeId).Result,
                        PayementDateTime = payementDateTime
                    };
                },
                splitOn: "Customer,Employee,MembershipType,PayementDateTime");

            return receipts.AsList();
        }

        public async Task<MembershipReceipt> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [Id]
                                       ,[Customer]
                                       ,[Employee]
                                       ,[MembershipType]
                                       ,[PayementDateTime]
                                 FROM [MembershipReceipt]
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var receipts = await connection.QueryAsync<int, int, int, int, DateTime?, MembershipReceipt>(
                sql,
                (id, customerId, sellerId, memTypeId, payementDateTime) =>
                {
                    return new MembershipReceipt
                    {
                        Id = id,
                        Seller = _employeeService.GetByIdAsync(sellerId).Result,
                        Customer = _customerService.GetByIdAsync(customerId).Result,
                        MembershipType = _membershipTypeService.GetByIdAsync(memTypeId).Result,
                        PayementDateTime = payementDateTime
                    };
                },
                param: new { id },
                splitOn: "Customer,Employee,MembershipType,PayementDateTime");

            return receipts.FirstOrDefault();
        }

        public async Task<MembershipReceipt> CreateAsync(MembershipReceiptDto membershipReceipt)
        {
            const string insertSql = @"INSERT INTO MembershipReceipt (Customer, Employee, MembershipType, PayementDateTime, CreateDateTime)
                                       VALUES (@CustomerId, @SellerId, @MembershipTypeId, @PayementDateTime, @CreateDateTime)";

            const string getIdSql = @"SELECT MAX(Id) FROM MembershipReceipt";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                                                insertSql,
                                                new
                                                {
                                                    membershipReceipt.CustomerId,
                                                    membershipReceipt.SellerId,
                                                    membershipReceipt.MembershipTypeId,
                                                    membershipReceipt.PayementDateTime,
                                                    CreateDateTime = DateTime.Now
                                                });

            if (affectedRows != 1)
            {
                return null;
            }

            int createdId = await connection.ExecuteScalarAsync<int>(getIdSql);

            return await GetByIdAsync(createdId);
        }

        public async Task<MembershipReceipt> UpdateAsync(MembershipReceiptDto membershipReceipt)
        {
            const string updateSql = @"UPDATE MembershipReceipt
                                       SET Customer = @CustomerId,
                                       	   Employee = @SellerId,
                                       	   MembershipType = @MembershipTypeId,
                                       	   PayementDateTime = @PayementDateTime,
                                       	   UpdateDateTime = @UpdateDateTime
                                       WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                                                updateSql,
                                                new
                                                {
                                                    membershipReceipt.CustomerId,
                                                    membershipReceipt.SellerId,
                                                    membershipReceipt.MembershipTypeId,
                                                    membershipReceipt.PayementDateTime,
                                                    UpdateDateTime = DateTime.Now,
                                                    membershipReceipt.Id
                                                });

            if (affectedRows != 1)
            {
                return null;
            }

            return await GetByIdAsync(membershipReceipt.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteSql = @"DELETE FROM MembershipReceipt WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(deleteSql, new { id });

            return affectedRows == 1;
        }
    }
}
