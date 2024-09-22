using LibraryManagementSystem.Models.Repositories;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorRepository _authorRepository;
        public AuthorController(IWebHostEnvironment environment)
        {
            string csvFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "LibraryManagementAuthorsDB.csv");
            _authorRepository = new AuthorRepository(csvFilePath);
        }


        public IActionResult ViewAll(string search,int page = 1)
        {
            int pageSize = 12;
            var authors = _authorRepository.GetAll().OrderByDescending(I => I.UpdateDate).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                authors = authors.Where(I => I.FullName.Contains(search) || I.About.Contains(search)).ToList();
            }

            var totalAuthors = authors.Count();
            var totalPages = (int)Math.Ceiling(totalAuthors / (double)pageSize);


            int maxPagesToShow = 10;
            int startPage = Math.Max(1, page - maxPagesToShow / 2);
            int endPage = Math.Min(totalPages, startPage + maxPagesToShow - 1);


            if (endPage - startPage < maxPagesToShow - 1)
            {
                startPage = Math.Max(1, endPage - maxPagesToShow + 1);
            }

            var pagedAuthors = authors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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


        [HttpGet]
        public IActionResult View(Guid authorId)
        {
            var authorViewModel=_authorRepository.GetById(authorId);
            
            return View(authorViewModel);
        }



        [HttpPost]
        public IActionResult Update(AuthorUpdateViewModel author)
        {
            if (author.AuthorId == Guid.Empty)
            {
                
                return BadRequest("Invalid AuthorId.");
            }


            if (!ModelState.IsValid)
            {
                return RedirectToAction("ViewAll");
            }

            var findAuthor= _authorRepository.GetById(author.AuthorId);

            findAuthor.FullName = author.FullName;
            findAuthor.Birthdate = author.Birthdate;
            findAuthor.About=author.About;
            findAuthor.ImageUrl = author.ImageUrl;
            findAuthor.UpdateDate=DateTime.Now;

            _authorRepository.Update(findAuthor);

            return RedirectToAction("ViewAll");
        }


        public IActionResult Delete(Guid authorId)
        {
            _authorRepository.DeleteById(authorId);

            return RedirectToAction("ViewAll");
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        public IActionResult Create(AuthorCreateViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            var addedAuthor = new AuthorViewModel() {  FullName=author.FullName, About=author.About, AuthorId= Guid.NewGuid(), Birthdate=author.Birthdate, ImageUrl=author.ImageUrl, IsDeleted=0, UpdateDate=DateTime.Now };
            _authorRepository.Add(addedAuthor);

            return RedirectToAction("ViewAll");
        }
    }
}
