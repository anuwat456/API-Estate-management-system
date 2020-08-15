using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreateHouseModel
    {
        [StringLength(256), Required]
        public string Id { get; set; }

        [StringLength(256), Required]
        public string LaneHouse { get; set; }

        [StringLength(256), Required]
        public string ColorHouse { get; set; }

        [StringLength(256), Required]
        public string AreaHouse { get; set; }
    }
}
