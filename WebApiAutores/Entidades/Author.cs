using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.Entidades
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage="The field {0} is required")]
        [StringLength(maximumLength:5, ErrorMessage ="The field {0} musn´t have more than {1} characters")]
        public string Nombre { get; set; }
        [Range(18,120)]
        [NotMapped]
        public int Age { get; set; }
        [CreditCard]
        [NotMapped]
        public string CreditCard { get; set; }
        public List<Book> Books { get; set; }

    }
}
