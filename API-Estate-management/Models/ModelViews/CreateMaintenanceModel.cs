using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreateMaintenanceModel
    {
        [Column(TypeName = "text"), Required]
        public string Detail { get; set; }

        [StringLength(256), Required]
        public string Location { get; set; }

        [StringLength(256)]
        public string ImageMainTen { get; set; }

        [Required]
        public bool StatusMainten { get; set; }
    }
}
