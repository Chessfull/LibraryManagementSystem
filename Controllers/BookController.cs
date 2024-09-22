using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.Repositories;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        

        private readonly BookRepository _bookRepository; // > Using book repository for user database processes


        public BookController(IWebHostEnvironment environment)
        {
            // ▼ For dynamic my csv database root path I used IWebHostEnvironment.contentrootpath, its useful for downloading this project from anyone my datasource path will be dynamic in app_data folder ▼
            string csvFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "LibraryManagementBooksDB.csv");
            
            _bookRepository = new BookRepository(csvFilePath); // -> Instance for book repository to use data actions
        }

        // ▼ Getting books from csv file database for displaying processes
        public IActionResult ViewAll(string search,int page = 1)
        {
            // ▼ Getting books with repository and turning to 'BookViewModel' for ViewAll razor view, then order according to UpdateDate ▼
            int pageSize = 12;
            List<BookViewModel> books = _bookRepository.GetAll()
                                       .Select(I => _bookRepository.EntityToViewModel(I)).ToList()
                                       .OrderByDescending(I => I.UpdateDate).ToList();

            // ▼ This part for search box on page, with input searching database ▼
            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(I=>I.BookTitle.Contains(search)|| I.Genre.Contains(search)|| I.AuthorName.Contains(search)).ToList();
            }

            // ▼ This part all for pagination on page, per page 12 author
            var totalBooks = books.Count();
            var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize); // -> Defining total page with total count


            int maxPagesToShow = 10; // -> Managing view limit for page
            int startPage = Math.Max(1, page - maxPagesToShow / 2); // -> Calculate the start page number (making sure it's not less than 1)
            int endPage = Math.Min(totalPages, startPage + maxPagesToShow - 1); // -> Calculate the end page number (making sure it doesn't exceed the total number of pages)

            // ▼ Adjust startPage if the number of pages to show is less than maxPagesToShow ▼
            if (endPage - startPage < maxPagesToShow - 1)
            {
                startPage = Math.Max(1, endPage - maxPagesToShow + 1);
            }

            // ▼ Retrieve the books for the current page, using Skip and Take for pagination ▼
            var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // ▼ Instance of BookListViewModel for sending to ViewAll razor view. Include authors and managing pagination props ▼
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

        [HttpGet] // ▼ For create book form page ▼
        public IActionResult Create()
        {
            return View();
        }




        [HttpPost] // ▼ For post form create book  ▼
        public IActionResult Create(BookCreateViewModel book)
        {
            // ▼ Checking modalstate ▼
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            // ▼ Creating new book from post form and adding to database with repos. ▼
            var addedBook = new Book() { AuthorName = book.AuthorName, BookTitle = book.BookTitle, Description = book.Description, Genre = book.Genre, ImageURL = book.ImageURL, ISBN = book.ISBN, Publisher = book.Publisher, PublicationYear = book.PublicationYear, Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), IsDeleted=0, UpdateDate=DateTime.Now };

            _bookRepository.Add(addedBook);

            return RedirectToAction("ViewAll");
        }



        [HttpPost] // ▼ For update pop up post form ▼
        public IActionResult Update(BookUpdateViewModel book)
        {
            // ▼ Checking modalstate ▼
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ViewAll");
            }

            // ▼ Finding which book will be updated by id ▼
            var findBook=_bookRepository.GetById(book.BookId);

            // ▼ Update values from post form and update with repos. ▼
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

        // ▼ Delete button ▼
        public IActionResult Delete(Guid bookId)
        {
            _bookRepository.DeleteById(bookId);

            return RedirectToAction("ViewAll");
        }

        [HttpGet] // ▼ Book details button ▼
        public IActionResult View(Guid bookId)
        {
            // ▼ Finding book by id and returning to viewmodel with repository method ▼
            var bookViewModel = _bookRepository.EntityToViewModel (_bookRepository.GetById(bookId));

            return View(bookViewModel);
        }
    }
   
}
        
