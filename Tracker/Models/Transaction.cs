using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Models
{
    public class Transaction
    {
        [Key] //PK
        public int TransactionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Note { get; set; } // not required

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now; //default value current time

        //FK 
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [NotMapped] 
        public string? CategoryTitleWithIcon {
            get {
                return Category == null ? "" : Category.Icon + " " + Category.Title;

            }
        }
        [NotMapped]
        public string? FormatedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ")+Amount.ToString("C0");

            }
        }
    }
}
