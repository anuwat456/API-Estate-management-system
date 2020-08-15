using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [StringLength(256), Required]
        public string Subject { get; set; }

        [Column(TypeName = "text"), Required]
        public string Detail { get; set; }

        [StringLength(100)]
        public string ImageNews { get; set; }

        [DataType(DataType.Date), Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateNews { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ApplicationNews()
        {

        }
    }
}
