using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationCommonFeeType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [StringLength(256), Required]
        public string Type { get; set; }

        public virtual ICollection<ApplicationCommonFee> CommonFees { get; set; }

        public ApplicationCommonFeeType()
        {

        }
    }
}
