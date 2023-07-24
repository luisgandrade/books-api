using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Services.BooksEditor.DTOs
{
    public record EditBookDTO(string? title, string? author, int? publishYear);
}
