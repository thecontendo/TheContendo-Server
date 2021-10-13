using Contendo.Models.Challenges;
using Contendo.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Contendo.Db.Extensions.EntityConfiguration
{
    public static class EntityConfigurationExtensions
    {
        public static void ApplyEntityConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(c => new { c.Username, c.Email }).IsUnique();

            #region UserContact
            modelBuilder.Entity<UserContact>()
                .HasKey(t => new { t.ContactId, t.UserId });

            modelBuilder.Entity<UserContact>()
                .HasOne(pt => pt.Contact)
                .WithMany(p => p.ContactUsers)
                .HasForeignKey(pt => pt.ContactId)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<UserContact>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.UserContacts)
                .HasForeignKey(pt => pt.UserId)
                .HasPrincipalKey(e => e.Id);
            #endregion

            #region Challenge
            /*modelBuilder.Entity<Challenge>()
                .HasKey(t => new { t.ChallengerId, t.ParticipantId, t.SportId });

            modelBuilder.Entity<Challenge>()
                .HasOne(pt => pt.Challenger)
                .WithMany(p => p.)
                .HasForeignKey(pt => pt.ContactId);

            modelBuilder.Entity<UserContact>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.UserContacts)
                .HasForeignKey(pt => pt.UserId);*/

            #endregion
            
        }

    }
}