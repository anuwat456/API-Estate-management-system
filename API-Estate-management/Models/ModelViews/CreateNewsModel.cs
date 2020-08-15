using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreateNewsModel
    {
        [StringLength(256), Required]
        public string Subject { get; set; }

        [Column(TypeName = "text"), Required]
        public string Detail { get; set; }

        [StringLength(100)]
        public string ImageNews { get; set; }
    }
}
