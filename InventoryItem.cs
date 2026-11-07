using System.ComponentModel.DataAnnotations;

namespace DN_8.Models;

public class InventoryItem
{
    public int ItemId { get; set; }

    [Required(ErrorMessage = "Item name is required.")]
    [StringLength(100)]
    [Display(Name = "Item Name")]
    public string ItemName { get; set; } = "";

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(60)]
    public string Category { get; set; } = "";

    [Range(0, 1000000, ErrorMessage = "Quantity must be zero or more.")]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "99999999", ErrorMessage = "Price must be greater than zero.")]
    [Display(Name = "Unit Price")]
    public decimal UnitPrice { get; set; }

    [StringLength(100)]
    public string? Supplier { get; set; }
}
