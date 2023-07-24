using BooksAPI.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.DataLayer.Abstractions.Repositories
{
    public interface IBookRepository
    {
        Task Add(Book book);
        Task<Book?> Get(int id);
        Task Update(Book book);
        Task Delete(int id);

    }
}
