using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebbShopInlamningsUppgift.Models
{
    class BookCategory
    {
        [Key]
        public int ID { get; set; }
        public string Genere { get; set; }
    }
}
