﻿using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        // Resolve
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        // GET
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.BookService.GetAllBooks(false);

                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                // LINQ
                var book = _manager.BookService.GetOneBookById(id, false);

                if (book is null)
                    return NotFound();  // 404

                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();  // 400

                _manager.BookService.CreateOneBook(book);

                return StatusCode(201, book);   // Created
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();  // 400

                // check book ?
                _manager.BookService.UpdateOneBook(id, book, false);

                //return Ok(book);
                return NoContent();   // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                _manager.BookService.DeleteOneBook(id, false);

                return NoContent();  // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PATCH
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                // check book ?
                var entity = _manager
                    .BookService
                    .GetOneBookById(id, true);

                if (entity is null)
                    return NotFound();  // 404

                bookPatch.ApplyTo(entity);

                _manager.BookService.UpdateOneBook(id, entity, true);

                return NoContent();  // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
