using BooksAPI.DataLayer.Abstractions.Repositories;
using BooksAPI.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.DataLayer.EF.Repositories
{
    public class EFBooksRepository : IBookRepository
    {
        private readonly BooksContext _context;

        public EFBooksRepository(BooksContext context)
        {
            _context = context;
        }

        public async Task Add(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task Delete(int id)
        {
            var book = await Get(id);
            if (book is not null)
                _context.Books.Remove(book);
        }

        public async Task<Book?> Get(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public Task Update(Book book)
        {            
            //Update não faz sentido para o EF se o Change Tracker estiver ativado.
            //O método foi adicionado para atender a interface
            return Task.CompletedTask;
        }
    }
}
