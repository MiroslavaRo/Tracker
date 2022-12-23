using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Models
{
    public class Transaction
    {
        [Key] //PK
        public int TransactionId { get; set; }


        public int Amount { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Note { get; set; } // not required
        public DateTime Date { get; set; } = DateTime.Now; //default value current time

        //FK 
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
