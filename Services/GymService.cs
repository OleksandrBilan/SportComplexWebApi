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
    public class GymService
    {
        private readonly string ConnectionString;

        public GymService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
        }

        public async Task<List<Gym>> GetAllAsync()
        {
            const string sql = @"SELECT g.[Id]
                                    ,[Address]
                                    ,[PhoneNumber]
	                                ,c.Id
	                                ,c.Name
                              FROM [Gym] as g
                              INNER JOIN City as c on g.City = c.Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var gyms = await connection.QueryAsync<int, string, string, int, string, Gym>(
                sql,
                (id, address, phoneNumber, cityId, cityName) =>
                {
                    return new Gym
                    {
                        Id = id,
                        Address = address,
                        PhoneNumber = phoneNumber,
                        City = new City
                        {
                            Id = cityId,
                            Name = cityName
                        }
                    };
                },
                splitOn: "Address,PhoneNumber,Id,Name");

            return gyms.AsList();
        }

        public async Task<Gym> GetByIdAsync(int id)
        {
            const string sql = @"SELECT g.[Id]
                                    ,[Address]
                                    ,[PhoneNumber]
	                                ,c.Id
	                                ,c.Name
                              FROM [Gym] as g
                              INNER JOIN City as c on g.City = c.Id
                              WHERE g.Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var gyms = await connection.QueryAsync<int, string, string, int, string, Gym>(
                sql,
                (id, address, phoneNumber, cityId, cityName) =>
                {
                    return new Gym
                    {
                        Id = id,
                        Address = address,
                        PhoneNumber = phoneNumber,
                        City = new City
                        {
                            Id = cityId,
                            Name = cityName
                        }
                    };
                },
                param: new { id },
                splitOn: "Address,PhoneNumber,Id,Name");

            return gyms.FirstOrDefault();
        }

        public async Task<Gym> CreateAsync(GymDto gym)
        {
            const string insertSql = @"INSERT INTO Gym (Address, PhoneNumber, City)
                                       VALUES (@Address, @PhoneNumber, @CityId)";

            const string getSql = @"SELECT MAX(Id) FROM Gym";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(insertSql,
                new
                {
                    gym.Address,
                    gym.PhoneNumber,
                    gym.CityId
                });

            if (affectedRows != 1)
            {
                return null;
            }

            int createdId = await connection.ExecuteScalarAsync<int>(getSql);

            return await GetByIdAsync(createdId);
        }

        public async Task<Gym> UpdateAsync(GymDto gym)
        {
            const string sql = @"UPDATE Gym
                                 SET Address = @Address,
	                                 PhoneNumber = @PhoneNumber,
	                                 City = @CityId,
	                                 UpdateDateTime = @UpdateDateTime
                                 WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                sql,
                new
                {
                    gym.Address,
                    gym.PhoneNumber,
                    gym.CityId,
                    UpdateDateTime = DateTime.Now,
                    gym.Id
                });

            if (affectedRows != 1)
            {
                return null;
            }

            return await GetByIdAsync(gym.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteSql = @"DELETE FROM Gym WHERE Id = @id";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(deleteSql, new { id });

            return affectedRows == 1;
        }
    }
}
