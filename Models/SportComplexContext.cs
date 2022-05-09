using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Models
{
    public partial class SportComplexContext : DbContext
    {
        public SportComplexContext()
        {
        }

        public SportComplexContext(DbContextOptions<SportComplexContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<CoachSportType> CoachSportTypes { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Day> Days { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<EducationSpecialty> EducationSpecialties { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupTraining> GroupTrainings { get; set; }
        public virtual DbSet<GroupTrainingSchedule> GroupTrainingSchedules { get; set; }
        public virtual DbSet<Gym> Gyms { get; set; }
        public virtual DbSet<IndividualCoach> IndividualCoaches { get; set; }
        public virtual DbSet<IndividualTraining> IndividualTrainings { get; set; }
        public virtual DbSet<MembershipReceipt> MembershipReceipts { get; set; }
        public virtual DbSet<MembershipType> MembershipTypes { get; set; }
        public virtual DbSet<MembershipTypeSportType> MembershipTypeSportTypes { get; set; }
        public virtual DbSet<PositionType> PositionTypes { get; set; }
        public virtual DbSet<PreviousJob> PreviousJobs { get; set; }
        public virtual DbSet<SportSection> SportSections { get; set; }
        public virtual DbSet<SportType> SportTypes { get; set; }
        public virtual DbSet<SubscriptionReceipt> SubscriptionReceipts { get; set; }
        public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public virtual DbSet<TrainingSchedule> TrainingSchedules { get; set; }
        public virtual DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_CI_AS");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Coach>(entity =>
            {
                entity.ToTable("Coach");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Coaches)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Coach__EmployeeI__5629CD9C");
            });

            modelBuilder.Entity<CoachSportType>(entity =>
            {
                entity.ToTable("CoachSportType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.CoachNavigation)
                    .WithMany(p => p.CoachSportTypes)
                    .HasForeignKey(d => d.Coach)
                    .HasConstraintName("FK__CoachSpor__Coach__5812160E");

                entity.HasOne(d => d.SportTypeNavigation)
                    .WithMany(p => p.CoachSportTypes)
                    .HasForeignKey(d => d.SportType)
                    .HasConstraintName("FK__CoachSpor__Sport__571DF1D5");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.ToTable("Day");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<EducationLevel>(entity =>
            {
                entity.ToTable("EducationLevel");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<EducationSpecialty>(entity =>
            {
                entity.ToTable("EducationSpecialty");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.DismissDate).HasColumnType("date");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Login).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.GymNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Gym)
                    .HasConstraintName("FK__Employee__Gym__59FA5E80");

                entity.HasOne(d => d.PositionNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Position)
                    .HasConstraintName("FK__Employee__Positi__59063A47");
            });

            modelBuilder.Entity<EmployeeEducation>(entity =>
            {
                entity.ToTable("EmployeeEducation");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.GraduationDate).HasColumnType("date");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.EmployeeEducations)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK__EmployeeE__Emplo__5BE2A6F2");

                entity.HasOne(d => d.LevelNavigation)
                    .WithMany(p => p.EmployeeEducations)
                    .HasForeignKey(d => d.Level)
                    .HasConstraintName("FK__EmployeeE__Level__5CD6CB2B");

                entity.HasOne(d => d.SpecialtyNavigation)
                    .WithMany(p => p.EmployeeEducations)
                    .HasForeignKey(d => d.Specialty)
                    .HasConstraintName("FK__EmployeeE__Speci__5DCAEF64");

                entity.HasOne(d => d.UniversityNavigation)
                    .WithMany(p => p.EmployeeEducations)
                    .HasForeignKey(d => d.University)
                    .HasConstraintName("FK__EmployeeE__Unive__5AEE82B9");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CoachNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.Coach)
                    .HasConstraintName("FK__Group__Coach__5FB337D6");

                entity.HasOne(d => d.SportSectionNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.SportSection)
                    .HasConstraintName("FK__Group__SportSect__5EBF139D");
            });

            modelBuilder.Entity<GroupTraining>(entity =>
            {
                entity.ToTable("GroupTraining");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.GroupNavigation)
                    .WithMany(p => p.GroupTrainings)
                    .HasForeignKey(d => d.Group)
                    .HasConstraintName("FK__GroupTrai__Group__619B8048");

                entity.HasOne(d => d.SubscriptionReceiptNavigation)
                    .WithMany(p => p.GroupTrainings)
                    .HasForeignKey(d => d.SubscriptionReceipt)
                    .HasConstraintName("FK__GroupTrai__Subsc__60A75C0F");
            });

            modelBuilder.Entity<GroupTrainingSchedule>(entity =>
            {
                entity.ToTable("GroupTrainingSchedule");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.GroupNavigation)
                    .WithMany(p => p.GroupTrainingSchedules)
                    .HasForeignKey(d => d.Group)
                    .HasConstraintName("FK__GroupTrai__Group__6383C8BA");

                entity.HasOne(d => d.TrainingScheduleNavigation)
                    .WithMany(p => p.GroupTrainingSchedules)
                    .HasForeignKey(d => d.TrainingSchedule)
                    .HasConstraintName("FK__GroupTrai__Train__628FA481");
            });

            modelBuilder.Entity<Gym>(entity =>
            {
                entity.ToTable("Gym");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Gyms)
                    .HasForeignKey(d => d.City)
                    .HasConstraintName("FK__Gym__City__6477ECF3");
            });

            modelBuilder.Entity<IndividualCoach>(entity =>
            {
                entity.ToTable("IndividualCoach");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.PricePerHour).HasColumnType("smallmoney");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CoachNavigation)
                    .WithMany(p => p.IndividualCoaches)
                    .HasForeignKey(d => d.Coach)
                    .HasConstraintName("FK__Individua__Coach__656C112C");
            });

            modelBuilder.Entity<IndividualTraining>(entity =>
            {
                entity.ToTable("IndividualTraining");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.PayementDateTime).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("smallmoney");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.IndividualCoachNavigation)
                    .WithMany(p => p.IndividualTrainings)
                    .HasForeignKey(d => d.IndividualCoach)
                    .HasConstraintName("FK__Individua__Indiv__6754599E");

                entity.HasOne(d => d.MembershipReceiptNavigation)
                    .WithMany(p => p.IndividualTrainings)
                    .HasForeignKey(d => d.MembershipReceipt)
                    .HasConstraintName("FK__Individua__Membe__66603565");
            });

            modelBuilder.Entity<MembershipReceipt>(entity =>
            {
                entity.ToTable("MembershipReceipt");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.PayementDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.MembershipReceipts)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK__Membershi__Custo__68487DD7");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.MembershipReceipts)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK__Membershi__Emplo__693CA210");

                entity.HasOne(d => d.MembershipTypeNavigation)
                    .WithMany(p => p.MembershipReceipts)
                    .HasForeignKey(d => d.MembershipType)
                    .HasConstraintName("FK__Membershi__Membe__1AD3FDA4");
            });

            modelBuilder.Entity<MembershipType>(entity =>
            {
                entity.ToTable("MembershipType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Price).HasColumnType("smallmoney");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.Property(e => e.WorkoutEndTime).HasColumnType("datetime");

                entity.Property(e => e.WorkoutStartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MembershipTypeSportType>(entity =>
            {
                entity.ToTable("MembershipTypeSportType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.MembershipTypeNavigation)
                    .WithMany(p => p.MembershipTypeSportTypes)
                    .HasForeignKey(d => d.MembershipType)
                    .HasConstraintName("FK__Membershi__Membe__151B244E");

                entity.HasOne(d => d.SportTypeNavigation)
                    .WithMany(p => p.MembershipTypeSportTypes)
                    .HasForeignKey(d => d.SportType)
                    .HasConstraintName("FK__Membershi__Sport__6B24EA82");
            });

            modelBuilder.Entity<PositionType>(entity =>
            {
                entity.ToTable("PositionType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PreviousJob>(entity =>
            {
                entity.ToTable("PreviousJob");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CompanyNavigation)
                    .WithMany(p => p.PreviousJobs)
                    .HasForeignKey(d => d.Company)
                    .HasConstraintName("FK__PreviousJ__Compa__6E01572D");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.PreviousJobs)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK__PreviousJ__Emplo__6D0D32F4");
            });

            modelBuilder.Entity<SportSection>(entity =>
            {
                entity.ToTable("SportSection");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.SportTypeNavigation)
                    .WithMany(p => p.SportSections)
                    .HasForeignKey(d => d.SportType)
                    .HasConstraintName("FK__SportSect__Sport__6EF57B66");
            });

            modelBuilder.Entity<SportType>(entity =>
            {
                entity.ToTable("SportType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SubscriptionReceipt>(entity =>
            {
                entity.ToTable("SubscriptionReceipt");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.SubscriptionReceipts)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK__Subscript__Custo__70DDC3D8");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.SubscriptionReceipts)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK__Subscript__Emplo__6FE99F9F");

                entity.HasOne(d => d.SubscriptionTypeNavigation)
                    .WithMany(p => p.SubscriptionReceipts)
                    .HasForeignKey(d => d.SubscriptionType)
                    .HasConstraintName("FK__Subscript__Subsc__71D1E811");
            });

            modelBuilder.Entity<SubscriptionType>(entity =>
            {
                entity.ToTable("SubscriptionType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("smallmoney");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.SportSectionNavigation)
                    .WithMany(p => p.SubscriptionTypes)
                    .HasForeignKey(d => d.SportSection)
                    .HasConstraintName("FK__Subscript__Sport__72C60C4A");
            });

            modelBuilder.Entity<TrainingSchedule>(entity =>
            {
                entity.ToTable("TrainingSchedule");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.DayNavigation)
                    .WithMany(p => p.TrainingSchedules)
                    .HasForeignKey(d => d.Day)
                    .HasConstraintName("FK__TrainingSch__Day__73BA3083");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("University");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
