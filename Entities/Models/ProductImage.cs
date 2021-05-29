using System.Runtime.Intrinsics.X86;

namespace Entities.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string Image { get; set; }

        public bool PrimaryImage { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}