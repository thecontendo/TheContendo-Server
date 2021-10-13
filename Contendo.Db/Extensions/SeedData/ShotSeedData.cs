using System;
using Contendo.Models.Shots;
using Microsoft.EntityFrameworkCore;

namespace Contendo.Db.Extensions.SeedData
{
    public static class ShotSeedData
    {
        public static void AddDefaultShots(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shot>()
                .HasData(
                    new Shot { Id = Guid.Parse("0a4263c0-4f89-421c-871c-1a811092c316"),  Name = "Push Ups", Icon = "pushups.png"},
                    new Shot { Id = Guid.Parse("7141c807-233b-42de-8b18-878f4c5d6f91"),  Name = "Burpees", Icon = "burpees.png"},
                    new Shot { Id = Guid.Parse("d0562f7f-bf94-44a3-a3e0-d8d40d419880"),  Name = "Jumping Jacks", Icon = "jumpingjacks.png" },
                    new Shot { Id = Guid.Parse("02b8a53a-9b37-439a-88d1-d0363d621508"),  Name = "Classical Plank", Icon = "classicplank.png" },
                    new Shot { Id = Guid.Parse("a5bb13cb-adb2-4bb6-b490-77bee49182e4"),  Name = "Straight Hand Plank", Icon = "straighthandplank.png" },
                    new Shot { Id = Guid.Parse("27b4717e-bc68-484d-b98b-07387425604c"),  Name = "Side Plank", Icon = "sideplank.png" }
                );
        }
    }
}