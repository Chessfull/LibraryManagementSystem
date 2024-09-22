using CsvHelper;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.ViewModels;
using System.Globalization;


namespace LibraryManagementSystem.Models.Repositories
{
    public class BookRepository : ICsvRepository<BookViewModel>
    {
        private readonly string _filePathCsv;

        public BookRepository(string filePathCsv)
        {
            _filePathCsv = filePathCsv;
        }

        public List<BookViewModel> GetAll()
        {
            using (var reader = new StreamReader(_filePathCsv))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var books = csv.GetRecords<Book>().Select(I => new BookViewModel { Id = I.Id, AuthorId = I.AuthorId, AuthorName = I.AuthorName, BookTitle = I.BookTitle, Description = I.Description, Genre = I.Genre, ImageURL = I.ImageURL, ISBN = I.ISBN, Publisher = I.Publisher, PublicationYear = I.PublicationYear, IsDeleted=I.IsDeleted, UpdateDate=I.UpdateDate }).ToList().Where(I=>I.IsDeleted==0).ToList(); 
                return books;
            }
        }

        public BookViewModel GetById(Guid bookId)
        {
            return GetAll().Find(I => I.Id == bookId);
        }

        public void Add(BookViewModel entity)
        {
            var books = GetAll();
            books.Add(entity);
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }

        public void Update(BookViewModel entity)
        {
            var books = GetAll();

            var bookIndex = books.FindIndex(I => I.Id == entity.Id);

            books[bookIndex] = entity;
            

            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }

        public void DeleteById(Guid id)
        {
            var books = GetAll();

            var bookDeleted = books.SingleOrDefault(I => I.Id == id);

            if (bookDeleted != null)
                bookDeleted.IsDeleted = 1;

            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }

       
    }
}
