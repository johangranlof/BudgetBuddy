using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            context.Database.EnsureCreated();

            Populate.PopulateData(context);
        }
    }
}