namespace MvcBook.Migrations
{
    using MvcBook.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcBook.Models.BookTitleDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcBook.Models.BookTitleDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Titles.AddOrUpdate(i => i.Title,
                new BookTitle
                {
                    ISBN = "0747538492",
                    Title = "Harry Potter and the Chamber of Secrets",
                    EditionNumber = 1,
                    Copyright = "1999",
                },

                 new BookTitle
                 {
                     ISBN = "0690013590",
                     Title = "Bridge to Terabithia",
                     EditionNumber = 2,
                     Copyright = "1977",
                 },

                 new BookTitle
                 {
                     ISBN = "0747532699",
                     Title = "Harry Potter and the Sorcerer's Stone",
                     EditionNumber = 1,
                     Copyright = "1998",
                 },

                 new BookTitle
                 {
                     ISBN = "9780786221868",
                     Title = "Holes",
                     EditionNumber = 1,
                     Copyright = "1998",
                 }
           );

        }
    }
}
