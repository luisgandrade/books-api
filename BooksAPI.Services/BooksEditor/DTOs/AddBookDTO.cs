using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Services.BooksEditor.DTOs
{
    public record AddBookDTO([Required]string title, [Required] string author, int publishYear);
}
