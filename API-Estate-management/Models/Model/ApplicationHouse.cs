using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationHouse
    {
        [Key]
        [StringLength(256), Required]
        public string Id { get; set; }

        [StringLength(256), Required]
        public string LaneHouse { get; set; }

        [StringLength(256), Required]
        public string ColorHouse { get; set; }

        [StringLength(256), Required]
        public string AreaHouse { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ApplicationHouseType HouseType { get; set; }
        public string HouseTypeId { get; set; }

        public virtual ICollection<ApplicationMaintenance> Maintenances { get; set; }

        public virtual ICollection<ApplicationCommonFee> CommonFees { get; set; }

        public ApplicationHouse()
        {

        }
    }
}
