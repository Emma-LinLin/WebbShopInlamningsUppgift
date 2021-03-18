using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebbShopInlamningsUppgift.Database;
using WebbShopInlamningsUppgift.Models;

namespace WebbShopInlamningsUppgift.API
{
    class Helper
    {
        public static int GetUserID(string name)
        {
            using(var db = new WebbshopContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Name == name);
                if(user != null)
                {
                    return user.ID;
                }
                return 0;
            }
        }

        public static int GetBookID(string title)
        {
            using (var db = new WebbshopContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Title == title);
                if(book != null)
                {
                    return book.ID;
                }
                return 0;
            }
        }

        public static Books GetBookObject(string title)
        {
            using (var db = new WebbshopContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Title == title);
                if (book != null)
                {
                    return book;
                }
                return null;
            }
        }

        public static int GetCategoryId(string genere)
        {
            using (var db = new WebbshopContext())
            {
                var category = db.BookCategories.FirstOrDefault(b => b.Genere == genere);
                if (category != null)
                {
                    return category.ID;
                }
                return 0;
            }
        }
    }
}
