using LibraryManagementSystem.Models.Repositories;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;
        

        public BookController(IWebHostEnvironment environment)
        {
            string csvFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "LibraryManagementBooksDB.csv");
            _bookRepository = new BookRepository(csvFilePath);
        }


        public IActionResult ViewAll(string search,int page = 1)
        {
            int pageSize = 12;
            var books = _bookRepository.GetAll().OrderByDescending(I => I.UpdateDate).ToList();



            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(I=>I.BookTitle.Contains(search)|| I.Genre.Contains(search)|| I.AuthorName.Contains(search)).ToList();
            }

            var totalBooks = books.Count();
            var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);


            int maxPagesToShow = 10;
            int startPage = Math.Max(1, page - maxPagesToShow / 2);
            int endPage = Math.Min(totalPages, startPage + maxPagesToShow - 1);


            if (endPage - startPage < maxPagesToShow - 1)
            {
                startPage = Math.Max(1, endPage - maxPagesToShow + 1);
            }

            var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var bookViewModel = new BookListViewModel
            {
                Books = pagedBooks,
                CurrentPage = page,
                TotalPages = totalPages,
                StartPage = startPage,
                EndPage = endPage
            };

            return View(bookViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        public IActionResult Create(BookCreateViewModel book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            var addedBook = new BookViewModel() { AuthorName = book.AuthorName, BookTitle = book.BookTitle, Description = book.Description, Genre = book.Genre, ImageURL = book.ImageURL, ISBN = book.ISBN, Publisher = book.Publisher, PublicationYear = book.PublicationYear, Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), IsDeleted=0, UpdateDate=DateTime.Now };
            _bookRepository.Add(addedBook);

            return RedirectToAction("ViewAll");
        }



        [HttpPost]
        public IActionResult Update(BookUpdateViewModel book)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ViewAll");
            }

            var findBook=_bookRepository.GetById(book.BookId);

            findBook.ImageURL = book.ImageURL;
            findBook.AuthorName = book.AuthorName;
            findBook.Publisher = book.Publisher;
            findBook.BookTitle = book.BookTitle;
            findBook.Description = book.Description;
            findBook.PublicationYear = book.PublicationYear;
            findBook.UpdateDate = DateTime.Now;

            _bookRepository.Update(findBook);

            return RedirectToAction("ViewAll");
        }



        public IActionResult Delete(Guid bookId)
        {
            _bookRepository.DeleteById(bookId);

            return RedirectToAction("ViewAll");
        }



        [HttpGet]
        public IActionResult View(Guid bookId)
        {

            var bookViewModel = _bookRepository.GetById(bookId);

            return View(bookViewModel);
        }
    }
}

