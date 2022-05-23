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
using WebApi.Models.GroupTrainingSubscription;

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

            const string getScheduleSql = @"SELECT ts.[Id]
                                                  ,cast([StartTime] as varchar) as StartTime
                                                  ,cast([EndTime] as varchar) as EndTime
	                                              ,d.Id
	                                              ,d.Name
                                              FROM [TrainingSchedule] as ts
                                            INNER JOIN Day as d ON ts.Day = d.Id
                                            INNER JOIN GroupTrainingSchedule as gts ON gts.TrainingSchedule = ts.Id
									                                               AND gts.[Group] = @Id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var groups = await connection.QueryAsync<int, int, int, int, DateTime, DateTime, Group>(
                sql,
                (id, sportSectionId, coachId, maxCustomersNumber, startDate, endDate) =>
                {
                    return new Group
                    {
                        Id = id,
                        SportSection = new SportSection { Id = sportSectionId },
                        Coach = new Coach { Id = coachId },
                        MaxCustomersCount = maxCustomersNumber,
                        StartDate = startDate,
                        EndDate = endDate
                    };
                },
                splitOn: "SportSection,Coach,MaxCustomersNumber,StartDate,EndDate");

            foreach (var group in groups)
            {
                group.SportSection = await _sportSectionService.GetByIdAsync(group.SportSection.Id);
                group.Coach = await _coachService.GetByIdAsync(group.Coach.Id);
                group.Schedules = (await connection.QueryAsync<TrainingSchedule, Day, TrainingSchedule>(
                    getScheduleSql,
                    (schedule, day) =>
                    {
                        schedule.Day = day;
                        return schedule;
                    },
                    param: new { group.Id },
                    splitOn: "Id")).AsList();
            }

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

            const string getScheduleSql = @"SELECT ts.[Id]
                                                  ,cast([StartTime] as varchar) as StartTime
                                                  ,cast([EndTime] as varchar) as EndTime
	                                              ,d.Id
	                                              ,d.Name
                                              FROM [TrainingSchedule] as ts
                                            INNER JOIN Day as d ON ts.Day = d.Id
                                            INNER JOIN GroupTrainingSchedule as gts ON gts.TrainingSchedule = ts.Id
									                                               AND gts.[Group] = @Id;";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var groups = await connection.QueryAsync<int, int, int, int, DateTime, DateTime, Group>(
                sql,
                (id, sportSectionId, coachId, maxCustomersNumber, startDate, endDate) =>
                {
                    return new Group
                    {
                        Id = id,
                        SportSection = new SportSection { Id = sportSectionId },
                        Coach = new Coach { Id = coachId },
                        MaxCustomersCount = maxCustomersNumber,
                        StartDate = startDate,
                        EndDate = endDate
                    };
                },
                param: new { id },
                splitOn: "SportSection,Coach,MaxCustomersNumber,StartDate,EndDate");

            foreach (var group in groups)
            {
                group.SportSection = await _sportSectionService.GetByIdAsync(group.SportSection.Id);
                group.Coach = await _coachService.GetByIdAsync(group.Coach.Id);
                group.Schedules = (await connection.QueryAsync<TrainingSchedule, Day, TrainingSchedule>(
                    getScheduleSql,
                    (schedule, day) =>
                    {
                        schedule.Day = day;
                        return schedule;
                    },
                    param: new { id },
                    splitOn: "Id")).AsList();
            }

            return groups.FirstOrDefault();
        }

        public async Task<Group> CreateAsync(GroupDto group)
        {
            const string insertGroupSql = @"INSERT INTO [Group] (SportSection, Coach, MaxCustomersNumber, StartDate, EndDate, CreateDateTime)
                                            VALUES (@SportSectionId, @CoachId, @MaxCustomersNumber, @StartDate, @EndDate, @CreateDateTime)";

            const string insertTrainingScheduleSql = @"INSERT INTO TrainingSchedule (Day, StartTime, EndTime, CreateDateTime)
                                                       VALUES (@DayId, @StartTime, @EndTime, @CreateDateTime)";

            const string insertDependencySql = @"INSERT INTO GroupTrainingSchedule ([Group], TrainingSchedule)
                                                 VALUES (@GroupId, @ScheduleId)";

            const string findSameScheduleSql = @"SELECT Id FROM TrainingSchedule
                                                 WHERE Day = @DayId AND StartTime = @StartTime AND EndTime = @EndTime";

            const string getGroupIdSql = @"SELECT MAX(Id) FROM [Group]";

            const string getSheduleIdSql = @"SELECT MAX(Id) FROM TrainingSchedule";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(
                insertGroupSql,
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

            int createdGroupId = await connection.ExecuteScalarAsync<int>(getGroupIdSql);

            foreach (var schedule in group.Schedules)
            {
                int scheduleId = await connection.ExecuteScalarAsync<int>(findSameScheduleSql, new { schedule.DayId, schedule.StartTime, schedule.EndTime });

                if (scheduleId == 0)
                {
                    await connection.ExecuteAsync(
                    insertTrainingScheduleSql,
                    new
                    {
                        schedule.DayId,
                        schedule.StartTime,
                        schedule.EndTime,
                        CreateDateTime = DateTime.Now
                    });

                    scheduleId = await connection.ExecuteScalarAsync<int>(getSheduleIdSql);
                }

                await connection.ExecuteAsync(insertDependencySql, new { GroupId = createdGroupId, ScheduleId = scheduleId });
            }

            return await GetByIdAsync(createdGroupId);
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

            const string insertTrainingScheduleSql = @"INSERT INTO TrainingSchedule (Day, StartTime, EndTime, CreateDateTime)
                                                       VALUES (@DayId, @StartTime, @EndTime, GETDATE())";

            const string insertDependencySql = @"INSERT INTO GroupTrainingSchedule ([Group], TrainingSchedule)
                                                 VALUES (@GroupId, @ScheduleId)";

            const string findSameScheduleSql = @"SELECT Id FROM TrainingSchedule
                                                 WHERE Day = @DayId AND StartTime = @StartTime AND EndTime = @EndTime";

            const string deleteDependenciesSql = @"DELETE FROM GroupTrainingSchedule
                                                   WHERE [Group] = @GroupId";

            const string getSheduleIdSql = @"SELECT MAX(Id) FROM TrainingSchedule";


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

            await connection.ExecuteAsync(deleteDependenciesSql, new { GroupId = group.Id });

            foreach (var schedule in group.Schedules)
            {
                int scheduleId = await connection.ExecuteScalarAsync<int>(findSameScheduleSql, new { schedule.DayId, schedule.StartTime, schedule.EndTime });

                if (scheduleId == 0)
                {
                    await connection.ExecuteAsync(
                    insertTrainingScheduleSql,
                    new
                    {
                        DayId = schedule.DayId,
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        CreateDateTime = DateTime.Now
                    });

                    scheduleId = await connection.ExecuteScalarAsync<int>(getSheduleIdSql);
                }

                await connection.ExecuteAsync(insertDependencySql, new { GroupId = group.Id, ScheduleId = scheduleId });
            }

            return await GetByIdAsync(group.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteSql = @"DELETE FROM [Group] WHERE Id = @id";

            const string deleteDependenciesSql = @"DELETE FROM GroupTrainingSchedule
                                                   WHERE [Group] = @GroupId";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            int affectedRows = await connection.ExecuteAsync(deleteSql, new { id });

            await connection.ExecuteAsync(deleteDependenciesSql, new { GroupId = id });

            return affectedRows == 1;
        }

        public async Task<List<Day>> GetDaysAsync()
        {
            const string sql = @"SELECT Id, Name FROM Day";

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var days = await connection.QueryAsync<Day>(sql);

            return days.AsList();
        }
    }
}
