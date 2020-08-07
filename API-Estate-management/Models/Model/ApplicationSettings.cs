using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationSettings
    {
        public string JWT_Secret { get; set; }

        public string ExpiryTime { get; set; }

        public string ValidAudience { get; set; }

        public string SetRoleDefault { get; set; }
        
        public string DefaultImageUrl { get; set; }
    }
}
