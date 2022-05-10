using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Models;
using WebApi.Models.CoachInfo;
using WebApi.Models.Employee;

namespace WebApi.Services
{
    public class CoachService
    {
        #region SQL

        const string GetCoachesSql = @"SELECT Coach.Id
                                             ,Coach.Description
                                       	     ,e.Id
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
                                       	     ,st.Id
                                       	     ,st.Name
                                       FROM Coach
                                       INNER JOIN Employee as e ON Coach.EmployeeId = e.Id
                                       INNER JOIN PositionType as pt ON e.Position = pt.Id
                                       INNER JOIN Gym as g ON e.Gym = g.Id
                                       INNER JOIN City as c ON g.City = c.Id
                                       INNER JOIN CoachSportType as cst ON Coach.Id = cst.Coach
                                       INNER JOIN SportType as st ON cst.SportType = st.Id";

        const string UpdateCoachSql = @"UPDATE Coach SET [Description] = @Description, UpdateDateTime = @UpdateDateTime;";

        const string DeleteCoachSportTypesSql = @"DELETE FROM CoachSportType WHERE Coach = @Id;";

        const string InsertSportTypesSql = @"INSERT INTO CoachSportType (Coach, SportType)
                                                 VALUES (@CoachId, @SportTypeId);";

        const string InsertIndividualCoachSql = @"INSERT INTO IndividualCoach (Coach, PricePerHour, CreateDateTime)
                                                      VALUES (@CoachId, @PricePerHour, @CreateDateTime);";

        const string GetIndividualCoachSql = @"SELECT Id FROM IndividualCoach WHERE Coach = @Id;";

        const string UpdateIndividualCoachSql = @"UPDATE IndividualCoach 
                                                      SET PricePerHour = @PricePerHour, UpdateDateTime = @UpdateDateTime 
                                                      WHERE Id = @Id;";

        const string DeleteIndividualCoachSql = @"DELETE FROM IndividualCoach WHERE Id = @Id;";

        const string InsertCoachSql = @"INSERT INTO Coach (EmployeeId, Description, CreateDateTime)
                                            VALUES (@EmployeeId, @Description, @CreateDateTime);";

        const string GetCoachIdSql = @"SELECT MAX(Id) FROM Coach;";

        const string GetIndividualCoachesSql = @"SELECT ic.Id
                                                       ,ic.PricePerHour
                                                       ,Coach.Id
                                                       ,Coach.Description
                                                       ,e.Id
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
                                                       ,st.Id
                                                       ,st.Name
                                                 FROM [SportComplex].[dbo].[IndividualCoach] as ic
                                                 INNER JOIN Coach ON ic.Coach = Coach.Id
                                                 INNER JOIN Employee as e ON Coach.EmployeeId = e.Id
                                                 INNER JOIN PositionType as pt ON e.Position = pt.Id
                                                 INNER JOIN Gym as g ON e.Gym = g.Id
                                                 INNER JOIN City as c ON g.City = c.Id
                                                 INNER JOIN CoachSportType as cst ON Coach.Id = cst.Coach
                                                 INNER JOIN SportType as st ON cst.SportType = st.Id";

        #endregion

        private readonly string ConnectionString;

        public CoachService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<Coach> GetByIdAsync(int id)
        {
            var sql = GetCoachesSql + "\nWHERE Coach.Id = @id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var coaches = await connection.QueryAsync<Coach, EmployeeInfo, PositionType, Gym, City, SportType, Coach>(
                sql,
                (coach, employee, position, gym, city, sportType) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;
                    coach.EmployeeInfo = employee;

                    if (coach.SportTypes is null)
                    {
                        coach.SportTypes = new List<SportType> { sportType };
                    }
                    else
                    {
                        coach.SportTypes.Add(sportType);
                    }

                    return coach;
                },
                splitOn: "Id",
                param: new { id });

            var result = coaches.GroupBy(c => c.Id).Select(c =>
            {
                var groupedCoach = c.First();
                groupedCoach.SportTypes = c.Select(c => c.SportTypes.FirstOrDefault()).ToList();
                return groupedCoach;
            });

