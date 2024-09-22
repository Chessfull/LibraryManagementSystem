using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.Repositories;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorRepository _authorRepository; // > Using author repository for user database processes
        
        // ▼ ctor ▼
        public AuthorController(IWebHostEnvironment environment)
        {
            // ▼ For dynamic my csv database root path I used IWebHostEnvironment.contentrootpath, its useful for downloading this project from anyone my datasource path will be dynamic in app_data folder ▼
            string csvFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "LibraryManagementAuthorsDB.csv");
            
            _authorRepository = new AuthorRepository(csvFilePath); // -> Instance for author repository to use data actions 
        }

        // ▼ Getting authors from csv file database for displaying processes
        public IActionResult ViewAll(string search,int page = 1)
        {
            // ▼ Getting authors with repository and turning to 'AuthorViewModel' for ViewAll razor view, then order according to UpdateDate ▼
            List<AuthorViewModel> authors = _authorRepository.GetAll()
                                                             .Select(I=>_authorRepository.EntityToViewModel(I)).ToList()
                                                             .OrderByDescending(I => I.UpdateDate).ToList();
           
            // ▼ This part for search box on page, with input searching database ▼
            if (!string.IsNullOrEmpty(search))
            {
                authors = authors.Where(I => I.FullName.Contains(search) || I.About.Contains(search)).ToList();
            }

            // ▼ This part all for pagination on page, per page 12 author
            int pageSize = 12;
            var totalAuthors = authors.Count();
            var totalPages = (int)Math.Ceiling(totalAuthors / (double)pageSize); // -> Defining total page with total count

            int maxPagesToShow = 10; // -> Managing view limit for page
            int startPage = Math.Max(1, page - maxPagesToShow / 2); //  Calculate the start page number (making sure it's not less than 1)
            int endPage = Math.Min(totalPages, startPage + maxPagesToShow - 1); // Calculate the end page number (making sure it doesn't exceed the total number of pages)

            // ▼ Adjust startPage if the number of pages to show is less than maxPagesToShow ▼
            if (endPage - startPage < maxPagesToShow - 1)
            {
                startPage = Math.Max(1, endPage - maxPagesToShow + 1);
            }

            // ▼ Retrieve the authors for the current page, using Skip and Take for pagination ▼
            var pagedAuthors = authors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // ▼ Instance of AuthorListViewModel for sending to ViewAll razor view. Include authors and managing pagination props ▼
            var authorViewModel = new AuthorListViewModel
            {
                Authors = pagedAuthors,
                CurrentPage = page,
                TotalPages = totalPages,
                StartPage = startPage,
                EndPage = endPage
            };

            return View(authorViewModel);
        }


        [HttpGet] // ▼ For viewing author more details other page from books page clickable author 
        public IActionResult View(Guid authorId)
        {
            // ▼ Getting author from database and returning view model with method I define in repository
            var authorViewModel=_authorRepository.EntityToViewModel(_authorRepository.GetById(authorId));
            
            return View(authorViewModel);
        }



        [HttpPost] // ▼ For update pop up post form
        public IActionResult Update(AuthorUpdateViewModel author)
        {
            // ▼ Checking modelstate
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ViewAll");
            }

            // ▼ Finding author by AuthorUpdateViewModel post id ▼
            var findAuthor= _authorRepository.GetById(author.AuthorId);

            // ▼ Updating and updating with repository method ▼
            findAuthor.FullName = author.FullName;
            findAuthor.Birthdate = author.Birthdate;
            findAuthor.About=author.About;
            findAuthor.ImageUrl = author.ImageUrl;
            findAuthor.UpdateDate=DateTime.Now;

            _authorRepository.Update(findAuthor);

            return RedirectToAction("ViewAll");
        }

        // ▼ For deleting button ▼
        public IActionResult Delete(Guid authorId)
        {
            _authorRepository.DeleteById(authorId);

            return RedirectToAction("ViewAll");
        }

        // ▼ For Create button get view ▼
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ▼ Post form create page ▼ 
        [HttpPost]
        public IActionResult Create(AuthorCreateViewModel author)
        {
            // ▼ Checking modelstate ▼
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            // ▼ With post form datas creating instance of author and sending to database with repository add
            var addedAuthor = new Author () {  FullName=author.FullName, About=author.About, AuthorId= Guid.NewGuid(), Birthdate=author.Birthdate, ImageUrl=author.ImageUrl, IsDeleted=0, UpdateDate=DateTime.Now };
            
            _authorRepository.Add(addedAuthor);

            return RedirectToAction("ViewAll");
        }
    }
}
