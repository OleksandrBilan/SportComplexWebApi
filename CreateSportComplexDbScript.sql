CREATE DATABASE SportComplex
ON PRIMARY
(
	NAME = 'SportComplex_Data',
	FILENAME = 'N'D:\SSMS\MSSQL15.MSSQLSERVER\MSSQL\DATA\SportComplex.mdf',
	SIZE = 8192 KB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1024
)
LOG ON
(
	NAME = 'SportComplex_Log',
	FILENAME = 'N'D:\SSMS\MSSQL15.MSSQLSERVER\MSSQL\DATA\SportComplex.ldf',
	SIZE = 8192 KB,
	MAXSIZE = 2048 GB,
	FILEGROWTH = 10%
)
go

CREATE TABLE City
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE City
ADD CONSTRAINT PK__City__3214EC07775D8C3E PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Coach
(
	Id int IDENTITY ( 1,1 ) ,
	EmployeeId int NULL ,
	Description nvarchar(max) NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE Coach
ADD CONSTRAINT PK__Coach__3214EC0727823FE8 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE CoachSportType
(
	Id int IDENTITY ( 1,1 ) ,
	SportType int NULL ,
	Coach int NULL
)
go

ALTER TABLE CoachSportType
ADD CONSTRAINT PK__CoachSpo__3214EC072C300E4D PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Company
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE Company
ADD CONSTRAINT PK__Company__3214EC079787A5BB PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Customer
(
	Id int IDENTITY ( 1,1 ) ,
	FirstName nvarchar(max) NOT NULL ,
	LastName nvarchar(max) NOT NULL ,
	PhoneNumber nvarchar(13) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE Customer
ADD CONSTRAINT PK__Customer__3214EC07ED3C89E1 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Day
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE Day
ADD CONSTRAINT PK__Day__3214EC076DDF0FED PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE EducationLevel
(
	Id int NOT NULL ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE EducationLevel
ADD CONSTRAINT XPKEducationLevel PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE EducationSpecialty
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE EducationSpecialty
ADD CONSTRAINT PK__Educatio__3214EC07CBECB0C5 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Employee
(
	Id int IDENTITY ( 1,1 ) ,
	FirstName nvarchar(max) NOT NULL ,
	LastName nvarchar(max) NOT NULL ,
	PhoneNumber nvarchar(13) NOT NULL ,
	Position int NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL ,
	HireDate date NOT NULL ,
	Login nvarchar(max) NOT NULL ,
	Password nvarchar(max) NOT NULL ,
	DismissDate date NULL ,
	Gym int NULL
)
go

ALTER TABLE Employee
ADD CONSTRAINT PK__Employee__3214EC07846D5118 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE EmployeeEducation
(
	Id int IDENTITY ( 1,1 ) ,
	University int NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL ,
	Employee int NULL ,
	GraduationDate date NOT NULL ,
	Level int NULL ,
	Specialty int NULL
)
go

ALTER TABLE EmployeeEducation
ADD CONSTRAINT PK__Employee__3214EC070718CA16 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Group
(
	Id int IDENTITY ( 1,1 ) ,
	SportSection int NULL ,
	Coach int NULL ,
	MaxCustomersNumber int NOT NULL ,
	StartDate date NOT NULL ,
	EndDate date NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE Group
ADD CONSTRAINT PK__Group__3214EC0716B38356 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE GroupTraining
(
	Id int IDENTITY ( 1,1 ) ,
	SubscriptionReceipt int NULL ,
	Group int NULL ,
	StartDateTime datetime NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE GroupTraining
ADD CONSTRAINT PK__GroupTra__3214EC0715A1A142 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE GroupTrainingSchedule
(
	Id int IDENTITY ( 1,1 ) ,
	Group int NULL ,
	TrainingSchedule int NULL
)
go

ALTER TABLE GroupTrainingSchedule
ADD CONSTRAINT PK__GroupTra__3214EC073DF44257 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE Gym
(
	Id int IDENTITY ( 1,1 ) ,
	City int NULL ,
	Address nvarchar(max) NOT NULL ,
	PhoneNumber nvarchar(13) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE Gym
ADD CONSTRAINT PK__Gym__3214EC0742182FD5 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE IndividualCoach
(
	Id int IDENTITY ( 1,1 ) ,
	Coach int NULL ,
	PricePerHour smallmoney NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE IndividualCoach
ADD CONSTRAINT PK__Individu__3214EC0777073A60 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE IndividualTraining
(
	Id int IDENTITY ( 1,1 ) ,
	MembershipReceipt int NULL ,
	PayedHours float NOT NULL ,
	Price smallmoney NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL ,
	IndividualCoach int NULL ,
	PayementDateTime datetime NULL
)
go

ALTER TABLE IndividualTraining
ADD CONSTRAINT PK__Individu__3214EC0757BED2CD PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE MembershipReceipt
(
	Id int IDENTITY ( 1,1 ) ,
	Customer int NULL ,
	Employee int NULL ,
	MembershipType int NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL ,
	PayementDateTime datetime NULL
)
go

ALTER TABLE MembershipReceipt
ADD CONSTRAINT PK__Membersh__3214EC07800CEDED PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE MembershipType
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	AvailabilityDurationInMonths int NOT NULL ,
	Price smallmoney NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL ,
	WorkoutStartTime time NOT NULL ,
	WorkoutEndTime time NOT NULL
)
go

ALTER TABLE MembershipType
ADD CONSTRAINT PK__Membersh__3214EC07865371FA PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE MembershipTypeSportType
(
	Id int IDENTITY ( 1,1 ) ,
	MembershipType int NULL ,
	SportType int NULL
)
go

ALTER TABLE MembershipTypeSportType
ADD CONSTRAINT PK__Membersh__3214EC0757088121 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE PositionType
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE PositionType
ADD CONSTRAINT PK__Position__3214EC07249D8BF6 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE PreviousJob
(
	Id int IDENTITY ( 1,1 ) ,
	Employee int NULL ,
	Company int NULL ,
	StartDate date NOT NULL ,
	EndDate date NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE PreviousJob
ADD CONSTRAINT PK__Previous__3214EC079C9EE573 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE SportSection
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	SportType int NULL ,
	Description nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE SportSection
ADD CONSTRAINT PK__SportSec__3214EC07FDB374D2 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE SportType
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE SportType
ADD CONSTRAINT PK__SportTyp__3214EC0725FF9260 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE SubscriptionReceipt
(
	Id int IDENTITY ( 1,1 ) ,
	Employee int NULL ,
	Customer int NULL ,
	SubscriptionType int NULL ,
	ExpireDate date NOT NULL ,
	IsPayed bit NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL ,
	IsActive bit NOT NULL
)
go

ALTER TABLE SubscriptionReceipt
ADD CONSTRAINT PK__Subscrip__3214EC07B9BF8A28 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE SubscriptionType
(
	Id int IDENTITY ( 1,1 ) ,
	SportSection int NULL ,
	AvailableTrainingsCount int NULL ,
	Price smallmoney NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE SubscriptionType
ADD CONSTRAINT PK__Subscrip__3214EC07D1C4A99A PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE TrainingSchedule
(
	Id int IDENTITY ( 1,1 ) ,
	Day int NULL ,
	StartTime time NOT NULL ,
	EndTime time NOT NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE TrainingSchedule
ADD CONSTRAINT PK__Training__3214EC07C2416A00 PRIMARY KEY CLUSTERED (Id ASC)
go

CREATE TABLE University
(
	Id int IDENTITY ( 1,1 ) ,
	Name nvarchar(max) NULL ,
	CreateDateTime datetime NOT NULL ,
	UpdateDateTime datetime NULL
)
go

ALTER TABLE University
ADD CONSTRAINT PK__Universi__3214EC0739B90971 PRIMARY KEY CLUSTERED (Id ASC)
go

ALTER TABLE Coach WITH CHECK
ADD CONSTRAINT FK__Coach__EmployeeI__5629CD9C FOREIGN KEY (EmployeeId) REFERENCES
Employee(Id)
ON DELETE CASCADE
go

ALTER TABLE CoachSportType WITH CHECK
ADD CONSTRAINT FK__CoachSpor__Coach__5812160E FOREIGN KEY (Coach) REFERENCES
Coach(Id)
ON DELETE CASCADE
go

ALTER TABLE CoachSportType WITH CHECK
ADD CONSTRAINT FK__CoachSpor__Sport__571DF1D5 FOREIGN KEY (SportType) REFERENCES
SportType(Id)
ON DELETE CASCADE
go

ALTER TABLE Employee WITH CHECK
ADD CONSTRAINT FK__Employee__Gym__59FA5E80 FOREIGN KEY (Gym) REFERENCES Gym(Id)
go

ALTER TABLE Employee WITH CHECK
ADD CONSTRAINT FK__Employee__Positi__59063A47 FOREIGN KEY (Position) REFERENCES
PositionType(Id)
go

ALTER TABLE EmployeeEducation WITH CHECK
ADD CONSTRAINT FK__EmployeeE__Emplo__5BE2A6F2 FOREIGN KEY (Employee) REFERENCES
Employee(Id)
go

ALTER TABLE EmployeeEducation WITH CHECK
ADD CONSTRAINT FK__EmployeeE__Level__5CD6CB2B FOREIGN KEY (Level) REFERENCES
EducationLevel(Id)
go

ALTER TABLE EmployeeEducation WITH CHECK
ADD CONSTRAINT FK__EmployeeE__Speci__5DCAEF64 FOREIGN KEY (Specialty) REFERENCES
EducationSpecialty(Id)
go

ALTER TABLE EmployeeEducation WITH CHECK
ADD CONSTRAINT FK__EmployeeE__Unive__5AEE82B9 FOREIGN KEY (University) REFERENCES
University(Id)
go

ALTER TABLE Group WITH CHECK
ADD CONSTRAINT FK__Group__Coach__5FB337D6 FOREIGN KEY (Coach) REFERENCES Coach(Id)
go

ALTER TABLE Group WITH CHECK
ADD CONSTRAINT FK__Group__SportSect__5EBF139D FOREIGN KEY (SportSection) REFERENCES
SportSection(Id)
go

ALTER TABLE GroupTraining WITH CHECK
ADD CONSTRAINT FK__GroupTrai__Group__619B8048 FOREIGN KEY (Group) REFERENCES
Group(Id)
go

ALTER TABLE GroupTraining WITH CHECK
ADD CONSTRAINT FK__GroupTrai__Subsc__60A75C0F FOREIGN KEY (SubscriptionReceipt)
REFERENCES SubscriptionReceipt(Id)
go

ALTER TABLE GroupTrainingSchedule WITH CHECK
ADD CONSTRAINT FK__GroupTrai__Group__6383C8BA FOREIGN KEY (Group) REFERENCES
Group(Id)
ON DELETE CASCADE
go

ALTER TABLE GroupTrainingSchedule WITH CHECK
ADD CONSTRAINT FK__GroupTrai__Train__628FA481 FOREIGN KEY (TrainingSchedule)
REFERENCES TrainingSchedule(Id)
ON DELETE CASCADE
go

ALTER TABLE Gym WITH CHECK
ADD CONSTRAINT FK__Gym__City__6477ECF3 FOREIGN KEY (City) REFERENCES City(Id)
go

ALTER TABLE IndividualCoach WITH CHECK
ADD CONSTRAINT FK__Individua__Coach__656C112C FOREIGN KEY (Coach) REFERENCES
Coach(Id)
ON DELETE CASCADE
go

ALTER TABLE IndividualTraining WITH CHECK
ADD CONSTRAINT FK__Individua__Indiv__6754599E FOREIGN KEY (IndividualCoach)
REFERENCES IndividualCoach(Id)
go

ALTER TABLE IndividualTraining WITH CHECK
ADD CONSTRAINT FK__Individua__Membe__66603565 FOREIGN KEY (MembershipReceipt)
REFERENCES MembershipReceipt(Id)
go

ALTER TABLE MembershipReceipt WITH CHECK
ADD CONSTRAINT FK__Membershi__Custo__68487DD7 FOREIGN KEY (Customer) REFERENCES
Customer(Id)
go

ALTER TABLE MembershipReceipt WITH CHECK
ADD CONSTRAINT FK__Membershi__Emplo__693CA210 FOREIGN KEY (Employee) REFERENCES
Employee(Id)
go

ALTER TABLE MembershipReceipt WITH CHECK
ADD CONSTRAINT FK__Membershi__Membe__1AD3FDA4 FOREIGN KEY (MembershipType)
REFERENCES MembershipType(Id)
go

ALTER TABLE MembershipTypeSportType WITH CHECK
ADD CONSTRAINT FK__Membershi__Membe__151B244E FOREIGN KEY (MembershipType)
REFERENCES MembershipType(Id)
go

ALTER TABLE MembershipTypeSportType WITH CHECK
ADD CONSTRAINT FK__Membershi__Sport__6B24EA82 FOREIGN KEY (SportType) REFERENCES
SportType(Id)
go

ALTER TABLE PreviousJob WITH CHECK
ADD CONSTRAINT FK__PreviousJ__Compa__6E01572D FOREIGN KEY (Company) REFERENCES
Company(Id)
go

ALTER TABLE PreviousJob WITH CHECK
ADD CONSTRAINT FK__PreviousJ__Emplo__6D0D32F4 FOREIGN KEY (Employee) REFERENCES
Employee(Id)
go

ALTER TABLE SportSection WITH CHECK
ADD CONSTRAINT FK__SportSect__Sport__6EF57B66 FOREIGN KEY (SportType) REFERENCES
SportType(Id)
go

ALTER TABLE SubscriptionReceipt WITH CHECK
ADD CONSTRAINT FK__Subscript__Custo__70DDC3D8 FOREIGN KEY (Customer) REFERENCES
Customer(Id)
go

ALTER TABLE SubscriptionReceipt WITH CHECK
ADD CONSTRAINT FK__Subscript__Emplo__6FE99F9F FOREIGN KEY (Employee) REFERENCES
Employee(Id)
ON DELETE SET NULL
go

ALTER TABLE SubscriptionReceipt WITH CHECK
ADD CONSTRAINT FK__Subscript__Subsc__71D1E811 FOREIGN KEY (SubscriptionType)
REFERENCES SubscriptionType(Id)
go

ALTER TABLE SubscriptionType WITH CHECK
ADD CONSTRAINT FK__Subscript__Sport__72C60C4A FOREIGN KEY (SportSection) REFERENCES
SportSection(Id)
go

ALTER TABLE TrainingSchedule WITH CHECK
ADD CONSTRAINT FK__TrainingSch__Day__73BA3083 FOREIGN KEY (Day) REFERENCES Day(Id)
go

CREATE PROCEDURE UpsertEmployeeEducation @Id int = null , @EmployeeId int , @UniversityName nvarchar(max) , @SpecialtyName nvarchar(max) , 
										 @GraduationDate date , @LevelId int
AS 
	if not exists (SELECT Id FROM University WHERE Name LIKE @UniversityName)
	begin
		INSERT INTO University (Name, CreateDateTime)
		VALUES (@UniversityName, GETDATE());
	end

	if not exists (SELECT Id FROM EducationSpecialty WHERE Name LIKE @SpecialtyName)
	begin
		INSERT INTO EducationSpecialty (Name, CreateDateTime)
		VALUES (@SpecialtyName, GETDATE());
	end

	declare @universityId int = (SELECT Id FROM University WHERE Name LIKE @UniversityName),
			@specialtyId int = (SELECT Id FROM EducationSpecialty WHERE Name LIKE @SpecialtyName);
	
	if @Id is null or @Id = 0
	begin
		INSERT INTO EmployeeEducation (Employee, University, Specialty, GraduationDate, Level, CreateDateTime)
		VALUES (@employeeId, @universityId, @specialtyId, @graduationDate, @levelId, GETDATE());
	end
	else
	begin
		UPDATE EmployeeEducation
		SET GraduationDate = @graduationDate,
		Level = @levelId,
		University = (SELECT Id FROM University WHERE Name LIKE @universityName),
		Specialty = (SELECT Id FROM EducationSpecialty WHERE Name LIKE @specialtyName),
		UpdateDateTime = GETDATE()
		WHERE Id = @id;
	end
go

CREATE PROCEDURE UpsertEmployeePreviousJob @Id int = null , @EmployeeId int , @CompanyName nvarchar(max) , @StartDate date , @EndDate date
AS
	if not exists (SELECT Id FROM Company WHERE Name like @CompanyName)
	begin
		INSERT INTO Company (Name, CreateDateTime)
		VALUES (@CompanyName, GETDATE());
	end

	declare @companyId int = (SELECT Id FROM Company WHERE Name like @CompanyName);
	if @Id is null or @Id = 0
	begin
		INSERT INTO PreviousJob (Employee, Company, StartDate, EndDate, CreateDateTime)
		VALUES (@EmployeeId, @companyId, @StartDate, @EndDate, GETDATE());
	end
	else
	begin
		UPDATE PreviousJob
		SET Employee = @EmployeeId,
		Company = @companyId,
		StartDate = @StartDate,
		EndDate = @EndDate,
		UpdateDateTime = GETDATE()
		WHERE Id = @Id;
	end
go