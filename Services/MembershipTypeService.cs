using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ApiModels.Membership;
using WebApi.Models;
using WebApi.Models.Membership;

namespace WebApi.Services
{
    public class MembershipTypeService
    {
        private readonly string ConnectionString;
        private readonly SportTypeService _sportTypeService;

        public MembershipTypeService(IConfiguration configuration, SportTypeService sportTypeService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _sportTypeService = sportTypeService;
        }

        public async Task<List<MembershipType>> GetAllAsync()
        {
            const string sql = @"SELECT mt.[Id]
                                       ,[Name]
                                       ,[AvailabilityDurationInMonths]
                                       ,[Price]
                                       ,[WorkoutStartTime]
                                       ,[WorkoutEndTime]
	                                   ,mtst.SportType
                                 FROM [MembershipType] as mt
                                 INNER JOIN MembershipTypeSportType as mtst ON mtst.MembershipType = mt.Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sportTypes = await _sportTypeService.GetAllAsync();

            var membershipTypes = await connection.QueryAsync<int, string, int, decimal, TimeSpan, TimeSpan, int, MembershipType>(
                sql,
                (id, name, availability, price, workoutStartTime, workoutEndTime, sportTypeId) =>
                {
                    return new MembershipType
                    {
                        Id = id,
                        Name = name,
                        AvailabilityDurationInMonths = availability,
                        Price = price,
                        WorkoutStartTime = workoutStartTime.ToString(),
                        WorkoutEndTime = workoutEndTime.ToString(),
                        SportTypes = new List<SportType> { sportTypes.FirstOrDefault(st => st.Id == sportTypeId) }
                    };
                },
                splitOn: "Id,Name,AvailabilityDurationInMonths,Price,WorkoutStartTime,WorkoutEndTime,SportType");

            var result = membershipTypes.GroupBy(mt => mt.Id).Select(mt =>
            {
                var groupedType = mt.First();
                groupedType.SportTypes = mt.Select(mt => mt.SportTypes.FirstOrDefault()).ToList();
                return groupedType;
            });

            return result.AsList();
        }

        public async Task<MembershipType> GetByIdAsync(int id)
        {
            const string sql = @"SELECT mt.[Id]
                                       ,[Name]
                                       ,[AvailabilityDurationInMonths]
                                       ,[Price]
                                       ,[WorkoutStartTime]
                                       ,[WorkoutEndTime]
	                                   ,mtst.SportType
                                 FROM [MembershipType] as mt
                                 INNER JOIN MembershipTypeSportType as mtst ON mtst.MembershipType = mt.Id
                                 WHERE mt.Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sportTypes = await _sportTypeService.GetAllAsync();

            var membershipTypes = await connection.QueryAsync<int, string, int, decimal, TimeSpan, TimeSpan, int, MembershipType>(
                sql,
                (id, name, availability, price, workoutStartTime, workoutEndTime, sportTypeId) =>
                {
                    return new MembershipType
                    {
                        Id = id,
                        Name = name,
                        AvailabilityDurationInMonths = availability,
                        Price = price,
                        WorkoutStartTime = workoutStartTime.ToString(),
                        WorkoutEndTime = workoutEndTime.ToString(),
                        SportTypes = new List<SportType> { sportTypes.FirstOrDefault(st => st.Id == sportTypeId) }
                    };
                },
                param: new { id },
                splitOn: "Id,Name,AvailabilityDurationInMonths,Price,WorkoutStartTime,WorkoutEndTime,SportType");

            var result = membershipTypes.GroupBy(mt => mt.Id).Select(mt =>
            {
                var groupedType = mt.First();
                groupedType.SportTypes = mt.Select(mt => mt.SportTypes.FirstOrDefault()).ToList();
                return groupedType;
            });

            return result.FirstOrDefault();
        }

        public async Task<MembershipType> CreateAsync(MembershipTypeDto membershipType)
        {
            const string insertSql = @"INSERT INTO MembershipType (Name, AvailabilityDurationInMonths, Price, WorkoutStartTime, WorkoutEndTime, CreateDateTime)
                                       VALUES (@Name, @AvailabilityDurationInMonths, @Price, @WorkoutStartTime, @WorkoutEndTime, @CreateDateTime);";

            const string getIdSql = @"SELECT MAX(Id) FROM MembershipType";

            const string insertMemTypeSportTypeSql = @"INSERT INTO MembershipTypeSportType (MembershipType, SportType)
                                                       VALUES (@Id, @sportTypeId)";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int createdId;
            var transaction = connection.BeginTransaction("InsertMembershipType");
            try
            {
               await connection.ExecuteAsync(
                                insertSql,
                                new
                                {
                                    membershipType.Name,
                                    membershipType.AvailabilityDurationInMonths,
                                    membershipType.Price,
                                    membershipType.WorkoutStartTime,
                                    membershipType.WorkoutEndTime,
                                    CreateDateTime = DateTime.Now
                                },
                                transaction);

                createdId = await connection.ExecuteScalarAsync<int>(getIdSql, transaction: transaction);

                foreach (int sportTypeId in membershipType.SportTypeIds)
                {
                    await connection.ExecuteAsync(insertMemTypeSportTypeSql, new { Id = createdId, sportTypeId }, transaction);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                transaction.Rollback();
                return null;
            }

            return await GetByIdAsync(createdId);
        }

        public async Task<MembershipType> UpdateAsync(MembershipTypeDto membershipType)
        {
            const string updateSql = @"UPDATE MembershipType
                                       SET Name = @Name,
	                                       AvailabilityDurationInMonths = @AvailabilityDurationInMonths,
	                                       Price = @Price,
	                                       WorkoutStartTime = @WorkoutStartTime,
	                                       WorkoutEndTime = @WorkoutEndTime,
	                                       UpdateDateTime = @UpdateDateTime
                                       WHERE Id = @Id";

            const string deleteRelationsSql = @"DELETE FROM MembershipTypeSportType WHERE MembershipType = @Id";

            const string insertMemTypeSportTypeSql = @"INSERT INTO MembershipTypeSportType (MembershipType, SportType)
                                                       VALUES (@Id, @sportTypeId)";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var transaction = connection.BeginTransaction("UpdateMembershipType");
            try
            {
                await connection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        membershipType.Name,
                        membershipType.AvailabilityDurationInMonths,
                        membershipType.Price,
                        membershipType.WorkoutStartTime,
                        membershipType.WorkoutEndTime,
                        UpdateDateTime = DateTime.Now,
                        membershipType.Id
                    },
                    transaction);

                await connection.ExecuteAsync(deleteRelationsSql, new { membershipType.Id }, transaction);

                foreach (int sportTypeId in membershipType.SportTypeIds)
                {
                    await connection.ExecuteAsync(insertMemTypeSportTypeSql, new { membershipType.Id, sportTypeId }, transaction);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                transaction.Rollback();
                return null;
            }

            return await GetByIdAsync(membershipType.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteSql = @"DELETE FROM MembershipType WHERE Id = @id";

            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(deleteSql, new { id });

            return affectedRows == 1;
        }
    }
}
