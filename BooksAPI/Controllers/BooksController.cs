using BooksAPI.DataLayer.Abstractions;
using BooksAPI.DataLayer.Abstractions.Repositories;
using BooksAPI.Services.BooksEditor;
using BooksAPI.Services.BooksEditor.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BooksAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookEditor _bookEditor;
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IBookRepository bookRepository, IBookEditor bookEditor, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _bookEditor = bookEditor;
            _unitOfWork = unitOfWork;
        }



        // GET: api/<BooksController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allBooks = await _bookRepository.GetAll();
            return Ok(allBooks);
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookRepository.Get(id);
            if (book is null)
                return NotFound();
            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required] AddBookDTO dto)
        {
            var bookCreated = await _bookEditor.AddBook(dto);
            return CreatedAtAction(nameof(Get), new { id = bookCreated.Id });
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody][Required] EditBookDTO dto)
        {
            try
            {
                await _bookEditor.EditBook(id, dto);
                return NoContent();
            }
            catch(ApplicationException ae)
            {
                return BadRequest(ae.Message);
            }
            
        }

        // PUT api/<BooksController>/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody][Required] EditBookDTO dto)
        {
            try
            {
                await _bookEditor.EditBook(id, dto, true);
                return NoContent();
            }
            catch (ApplicationException ae)
            {
                return BadRequest(ae.Message);
            }

        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookRepository.Delete(id);
            await _unitOfWork.Commit();
            return NoContent();
        }
    }
}
