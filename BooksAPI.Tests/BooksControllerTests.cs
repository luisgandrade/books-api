using BooksAPI.Controllers;
using BooksAPI.DataLayer.Abstractions;
using BooksAPI.DataLayer.Abstractions.Repositories;
using BooksAPI.Model.Entities;
using BooksAPI.Services.BooksEditor;
using BooksAPI.Services.BooksEditor.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BooksAPI.Tests
{
    public class BooksControllerTests
    {

        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookEditor> _bookEditorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly BooksController _booksController;

        public BooksControllerTests()
        {
            _bookRepositoryMock = new();
            _bookEditorMock = new();
            _unitOfWorkMock = new();
            _booksController = new(_bookRepositoryMock.Object, _bookEditorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async void GetAllShouldReturn200WithAllBooksFound()
        {
            var books = new[] { Mock.Of<Book>(), Mock.Of<Book>() };

            _bookRepositoryMock.Setup(br => br.GetAll()).ReturnsAsync(books);

            var response = await _booksController.Get();

            response.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(books);
        }

        [Fact]
        public async void GetByIdShouldReturn404IfNoBookIsFound()
        {
            var bookId = 1;

            var response = await _booksController.Get(bookId);

            response.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void GetByIdShouldReturn200WithBookIfItExists()
        {
            var bookId = 1;
            var book = Mock.Of<Book>(b => b.Id == bookId);

            _bookRepositoryMock.Setup(br => br.Get(bookId)).ReturnsAsync(book);

            var response = await _booksController.Get(bookId);

            response.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(book);
        }

        [Fact]
        public async void PostShouldAddBookAndReturn201WithIdOfBookCreated()
        {
            var bookDto = new AddBookDTO("title", "author", 2000);
            var expectedBookId = 1;
            var bookCreated = new Book { Id = expectedBookId };

            _bookEditorMock.Setup(be => be.AddBook(bookDto)).ReturnsAsync(bookCreated);

            var response = await _booksController.Post(bookDto);

            response.Should().BeAssignableTo<CreatedAtActionResult>();
            var responseAs201 = (CreatedAtActionResult)response;
            responseAs201.ActionName.Should().Be(nameof(BooksController.Get));
            responseAs201.RouteValues.Should().Contain(new KeyValuePair<string, object?>("id", expectedBookId));
            responseAs201.Value.Should().Be(bookCreated);

            _bookEditorMock.Verify(be => be.AddBook(bookDto), Times.Once());
        }

        [Fact]
        public async void PutShouldReturn400IfEditingABookThrowsAnApplicationException()
        {
            var bookDto = new EditBookDTO("title", "author", 2000);
            var bookId = 1;
            var editException = new ApplicationException("error");

            _bookEditorMock.Setup(be => be.EditBook(bookId, bookDto, false)).ThrowsAsync(editException);

            var response = await _booksController.Put(bookId, bookDto);

            response.Should().BeAssignableTo<BadRequestObjectResult>()
                .Which.Value.Should().Be(editException.Message);
            _bookEditorMock.Verify(be => be.EditBook(bookId, bookDto, false), Times.Once());
        }

        [Fact]
        public async void PutShouldReturn204IfEditingABookIsSuccessful()
        {
            var bookDto = new EditBookDTO("title", "author", 2000);
            var bookId = 1;

            var response = await _booksController.Put(bookId, bookDto);

            response.Should().BeAssignableTo<NoContentResult>();
            _bookEditorMock.Verify(be => be.EditBook(bookId, bookDto, false), Times.Once());
        }



        [Fact]
        public async void PatchShouldReturn400IfEditingABookThrowsAnApplicationException()
        {
            var bookDto = new EditBookDTO("title", "author", 2000);
            var bookId = 1;
            var editException = new ApplicationException("error");

            _bookEditorMock.Setup(be => be.EditBook(bookId, bookDto, true)).ThrowsAsync(editException);

            var response = await _booksController.Patch(bookId, bookDto);

            response.Should().BeAssignableTo<BadRequestObjectResult>()
                .Which.Value.Should().Be(editException.Message);
            _bookEditorMock.Verify(be => be.EditBook(bookId, bookDto, true), Times.Once());
        }

        [Fact]
        public async void PatchShouldReturn204IfEditingABookIsSuccessful()
        {
            var bookDto = new EditBookDTO("title", "author", 2000);
            var bookId = 1;

            var response = await _booksController.Patch(bookId, bookDto);

            response.Should().BeAssignableTo<NoContentResult>();
            _bookEditorMock.Verify(be => be.EditBook(bookId, bookDto, true), Times.Once());
        }
    }
}