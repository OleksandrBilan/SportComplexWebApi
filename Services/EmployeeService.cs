using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels.Employee;
using WebApi.Models.Employee;

namespace WebApi.Services
{
    public class EmployeeService
    { 
        private const string GetEmployeeSql = @"SELECT e.Id
                                                      ,e.FirstName
                                                      ,e.LastName
                                                      ,e.PhoneNumber
                                                      ,e.HireDate
                                                      ,e.Login
                                                      ,e.Password
                                                      ,e.DismissDate
	                                                  ,pt.Id
	                                                  ,pt.Name
                                                      ,g.Id
	                                                  ,g.Address
	                                                  ,g.PhoneNumber
	                                                  ,c.Id
	                                                  ,c.Name
                                                FROM Employee as e
                                                INNER JOIN PositionType as pt ON e.Position = pt.Id
                                                INNER JOIN Gym as g ON e.Gym = g.Id
                                                INNER JOIN City as c ON g.City = c.Id";

        private readonly string ConnectionString;

        public EmployeeService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<EmployeeInfo> LoginAsync(string login, string password)
        {
            var sql = GetEmployeeSql + "\nWHERE e.Login = @login AND e.Password = @password;";
            
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var employees = await connection.QueryAsync<EmployeeInfo, PositionType, Gym, City, EmployeeInfo>(
                sql,
                (employee, position, gym, city) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;

                    return employee;
                },
                splitOn: "Id",
                param: new { login, password });

            return employees.FirstOrDefault();
        }

        public async Task<EmployeeInfo> GetByIdAsync(int id)
        {
            var sql = GetEmployeeSql + "\nWHERE e.Id = @id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var employees = await connection.QueryAsync<EmployeeInfo, PositionType, Gym, City, EmployeeInfo>(
                sql,
                (employee, position, gym, city) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;

                    return employee;
                },
                splitOn: "Id",
                param: new { id });

            return employees.FirstOrDefault();
        }

        public async Task<List<EmployeeInfo>> GetAllAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var employees = await connection.QueryAsync<EmployeeInfo, PositionType, Gym, City, EmployeeInfo>(
                GetEmployeeSql,
                (employee, position, gym, city) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;

                    return employee;
                },
                splitOn: "Id");

            return employees.AsList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Employee WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }

        public async Task<EmployeeInfo> CreateAsync(EmployeeApiModel employee)
        {
            const string insertSql = @"INSERT INTO Employee ([FirstName],[LastName],[PhoneNumber],[Position],[CreateDateTime],[HireDate],[DismissDate],[Login],[Password],[Gym])
                                       VALUES (@FirstName,@LastName,@PhoneNumber,@PositionId,@CreateDateTime,@HireDate,@DismissDate,@Login,@Password,@GymId);";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql,
                                                             new
                                                             {
                                                                 employee.FirstName,
                                                                 employee.LastName,
                                                                 employee.PhoneNumber,
                                                                 employee.PositionId,
                                                                 CreateDateTime = DateTime.Now,
                                                                 employee.HireDate,
                                                                 employee.DismissDate,
                                                                 employee.Login,
                                                                 employee.Password,
                                                                 employee.GymId
                                                             });
            if (affectedRows != 1)
            {
                return null;
            }

            const string getSql = @"SELECT MAX(Id) FROM Employee;";

            int createdId = (int)await connection.ExecuteScalarAsync(getSql);
            return await GetByIdAsync(createdId);
        }

        public async Task<EmployeeInfo> UpdateAsync(EmployeeApiModel employee)
        {
            var sql = @"UPDATE Employee
                        SET [FirstName]		 = @FirstName
                           ,[LastName]		 = @LastName
                           ,[PhoneNumber]	 = @PhoneNumber
                           ,[Position]		 = @PositionId
                           ,[UpdateDateTime] = @UpdateDateTime
                           ,[HireDate]		 = @HireDate
                           ,[Login]			 = @Login
                           ,[Password]		 = @Password
                           ,[DismissDate]	 = @DismissDate
                           ,[Gym]			 = @GymId
                        WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql,
                                                             new
                                                             {
                                                                 employee.FirstName,
                                                                 employee.LastName,
                                                                 employee.PhoneNumber,
                                                                 employee.PositionId,
                                                                 UpdateDateTime = DateTime.Now,
                                                                 employee.HireDate,
                                                                 employee.DismissDate,
                                                                 employee.Login,
                                                                 employee.Password,
                                                                 employee.GymId,
                                                                 employee.Id
                                                             });

            if (affectedRows != 1)
            {
                return null;
            }

            return await GetByIdAsync(employee.Id);
        }
    }
}
