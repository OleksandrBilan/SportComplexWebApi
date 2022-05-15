﻿using Dapper;
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
    public class GroupService
    {
        private readonly string ConnectionString;
        private readonly CoachService _coachService;
        private readonly SportSectionService _sportSectionService;

        public GroupService(IConfiguration configuration, CoachService coachService, SportSectionService sportSectionService)
        {
            ConnectionString = configuration.GetConnectionString("SportComplex");
            _coachService = coachService;
            _sportSectionService = sportSectionService;
        }

        public async Task<List<Group>> GetAllAsync()
        {
            const string sql = @"SELECT [Id]
                                       ,[SportSection]
                                       ,[Coach]
                                       ,[MaxCustomersNumber]
                                       ,[StartDate]
                                       ,[EndDate]
                                 FROM [Group]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var groups = await connection.QueryAsync<int, int, int, int, DateTime, DateTime, Group>(
                sql,
                (id, sportSectionId, coachId, maxCustomersNumber, startDate, endDate) =>
                {
                    return new Group
                    {
                        Id = id,
                        SportSection = _sportSectionService.GetByIdAsync(sportSectionId).Result,
                        Coach = _coachService.GetByIdAsync(coachId).Result,
                        MaxCustomersCount = maxCustomersNumber,
                        StartDate = startDate,
                        EndDate = endDate
                    };
                },
                splitOn: "SportSection,Coach,MaxCustomersNumber,StartDate,EndDate");

            return groups.AsList();
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            const string sql = @"SELECT [Id]
                                       ,[SportSection]
                                       ,[Coach]
                                       ,[MaxCustomersNumber]
                                       ,[StartDate]
                                       ,[EndDate]
                                 FROM [Group]
                                 WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var groups = await connection.QueryAsync<int, int, int, int, DateTime, DateTime, Group>(
                sql,
                (id, sportSectionId, coachId, maxCustomersNumber, startDate, endDate) =>
                {
                    return new Group
                    {
                        Id = id,
                        SportSection = _sportSectionService.GetByIdAsync(sportSectionId).Result,
                        Coach = _coachService.GetByIdAsync(coachId).Result,
                        MaxCustomersCount = maxCustomersNumber,
                        StartDate = startDate,
                        EndDate = endDate
                    };
                },
                param: new { id },
                splitOn: "SportSection,Coach,MaxCustomersNumber,StartDate,EndDate");

            return groups.FirstOrDefault();
        }

        public async Task<Group> CreateAsync(GroupDto group)
        {
            const string insertSql = @"INSERT INTO [Group] (SportSection, Coach, MaxCustomersNumber, StartDate, EndDate, CreateDateTime)
                                       VALUES (@SportSectionId, @CoachId, @MaxCustomersNumber, @StartDate, @EndDate, @CreateDateTime)";

            const string getIdSql = @"SELECT MAX(Id) FROM [Group]";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                insertSql,
                new
                {
                    group.SportSectionId,
                    group.CoachId,
                    group.MaxCustomersNumber,
                    group.StartDate,
                    group.EndDate,
                    CreateDateTime = DateTime.Now
                });

            if (affectedRows != 1)
            {
                return null;
            }

            int createdId = await connection.ExecuteScalarAsync<int>(getIdSql);

            return await GetByIdAsync(createdId);
        }

        public async Task<Group> UpdateAsync(GroupDto group)
        {
            const string updateSql = @"UPDATE [Group] 
                                       SET SportSection = @SportSectionId,
                                       	   Coach = @CoachId,
                                       	   MaxCustomersNumber = @MaxCustomersNumber,
                                       	   StartDate = @StartDate,
                                       	   EndDate = @EndDate
                                       WHERE Id = @Id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                updateSql,
                new
                {
                    group.SportSectionId,
                    group.CoachId,
                    group.MaxCustomersNumber,
                    group.StartDate,
                    group.EndDate,
                    UpdateDateTime = DateTime.Now,
                    group.Id
                });

            return await GetByIdAsync(group.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteSql = @"DELETE FROM [Group] WHERE Id = @id";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(deleteSql, new { id });

            return affectedRows == 1;
        }
    }
}