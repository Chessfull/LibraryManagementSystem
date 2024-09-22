using CsvHelper;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.ViewModels;
using System.Globalization;

namespace LibraryManagementSystem.Models.Repositories
{
    public class AuthorRepository:ICsvRepository<Author> // With csvrepository<T> interface
    {

        private readonly string _filePathCsv; // -> Csv file path for database 

        // ▼ Ctor when creating instance take file path of repository connection to csv ▼
        public AuthorRepository(string filePathCsv)
        {
            _filePathCsv = filePathCsv;
        }

        // ▼ This method is getting all values from database ▼
        public List<Author> GetAll()
        {
            // ▼ This part, methods coming from CsvHelper package reading database ▼
            using (var reader = new StreamReader(_filePathCsv))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var authors = csv.GetRecords<Author>().Where(I=>I.IsDeleted==0).ToList(); // -> IsDeleted filter for soft delete process
                return authors;
            }
        }

        // ▼ This method is getting value from database match by id ▼
        public Author GetById(Guid id)
        {
            return GetAll().Find(I=>I.AuthorId==id);
        }

        // ▼ This method is adding value to database ▼
        public void Add(Author entity)
        {
            var authors = GetAll();
            authors.Add(entity);

            // ▼ This part, methods coming from CsvHelper package writing database ▼
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(authors);
            }
        }

        // ▼ This method is updating value from database ▼
        public void Update(Author entity)
        {
            var authors = GetAll();

            var authorIndex = authors.FindIndex(I => I.AuthorId == entity.AuthorId);

            authors[authorIndex] = entity;

            // ▼ This part, methods coming from CsvHelper package writing database after change ▼
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(authors);
            }
        }

        // ▼ This method is deleting value from database with id match ▼
        public void DeleteById(Guid id)
        {
            var authors = GetAll();

            var authorDeleted = authors.SingleOrDefault(I => I.AuthorId == id);

            if (authorDeleted != null)
                authorDeleted.IsDeleted = 1; // -> For soft delete process

            // ▼ This part, methods coming from CsvHelper package writing database after change ▼
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(authors);
            }

        }

        // ▼ This method is returning author entity to viewmodel for using razor view actions ▼
        public AuthorViewModel EntityToViewModel(Author entity)
        {
            return new AuthorViewModel {  About=entity.About, AuthorId=entity.AuthorId, Birthdate=entity.Birthdate, FullName=entity.FullName, ImageUrl=entity.ImageUrl, IsDeleted=entity.IsDeleted, UpdateDate=entity.UpdateDate };
        }
    }
}
