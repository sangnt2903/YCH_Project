using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace CalculateSalaryOfFleet.Models
{
    public partial class FleetsTripsContext : DbContext
    {
        private readonly string _connectionString;
        public FleetsTripsContext()
        {
        }

        public FleetsTripsContext(DbContextOptions<FleetsTripsContext> options)
            : base(options)
        {
        }

        //public FleetsTripsContext(IOptions<DbConnectionInfo> dbConnectionInfo)
        //{
        //    _connectionString = dbConnectionInfo.Value.FTContext;
        //}

        public virtual DbSet<DeliveryCustomers> DeliveryCustomers { get; set; }
        public virtual DbSet<DesLocateException> DesLocateException { get; set; }
        public virtual DbSet<Drivers> Drivers { get; set; }
        public virtual DbSet<Excels> Excels { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<MoneyByBigCar> MoneyByBigCar { get; set; }
        public virtual DbSet<MoneyJob> MoneyJob { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<RawData> RawData { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Rules> Rules { get; set; }
        public virtual DbSet<ServiceLevel> ServiceLevel { get; set; }
        public virtual DbSet<Trucks> Trucks { get; set; }
        public virtual DbSet<TruckSize> TruckSize { get; set; }
        public virtual DbSet<TruckSizeType> TruckSizeType { get; set; }
        public virtual DbSet<TruckWeightType> TruckWeightType { get; set; }
        public virtual DbSet<TypeJob> TypeJob { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=10.216.36.18;Database=FleetsTrips;User ID=triptest; Password= trip@123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryCustomers>(entity =>
            {
                entity.HasKey(e => e.DeliveryCustCode);

                entity.Property(e => e.DeliveryCustCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DeliveryAddress).HasMaxLength(200);

                entity.Property(e => e.ServiceLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DesLocateException>(entity =>
            {
                entity.HasKey(e => e.DesLocationNo);

                entity.Property(e => e.DeslocationFullName).HasMaxLength(50);

                entity.Property(e => e.DeslocationName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Drivers>(entity =>
            {
                entity.HasKey(e => e.DriverIcno);

                entity.Property(e => e.DriverIcno)
                    .HasColumnName("DriverICNo")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DriverName).HasMaxLength(100);

                entity.Property(e => e.DriverPhone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Excels>(entity =>
            {
                entity.HasKey(e => e.ExcelCode);

                entity.Property(e => e.ExcelFileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExcelUploadedDate).HasColumnType("date");
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobNo);

                entity.Property(e => e.JobNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DriverIcno)
                    .HasColumnName("DriverICNo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TruckId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.DriverIcnoNavigation)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.DriverIcno)
                    .HasConstraintName("FK_Jobs_Drivers1");

                entity.HasOne(d => d.Truck)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.TruckId)
                    .HasConstraintName("FK_Jobs_Trucks");
            });

            modelBuilder.Entity<MoneyByBigCar>(entity =>
            {
                entity.HasKey(e => e.TripNo);

                entity.Property(e => e.TripNo).ValueGeneratedNever();

                entity.Property(e => e.MoneyOfServiceLevelHcmc).HasColumnName("MoneyOfServiceLevelHCMC");

                entity.Property(e => e.MoneyOfServiceLevelHcmp).HasColumnName("MoneyOfServiceLevelHCMP");
            });

            modelBuilder.Entity<MoneyJob>(entity =>
            {
                entity.Property(e => e.DescriptionJobOrder).HasMaxLength(50);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.AtdcompleteDate)
                    .HasColumnName("ATDCompleteDate")
                    .HasColumnType("date");

                entity.Property(e => e.DeliveryCustCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TranportAgent)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.DeliveryCustCodeNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryCustCode)
                    .HasConstraintName("FK_Orders_DeliveryCustomers");

                entity.HasOne(d => d.JobNoNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.JobNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Jobs");
            });

            modelBuilder.Entity<RawData>(entity =>
            {
                entity.HasKey(e => e.RawId);

                entity.Property(e => e.RawId).HasColumnName("RawID");

                entity.Property(e => e.AtdcompleteDate)
                    .HasColumnName("ATDCompleteDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliveryAddress).HasMaxLength(200);

                entity.Property(e => e.DeliveryCustCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DriverName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DriverPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransportAgent)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TruckId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TruckType).HasMaxLength(30);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rules>(entity =>
            {
                entity.HasKey(e => e.RuleId);

                entity.Property(e => e.RuleId).HasColumnName("RuleID");
            });

            modelBuilder.Entity<ServiceLevel>(entity =>
            {
                entity.HasKey(e => e.ServiceLevelNo);

                entity.Property(e => e.ServiceLevelNo).ValueGeneratedNever();

                entity.Property(e => e.ServiceLevelName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trucks>(entity =>
            {
                entity.HasKey(e => e.TruckId);

                entity.Property(e => e.TruckId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.TruckType).HasMaxLength(30);

                entity.HasOne(d => d.TruckTypeNavigation)
                    .WithMany(p => p.Trucks)
                    .HasForeignKey(d => d.TruckType)
                    .HasConstraintName("FK_Trucks_TruckSize");
            });

            modelBuilder.Entity<TruckSize>(entity =>
            {
                entity.HasKey(e => e.TruckType);

                entity.Property(e => e.TruckType)
                    .HasMaxLength(30)
                    .ValueGeneratedNever();

                entity.HasOne(d => d.TruckSizeNavigation)
                    .WithMany(p => p.TruckSize)
                    .HasForeignKey(d => d.TruckSizeId)
                    .HasConstraintName("FK_TruckSize_TruckSizeType");
            });

            modelBuilder.Entity<TruckSizeType>(entity =>
            {
                entity.HasKey(e => e.TruckSizeId);

                entity.Property(e => e.TruckSizeName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TruckWeightType>(entity =>
            {
                entity.Property(e => e.TruckWeightType1)
                    .HasColumnName("TruckWeightType")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypeJob>(entity =>
            {
                entity.HasKey(e => e.TypeJobNo);

                entity.Property(e => e.TypeJob1)
                    .HasColumnName("TypeJob")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TypeJobDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAccount)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserAccount_Role");
            });
        }
    }
}