            return result.FirstOrDefault();
        }

        public async Task<List<Coach>> GetAllAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var coaches = await connection.QueryAsync<Coach, EmployeeInfo, PositionType, Gym, City, SportType, Coach>(
                GetCoachesSql,
                (coach, employee, position, gym, city, sportType) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;
                    coach.EmployeeInfo = employee;

                    if (coach.SportTypes is null)
                    {
                        coach.SportTypes = new List<SportType> { sportType };
                    }
                    else
                    {
                        coach.SportTypes.Add(sportType);
                    }

                    return coach;
                },
                splitOn: "Id");

            var result = coaches.GroupBy(c => c.Id).Select(c =>
            {
                var groupedCoach = c.First();
                groupedCoach.SportTypes = c.Select(c => c.SportTypes.FirstOrDefault()).ToList();
                return groupedCoach;
            });

            return result.AsList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Coach WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(sql, new { id });

            return affectedRows == 1;
        }

        public async Task<Coach> CreateAsync(CoachApiModel coach)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int createdId;
            var transaction = connection.BeginTransaction("InsertCoach");
            try
            {

                await connection.ExecuteAsync(InsertCoachSql,
                                              new
                                              {
                                                  coach.EmployeeId,
                                                  coach.Description,
                                                  CreateDateTime = DateTime.Now
                                              },
                                              transaction);

                createdId = (int)await connection.ExecuteScalarAsync(GetCoachIdSql, transaction: transaction);

                foreach (int sportTypeId in coach.SportTypeIds)
                {
                    await connection.ExecuteAsync(InsertSportTypesSql, 
                                                  new 
                                                  { 
                                                      CoachId = createdId, 
                                                      SportTypeId = sportTypeId 
                                                  }, 
                                                  transaction);
                }

                if (coach.CanBeIndividual)
                {
                    await connection.ExecuteAsync(InsertIndividualCoachSql,
                                                  new 
                                                  { 
                                                      CoachId = createdId,
                                                      coach.PricePerHour,
                                                      CreateDateTime = DateTime.Now
                                                  },
                                                  transaction);
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }

            return await GetByIdAsync(createdId);
        }

        public async Task<Coach> UpdateAsync(CoachApiModel coach)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var transaction = connection.BeginTransaction("InsertCoach");
            try
            {

                await connection.ExecuteAsync(UpdateCoachSql,
                                              new
                                              {
                                                  coach.Description,
                                                  UpdateDateTime = DateTime.Now
                                              },
                                              transaction);

                await connection.ExecuteAsync(DeleteCoachSportTypesSql, new { coach.Id }, transaction);

                foreach (int sportTypeId in coach.SportTypeIds)
                {
                    await connection.ExecuteAsync(InsertSportTypesSql,
                                                  new
                                                  {
                                                      CoachId = coach.Id,
                                                      SportTypeId = sportTypeId
                                                  },
                                                  transaction);
                }

                int? individualCoachId = (int?)await connection.ExecuteScalarAsync(GetIndividualCoachSql, new { coach.Id }, transaction);

                if (coach.CanBeIndividual)
                {
                    if (individualCoachId.HasValue)
                    {
                        await connection.ExecuteAsync(UpdateIndividualCoachSql, 
                                                      new
                                                      { 
                                                          coach.PricePerHour,
                                                          UpdateDateTime = DateTime.Now,
                                                          Id = individualCoachId.Value 
                                                      }, 
                                                      transaction);
                    }
                    else
                    {
                        await connection.ExecuteAsync(InsertIndividualCoachSql,
                                                      new
                                                      {
                                                          CoachId = coach.Id,
                                                          coach.PricePerHour,
                                                          CreateDateTime = DateTime.Now
                                                      },
                                                      transaction);
                    }
                }
                else
                {
                    if (individualCoachId.HasValue)
                    {
                        await connection.ExecuteAsync(DeleteIndividualCoachSql, new {Id = individualCoachId.Value}, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }

            return await GetByIdAsync(coach.Id);
        }

        public async Task<List<IndividualCoach>> GetIndividualCoachesAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var individualCoaches = await connection.QueryAsync<IndividualCoach, Coach, EmployeeInfo, PositionType, Gym, City, SportType, IndividualCoach>(
                GetIndividualCoachesSql,
                (individualCoach, coach, employee, position, gym, city, sportType) =>
                {
                    gym.City = city;
                    employee.Gym = gym;
                    employee.Position = position;
                    coach.EmployeeInfo = employee;

                    if (coach.SportTypes is null)
                    {
                        coach.SportTypes = new List<SportType> { sportType };
                    }
                    else
                    {
                        coach.SportTypes.Add(sportType);
                    }

                    individualCoach.CoachInfo = coach;

                    return individualCoach;
                },
                splitOn: "Id");

            var result = individualCoaches.GroupBy(ic => ic.Id).Select(ic =>
            {
                var individualCoach = ic.First();
                individualCoach.CoachInfo.SportTypes = ic.Select(c => c.CoachInfo.SportTypes.FirstOrDefault()).ToList();
                return individualCoach;
            });

            return result.AsList();
        }
    }
}
