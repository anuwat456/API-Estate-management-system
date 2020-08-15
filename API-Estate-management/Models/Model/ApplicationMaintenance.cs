using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationMaintenance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Column(TypeName = "text"), Required]
        public string Detail { get; set; }

        [StringLength(256), Required]
        public string Location { get; set; }

        [StringLength(256), Required]
        public string ImageMainTen { get; set; }

        [Required]
        public bool StatusMainten { get; set; }

        public ApplicationHouse House { get; set; }
        public string HouseId { get; set; }
    }
}
