using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Models
{
    public class Category
    {

        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string Icon { get; set; } = " "; //default value

        [Column(TypeName = "varchar(10)")]
        public string Type { get; set; } = "Expense"; //default value

        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }

    }
}
