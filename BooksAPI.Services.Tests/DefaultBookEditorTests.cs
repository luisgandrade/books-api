using BooksAPI.DataLayer.Abstractions;
using BooksAPI.DataLayer.Abstractions.Repositories;
using BooksAPI.Model.Entities;
using BooksAPI.Services.BooksEditor;
using BooksAPI.Services.BooksEditor.DTOs;
using FluentAssertions;
using Moq;

namespace BooksAPI.Services.Tests
{
    public class DefaultBookEditorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DefaultBookEditor _bookEditor;

        public DefaultBookEditorTests()
        {
            _bookRepositoryMock = new();
            _unitOfWorkMock = new();
            _bookEditor = new(_bookRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task AddingBookShouldCreateNewBookEntityAndAddToDb()
        {
            var dto = new AddBookDTO("title", "author", 2000);

            var bookAdded = await _bookEditor.AddBook(dto);

            bookAdded.Should().NotBeNull();
            bookAdded.Title.Should().Be(dto.title);
            bookAdded.Author.Should().Be(dto.author);
            bookAdded.PublishYear.Should().Be(dto.publishYear);

            _bookRepositoryMock.Verify(br => br.Add(bookAdded), Times.Once());
            _unitOfWorkMock.Verify(br => br.Commit(), Times.Once());
        }

        [Fact]
        public async Task EditBookShouldThrowExceptionIfBookEntityDoesNotExist()
        {
            var bookId = 1;
            var dto = new EditBookDTO(null, null, null);

            var editBookAction = async () => await _bookEditor.EditBook(bookId, dto, true);

            await editBookAction.Should().ThrowAsync<ApplicationException>();
        }

        [Fact]
        public async Task EditBookShouldReplaceAllPropertiesIfPathOnlyFlagIfNotSet()
        {
            var bookId = 1;
            var bookEntity = Mock.Of<Book>(b => b.Title == "Old Title" && b.Author == "Old author" && b.PublishYear == 2000);
            var dto = new EditBookDTO("New Title", null, 1000);

            _bookRepositoryMock.Setup(br => br.Get(bookId)).ReturnsAsync(bookEntity);

            var editedBook = await _bookEditor.EditBook(bookId, dto, false);

            editedBook.Should().NotBeNull();
            editedBook.Title.Should().Be(dto.title);
            editedBook.Author.Should().BeEmpty();
            editedBook.PublishYear.Should().Be(dto.publishYear);

            _bookRepositoryMock.Verify(br => br.Update(bookEntity), Times.Once());
            _unitOfWorkMock.Verify(br => br.Commit(), Times.Once());
        }

        [Theory]
        [InlineData(null, "new author", 1500)]
        [InlineData("new title", null, 1500)]
        [InlineData("new title", "new author", null)]
        public async Task EditBookShouldReplaceOnlyNotNullPropertiesIfPatchOnlyIsSet(string? newTitle, string? newAuthor, int? newPublishYear)
        {
            var bookId = 1;
            var bookEntity = Mock.Of<Book>(b => b.Title == "Old Title" && b.Author == "Old author" && b.PublishYear == 2000);
            var dto = new EditBookDTO(newTitle, newAuthor, newPublishYear);

            _bookRepositoryMock.Setup(br => br.Get(bookId)).ReturnsAsync(bookEntity);

            var editedBook = await _bookEditor.EditBook(bookId, dto, true);

            editedBook.Should().NotBeNull();
            editedBook.Title.Should().Be(dto.title ?? bookEntity.Title);
            editedBook.Author.Should().Be(dto.author ?? bookEntity.Author);
            editedBook.PublishYear.Should().Be(dto.publishYear ?? bookEntity.PublishYear);

            _bookRepositoryMock.Verify(br => br.Update(bookEntity), Times.Once());
            _unitOfWorkMock.Verify(br => br.Commit(), Times.Once());
        }

    }
}