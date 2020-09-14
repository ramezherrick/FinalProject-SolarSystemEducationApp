using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
