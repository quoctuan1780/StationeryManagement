using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Provider
    {
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string ProviderName { get; set; }

        public virtual IList<ProductProvider> ProductProviders { get; set; }
    }
}
