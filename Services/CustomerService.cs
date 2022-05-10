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
    public class CustomerService
    {
        private const string GetCustomersSql = @"SELECT [Id]
                                                       ,[FirstName]
                                                       ,[LastName]
                                                       ,[PhoneNumber]
                                                 FROM [Customer]";

        private readonly string ConnectionString;

        public CustomerService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var sql = GetCustomersSql + "\nWHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var customers = await connection.QueryAsync<Customer>(sql, param: new { id });

            return customers.FirstOrDefault();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var Customers = await connection.QueryAsync<Customer>(GetCustomersSql);

            return Customers.AsList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Customer WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            const string insertSql = @"INSERT INTO Customer (FirstName, LastName, PhoneNumber, CreateDateTime)
                                       VALUES (@FirstName, @LastName, @PhoneNumber, @CreateDateTime);";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql,
                                                             new
                                                             {
                                                                 customer.FirstName,
                                                                 customer.LastName,
                                                                 customer.PhoneNumber,
                                                                 CreateDateTime = DateTime.Now
                                                             });
            if (affectedRows != 1)
            {
                return null;
            }

            const string getSql = @"SELECT MAX(Id) FROM Customer;";

            int createdId = (int)await connection.ExecuteScalarAsync(getSql);
            return await GetByIdAsync(createdId);
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            var sql = @"UPDATE Customer
                        SET FirstName = @FirstName, 
                        	LastName = @LastName, 
                        	PhoneNumber = @PhoneNumber, 
                        	UpdateDateTime = @UpdateDateTime
                        WHERE Id = @Id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql,
                                                             new
                                                             {
                                                                 customer.FirstName,
                                                                 customer.LastName,
                                                                 customer.PhoneNumber,
                                                                 UpdateDateTime = DateTime.Now,
                                                                 customer.Id
                                                             });

            if (affectedRows != 1)
            {
                return null;
            }

            return await GetByIdAsync(customer.Id);
        }
    }
}
