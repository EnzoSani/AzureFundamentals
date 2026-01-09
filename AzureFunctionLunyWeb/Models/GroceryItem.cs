using System.ComponentModel.DataAnnotations;

namespace AzureFunctionLunyWeb.Models
{
    public class GroceryItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
