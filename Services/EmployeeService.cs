using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels.Employee;
using WebApi.Models.EmployeeInfo;

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

        private const int CoachPositionId = 2;

        private readonly string ConnectionString;

        public EmployeeService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<Employee> LoginAsync(string login, string password)
        {
            var sql = GetEmployeeSql + "\nWHERE e.Login LIKE @login";
            
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var employees = await connection.QueryAsync<Employee, PositionType, Gym, City, Employee>(
                sql,
                (employee, position, gym, city) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;

                    employee.Educations = GetEmployeeEducationsAsync(employee.Id).Result;
                    employee.PreviousJobs = GetEmployeePreviousJobsAsync(employee.Id).Result;

                    return employee;
                },
                splitOn: "Id",
                param: new { login, password });

            var employee = employees.FirstOrDefault();

            if (employee is null || !BCrypt.Net.BCrypt.Verify(password, employee.Password))
            {
                return null;
            }

            return employee;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var sql = GetEmployeeSql + "\nWHERE e.Id = @id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var employees = await connection.QueryAsync<Employee, PositionType, Gym, City, Employee>(
                sql,
                (employee, position, gym, city) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;

                    employee.Educations = GetEmployeeEducationsAsync(employee.Id).Result;
                    employee.PreviousJobs = GetEmployeePreviousJobsAsync(employee.Id).Result;

                    return employee;
                },
                splitOn: "Id",
                param: new { id });

            return employees.FirstOrDefault();
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var employees = await connection.QueryAsync<Employee, PositionType, Gym, City, Employee>(
                GetEmployeeSql,
                (employee, position, gym, city) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;

                    employee.Educations = GetEmployeeEducationsAsync(employee.Id).Result;
                    employee.PreviousJobs = GetEmployeePreviousJobsAsync(employee.Id).Result;

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

        public async Task<Employee> CreateAsync(EmployeeDto employee)
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
                                                                 Password = BCrypt.Net.BCrypt.HashPassword(employee.Password),
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

        public async Task<Employee> UpdateAsync(EmployeeDto employee)
        {
            const string sql = @"UPDATE Employee
                                 SET [FirstName]	  = @FirstName
                                    ,[LastName]		  = @LastName
                                    ,[PhoneNumber]	  = @PhoneNumber
                                    ,[Position]		  = @PositionId
                                    ,[UpdateDateTime] = @UpdateDateTime
                                    ,[HireDate]		  = @HireDate
                                    ,[Login]		  = @Login
                                    ,[Password]		  = @Password
                                    ,[DismissDate]	  = @DismissDate
                                    ,[Gym]			  = @GymId
                                 WHERE Id = @id";

            const string deleteCoachSql = @"DELETE FROM Coach WHERE Employee = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var oldEmployee = await GetByIdAsync(employee.Id);

            if (oldEmployee.Position.Id == CoachPositionId && employee.PositionId != CoachPositionId)
            {
                await connection.ExecuteAsync(deleteCoachSql, new { employee.Id });
            }

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

        public async Task<List<PositionType>> GetPositionTypesAsync()
        {
            const string sql = @"SELECT * FROM PositionType";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var positions = await connection.QueryAsync<PositionType>(sql);

            return positions.AsList();
        }

        #region Education

        public async Task<List<EducationLevel>> GetEducationLevelsAsync()
        {
            const string sql = @"SELECT Id, Name FROM EducationLevel";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var levels = await connection.QueryAsync<EducationLevel>(sql);

            return levels.AsList();
        }

        public async Task<List<Education>> GetEmployeeEducationsAsync(int id)
        {
            const string sql = @"SELECT ee.Id,ee.GraduationDate,u.Id,u.Name,el.Id,el.Name,es.Id,es.Name
                                 FROM EmployeeEducation as ee
                                 INNER JOIN University as u ON ee.University = u.Id
                                 INNER JOIN EducationLevel as el ON ee.Level = el.Id
                                 INNER JOIN EducationSpecialty as es ON ee.Specialty = es.Id
                                 WHERE ee.Employee = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var educations = await connection.QueryAsync<Education, University, EducationLevel, EducationSpecialty, Education>(
                sql, 
                (education, university, eduLevel, eduSpecialty) =>
                {
                    education.University = university;
                    education.Level = eduLevel;
                    education.Specialty = eduSpecialty;

                    return education;
                },
                param: new { id },
                splitOn: "Id");

            return educations.AsList();
        }

        public async Task AddEmployeeEducationAsync(EducationDto education)
        {
            const string sql = @"InsertEmployeeEducation";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                sql,
                new
                {
                    employeeId = education.EmployeeId,
                    universityName = education.University,
                    specialtyName = education.Specialty,
                    graduationDate = education.GraduationDate,
                    levelId = education.LevelId
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task UpdateEmployeeEducationAsync(EducationDto education)
        {
            const string sql = @"UpdateEmployeeEducation";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                sql,
                new
                {
                    education.Id,
                    universityName = education.University,
                    specialtyName = education.Specialty,
                    graduationDate = education.GraduationDate,
                    levelId = education.LevelId
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task DeleteEmployeeEducationAsync(int id)
        {
            const string sql = @"DELETE FROM EmployeeEducation WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, new { id });
        }

        #endregion

        #region PreviousJob

        public async Task<List<PreviousJob>> GetEmployeePreviousJobsAsync(int id)
        {
            const string sql = @"SELECT pj.Id,pj.StartDate,pj.EndDate,c.Id,c.Name
                                 FROM PreviousJob as pj
                                 INNER JOIN Company as c ON pj.Company = c.Id
                                 WHERE pj.Employee = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var previousJobs = await connection.QueryAsync<PreviousJob, Company, PreviousJob>(
                sql, 
                (job, company) =>
                {
                    job.Company = company;
                    return job;
                },
                param: new { id },
                splitOn: "Id");

            return previousJobs.AsList();
        }

        #endregion
    }
}
