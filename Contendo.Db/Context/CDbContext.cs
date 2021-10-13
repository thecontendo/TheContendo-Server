using Contendo.Db.Extensions.EntityConfiguration;
using Contendo.Db.Extensions.SeedData;
using Contendo.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Contendo.Models.Challenges;
using Contendo.Models.ContactRequests;
using Contendo.Models.Shots;

namespace Contendo.Db.Context
{
    public class CDbContext: DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<CDbContext> _logger;
        public CDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor, ILogger<CDbContext> logger) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public CDbContext(DbContextOptions options) : base(options)
        { }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEntityConfigurations();
            modelBuilder.AddDefaultUsers();
            modelBuilder.AddDefaultShots();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Shot> Shots { get; set; }
        
        //public DbSet<Participation> Participations { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
    }
}