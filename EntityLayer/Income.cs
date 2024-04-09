using Entities;
using EntityLayer.Enums;

namespace EntityLayer
{
    public class Income
    {
        public int IncomeID { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        public TypeOfIncome Type { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public Income() { }
    }
}
