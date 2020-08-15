using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationCommonFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Column(TypeName = "decimal(6, 2)"), Required]
        public decimal Amount { get; set; }

        [Column(TypeName = "text"), Required]
        public string Detail { get; set; }

        [Required]
        public bool StatusPay { get; set; }

        [DataType(DataType.Date), Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime KeepDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime GetDate { get; set; }


        public ApplicationHouse House { get; set; }
        public string HouseId { get; set; }

        public ApplicationCommonFeeType CommonFeeType { get; set; }
        public string CommonFeeTypeId { get; set; }
        

        public ApplicationCommonFee()
        {

        }
    }
}
