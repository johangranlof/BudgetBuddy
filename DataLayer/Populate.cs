using Entities;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class Populate
    {
        public static void PopulateData(ApplicationDbContext db)
        {
            // Seeding Users
            var users = new List<User>
            {
                new User { Name = "John Doe", Email = "JohnDoe@hotmail.com", Password = "Admin", Username = "Admin", Balance = 150000, Savings = 200000}
            };
            db.Users.AddRange(users);
            db.SaveChanges();
        }
    }
}
