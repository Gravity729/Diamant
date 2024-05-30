using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamant.Models
{
    public partial class ReturnedItem
    {
        public int ProductId { get; set; }

        public string NameProduct { get; set; } = null!;

        public decimal AssessedValue { get; set; }

        public decimal BailAmount { get; set; }

        public DateOnly DueDate { get; set; }

        public DateOnly ShelfLife { get; set; }

        public string StatusProduct { get; set; } = null!;

        public int ClientId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime DeletionDate { get; set; }

    }
}
