using BooksAPI.DataLayer.Abstractions;
using BooksAPI.DataLayer.Abstractions.Repositories;
using BooksAPI.Model.Entities;
using BooksAPI.Services.BooksEditor.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Services.BooksEditor
{
    public class DefaultBookEditor : IBookEditor
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DefaultBookEditor(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Book> AddBook(AddBookDTO newBook)
        {
            var bookEntity = new Book
            {
                Title = newBook.title,
                Author = newBook.author,
                PublishYear = newBook.publishYear
            };

            await _bookRepository.Add(bookEntity);
            await _unitOfWork.Commit();

            return bookEntity;
        }

        public async Task<Book> EditBook(int id, EditBookDTO bookDTO, bool patchOnly = false)
        {
            var bookEntity = await _bookRepository.Get(id);
            if (bookEntity is null)
                throw new ApplicationException("Book not found");
            if (patchOnly)
            {
                if(!string.IsNullOrWhiteSpace(bookDTO.title))
                    bookEntity.Title = bookDTO.title;
                if (!string.IsNullOrWhiteSpace(bookDTO.author))
                    bookEntity.Author = bookDTO.author;
                if (bookDTO.publishYear.HasValue)
                    bookEntity.PublishYear = bookDTO.publishYear.Value;
            }
            else
            {
                bookEntity.Title = bookDTO.title ?? string.Empty;
                bookEntity.Author = bookDTO.author ?? string.Empty;
                bookEntity.PublishYear = bookDTO.publishYear ?? 0;                
            }

            await _bookRepository.Update(bookEntity);
            await _unitOfWork.Commit();

            return bookEntity;
        }
    }
}
