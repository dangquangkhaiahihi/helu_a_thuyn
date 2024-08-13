using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.IdentityExtensions;
using CSMS.Entity.IdentityExtensions.IdentityMapping;
using CSMS.Entity.Issues;
using CSMS.Entity.LogHistory;
using CSMS.Entity.RoleProject;
using CSMS.Entity.SecurityMatrix;
using CSMSBE.Core.Helper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CSMS.Entity
{
    public partial class csms_dbContext : IdentityDbContext<User, Role, string, UserClaim
        , UserRole, UserLogin, RoleClaim, UserTokens>
    {
        private readonly IConfiguration _configuration;
        public csms_dbContext()
        {
        }
        public csms_dbContext(DbContextOptions<csms_dbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=localhost:5433;Database=csms_khai;Username=speckle;Password=speckle;");
            }
        }
        #region
        public override DbSet<User> Users { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<FileExtensions> FileExtensions { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<SecurityMatrices> SecurityMatrix { get; set; }
        public DbSet<SecurityMatrix.Action> Actions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TypeProject> TypeProject { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public DbSet<SomeTable> SomeTables { get; set; }
        public DbSet<Commune> Communes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<LogHistoryEntity> LogHistoryEntities { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<ModelVersion> ModelVersions { get; set; }
        public DbSet<RoleProject.RoleProject> RoleProjects { get; set; }
        public DbSet<ActionProject> ActionProjects { get; set; }
        public DbSet<SecurityMatrixProject> SecurityMatrixProjects { get; set; }
        public DbSet<ProjectUserRole> ProjectUserRoles { get; set; }
        public DbSet<PushNotification> PushNotifications { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AspNetRefreshTokensMap());

            base.OnModelCreating(modelBuilder);
            #region core model builder

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.ToTable("Screen", schema: "sys");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.ParentId)
                      .HasColumnName("parent_id");

                entity.HasOne(e => e.Parent)
                      .WithMany(p => p.Childrent)
                      .HasForeignKey(e => e.ParentId);

                entity.Property(e => e.Code)
                      .HasColumnName("code")
                      .IsRequired();

                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired();

                entity.Property(e => e.Title)
                      .HasColumnName("title")
                      .IsRequired();

                entity.Property(e => e.Icon)
                      .HasColumnName("icon");

                entity.Property(e => e.Order)
                      .HasColumnName("order")
                      .IsRequired();

                entity.HasMany(e => e.SecurityMatrices)
                      .WithOne(sm => sm.Screen)
                      .HasForeignKey(sm => sm.ScreenId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.PushNotifications)
                    .WithOne(e => e.AppUser)
                    .HasForeignKey(e => e.AppUserId);
            });

            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserClaim>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserClaims)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<RoleClaim>()
                .HasOne(e => e.Role)
                .WithMany(e => e.RoleClaims)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<SecurityMatrices>()
                .HasOne(e => e.Action)
                .WithMany(e => e.SecurityMatrices)
                .HasForeignKey(e => e.ActionId);

            modelBuilder.Entity<SecurityMatrices>()
                .HasOne(e => e.Screen)
                .WithMany(e => e.SecurityMatrices)
                .HasForeignKey(e => e.ScreenId);

            modelBuilder.Entity<SecurityMatrices>()
                .HasOne(e => e.Role)
                .WithMany(e => e.SecurityMatrices)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserLogin>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();
            modelBuilder.Entity<Issue>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Issue>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Issue>()
               .HasOne(i => i.User)
               .WithMany(u => u.Issues)
               .HasForeignKey(i => i.UserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(i => i.ProjectId)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Comment>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
               .HasMany(u => u.Comments)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId);
                
            modelBuilder.Entity<Issue>()
                .HasMany(i => i.Comments)
                .WithOne(c => c.Issues)
                .HasForeignKey(c => c.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Model>()
                 .HasKey(i => i.Id);

            modelBuilder.Entity<Model>()
                .Property(m => m.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Model>()
                .Property(m => m.Name)
                .HasColumnType("text");

            modelBuilder.Entity<Model>()
                .HasMany(u => u.Issues)
                .WithOne(c => c.Model)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Model>()
                .HasMany(u => u.ModelVersion)
                .WithOne(c => c.Model)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Project>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Project>()
                .Property(p => p.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Project>()
                .HasMany(u => u.Models)
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(u => u.Documents)
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(u => u.ProjectUserRoles)
                .WithOne(c => c.Projects)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Parent)
                .WithMany(d => d.Children);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectUserRole>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<ProjectUserRole>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProjectUserRole>()
                .HasOne(pur => pur.Users)
                .WithMany(u => u.ProjectUserRoles);

            modelBuilder.Entity<ProjectUserRole>()
                .HasOne(pur => pur.Projects)
                .WithMany(p => p.ProjectUserRoles)
                .HasForeignKey(pur => pur.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectUserRole>()
                .HasOne(pur => pur.Roles)
                .WithMany(r => r.ProjectUserRoles)
                .HasForeignKey(pur => pur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SecurityMatrixProject>()
                          .HasKey(i => i.Id);

            modelBuilder.Entity<SecurityMatrixProject>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<SecurityMatrixProject>()
                .HasOne(sm => sm.Role)
                .WithMany(r => r.SecurityMatrixProjects)
                .HasForeignKey(sm => sm.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SecurityMatrixProject>()
                .HasOne(sm => sm.Action)
                .WithMany(a => a.SecurityMatrixProjects)
                .HasForeignKey(sm => sm.ActionId);

            modelBuilder.Entity<ActionProject>()
                          .HasKey(i => i.Id);

            modelBuilder.Entity<RoleProject.RoleProject>()
                    .HasOne(rp => rp.Projects)
                    .WithMany(p => p.RoleProjects)
                    .HasForeignKey(rp => rp.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PushNotification>(entity =>
            {
                {
                    entity.ToTable(nameof(PushNotifications), schema: Constant.Schema.CSMS);

                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd();

                    entity.Property(e => e.Title)
                        .IsRequired()
                        .HasMaxLength(200);

                    entity.Property(e => e.Body)
                        .IsRequired()
                        .HasMaxLength(1000);

                    entity.HasOne(e => e.AppUser)
                        .WithMany(u => u.PushNotifications)
                        .HasForeignKey(n => n.AppUserId);

                    entity.Property(e => e.SentDate)
                        .IsRequired();

                    entity.Property(e => e.IsRead)
                        .IsRequired();
                }
            });
            #endregion
        }
    }
}
