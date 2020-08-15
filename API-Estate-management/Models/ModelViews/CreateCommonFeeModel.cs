using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreateCommonFeeModel
    {
        [Column(TypeName = "decimal(0, 2)"), Required]
        public decimal Amount { get; set; }

        [Required]
        public string Detail { get; set; }

        [Required]
        public bool StatusPay { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime KeepDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime GetDate { get; set; }
    }
}
