using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$"), Required, StringLength(30)]
        public string CategoryName { get; set; }
    }
}