///////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcBook.Models {
   public class BookTitle {
      public int ID { get; set; }

      [RegularExpression(@"[0-9]+", ErrorMessage = "The ISBN must be between 10 and 13 digits")]
      [Required]
      [StringLength(13, MinimumLength = 10)]
      public string ISBN { get; set; }

      [Display(Name = "Book Title")]
      [Required]
      [StringLength(100, MinimumLength = 2)]
      public string Title { get; set; }

      [Display(Name = "Edition Number")]
      [Range(0, 20)]
      [Required(ErrorMessage = "Integer value required")]
      public int EditionNumber { get; set; }

      [Display(Name = "Copyright Year")]
      [Required]
      [RegularExpression(@"[0-9]+", ErrorMessage = "The Copyright Year must consist of digit(s)")]
      [StringLength(4, MinimumLength = 1)]
      public string Copyright { get; set; }
   }//close class BookTitle


   public class BookTitleDBContext : DbContext {
      public DbSet<BookTitle> Titles { get; set; }
   }//close class BookTitleDBContext

}//close namespace MvcBook.Models