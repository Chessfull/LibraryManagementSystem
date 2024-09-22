using CsvHelper;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.ViewModels;
using System.Globalization;


namespace LibraryManagementSystem.Models.Repositories
{
    public class BookRepository : ICsvRepository<Book> // With csvrepository<T> interface
    {
        private readonly string _filePathCsv; // -> Csv file path for database 

        // ▼ Ctor when creating instance take file path of repository connection to csv ▼
        public BookRepository(string filePathCsv)
        {
            _filePathCsv = filePathCsv;
        }

        // ▼ This method is getting all values from database ▼
        public List<Book> GetAll()
        {
            // ▼ This part, methods coming from CsvHelper package reading database ▼
            using (var reader = new StreamReader(_filePathCsv))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var books = csv.GetRecords<Book>().Where(I=>I.IsDeleted==0).ToList(); 
                return books;
            }
        }

        // ▼ This method is getting value from database match by id ▼
        public Book GetById(Guid bookId)
        {
            return GetAll().Find(I => I.Id == bookId);
        }

        // ▼ This method is adding value to database ▼
        public void Add(Book entity)
        {
            var books = GetAll();
            books.Add(entity);

            // ▼ This part, methods coming from CsvHelper package writing database ▼
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }

        // ▼ This method is updating value from database ▼
        public void Update(Book entity)
        {
            var books = GetAll();

            var bookIndex = books.FindIndex(I => I.Id == entity.Id);

            books[bookIndex] = entity;

            // ▼ This part, methods coming from CsvHelper package writing database ▼
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }

        // ▼ This method is deleting value from database with id match ▼
        public void DeleteById(Guid id)
        {
            var books = GetAll();

            var bookDeleted = books.SingleOrDefault(I => I.Id == id);

            if (bookDeleted != null)
                bookDeleted.IsDeleted = 1;

            // ▼ This part, methods coming from CsvHelper package writing database ▼
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(books);
            }
        }

        // ▼ This method is returning book entity to viewmodel for using razor view actions ▼
        public BookViewModel EntityToViewModel(Book entity)
        {
            return new BookViewModel { AuthorId = entity.AuthorId, AuthorName = entity.AuthorName, IsDeleted = entity.IsDeleted, UpdateDate = entity.UpdateDate, BookTitle = entity.BookTitle, Description = entity.Description, Genre = entity.Genre, Id = entity.Id, ImageURL = entity.ImageURL, ISBN = entity.ISBN, PublicationYear = entity.PublicationYear, Publisher = entity.Publisher };
        }


    }
}
