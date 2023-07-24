using BooksAPI.Model.Entities;
using BooksAPI.Services.BooksEditor.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Services.BooksEditor
{
    public interface IBookEditor
    {
        Task<Book> AddBook(AddBookDTO newBook);
        Task<Book> EditBook(int id, EditBookDTO newBook, bool patchOnly = false);

    }
}
