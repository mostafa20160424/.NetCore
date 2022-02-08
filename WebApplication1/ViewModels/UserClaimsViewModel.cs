using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            UserClaims = new List<UserClaim>();
        }
        public string UserId { get; set; }
        public List<UserClaim> UserClaims { get; set; }
    }
}
