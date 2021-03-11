using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebbShopInlamningsUppgift.Models
{
    class Books
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }

        //Entity framework created not nullable field for foreign key, solution:
        //https://stackoverflow.com/questions/5668801/entity-framework-code-first-null-foreign-key
        public int? BookCategoryId { get; set; }
        public BookCategory BookCategory { get; set; }
    }
}
