using System;
using Contendo.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Contendo.Db.Extensions.SeedData
{
    public static class UserSeedData
    {
        public static void AddDefaultUsers(this ModelBuilder modelBuilder)
        {
            var s1 = Guid.Parse("2c5b22e6-8b98-460f-98c9-6227e61b8d66");
            var u1 = Guid.Parse("88922a62-7304-4234-8b91-6a901cfbf779");
            var u2 = Guid.Parse("b074cf2c-20b2-4bba-870e-f86a11f32bb6");
            var u3 = Guid.Parse("511b1390-e52c-474d-b64e-1073c881b1e6");
            var u4 = Guid.Parse("5fcf796a-3c30-4d25-9110-0a84e9eb85a7");
            var u5 = Guid.Parse("6d672be4-fa62-4451-8d92-5fc983f61ab6");
            /*var u6 = Guid.Parse("bd358e80-e3c1-4bcd-8a7a-76709f1e7420");
            var u7 = Guid.Parse("70e0f77a-6509-4119-b506-769eeb5023b3");
            var u8 = Guid.Parse("a532370d-21ac-4fab-8ce1-a1879a098ec3");*/
            
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = s1,
                        Title = "Company",
                        FirstName = "Super",
                        LastName = "Admin",
                        Username = "SuperAdmin",
                        Email = "thecontendo@gmail.com",
                        Status = UserStatus.Active,
                        ValidFrom = DateTime.Parse("01.01.2021").ToUniversalTime(),
                        ValidTo = DateTime.MaxValue.ToUniversalTime()
                    },
                    new User
                    {
                        Id = u1,
                        Title = "Mr.",
                        FirstName = "Abhinav",
                        LastName = "Parankusham",
                        Username = "pac",
                        Email = "abhinav9p@gmail.com",
                        Description = "Entrepreneur",
                        Status = UserStatus.Active,
                        ValidFrom = DateTime.Parse("01.01.2021").ToUniversalTime(),
                        ValidTo = DateTime.MaxValue.ToUniversalTime()
                    },
                    new User
                    {
                        Id = u2,
                        Title = "Ms.",
                        FirstName = "Soumya",
                        LastName = "Pullakhandam",
                        Username = "soumya",
                        Email = "soumya9v@gmail.com",
                        Description = "Lead",
                        Status = UserStatus.Active,
                        ValidFrom = DateTime.Parse("01.01.2021").ToUniversalTime(),
                        ValidTo = DateTime.MaxValue.ToUniversalTime()
                    },
                    new User
                    {
                        Id = u3,
                        Title = "Mr.",
                        FirstName = "P2",
                        LastName = "Bhikkumalla",
                        Username = "p2",
                        Email = "p2@gmail.com",
                        Description = "Guitarist",
                        Status = UserStatus.Active,
                        ValidFrom = DateTime.Parse("01.01.2021").ToUniversalTime(),
                        ValidTo = DateTime.MaxValue.ToUniversalTime()
                    },
                    new User
                    {
                        Id = u4,
                        Title = "Ms.",
                        FirstName = "u4",
                        LastName = "u4",
                        Username = "u4",
                        Email = "u4@gmail.com",
                        Description = "Hello",
                        Status = UserStatus.Active,
                        ValidFrom = DateTime.Parse("01.01.2021").ToUniversalTime(),
                        ValidTo = DateTime.MaxValue.ToUniversalTime()
                    },
                    new User
                    {
                        Id = u5,
                        Title = "Mr.",
                        FirstName = "u5",
                        LastName = "u5",
                        Username = "u5",
                        Email = "u5@gmail.com",
                        Description = "Hi",
                        Status = UserStatus.Active,
                        ValidFrom = DateTime.Parse("01.01.2021").ToUniversalTime(),
                        ValidTo = DateTime.MaxValue.ToUniversalTime()
                    }
                );

            modelBuilder.Entity<UserContact>()
                .HasData(
                    new UserContact { UserId = u1, ContactId = u2 },
                    new UserContact { UserId = u1, ContactId = u3 },
                    new UserContact { UserId = u2, ContactId = u3 },
                    new UserContact { UserId = u3, ContactId = u4 },
                    new UserContact { UserId = u3, ContactId = u5 },
                    new UserContact { UserId = u3, ContactId = u2 },
                    new UserContact { UserId = u4, ContactId = u1 },
                    new UserContact { UserId = u5, ContactId = u4 }
                );
        }

    }
}