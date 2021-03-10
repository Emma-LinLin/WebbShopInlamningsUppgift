using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebbShopInlamningsUppgift.Models
{
    class Users
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime SessionTimer { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

    }
}
