using CsvHelper;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.ViewModels;
using System.Globalization;

namespace LibraryManagementSystem.Models.Repositories
{
    public class AuthorRepository:ICsvRepository<AuthorViewModel>
    {

        private readonly string _filePathCsv;

        public AuthorRepository(string filePathCsv)
        {
            _filePathCsv = filePathCsv;
        }

        public List<AuthorViewModel> GetAll()
        {
            using (var reader = new StreamReader(_filePathCsv))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var authors = csv.GetRecords<Author>().ToList().Select(I=>new AuthorViewModel() { About=I.About, Birthdate=I.Birthdate, FullName=I.FullName, AuthorId=I.AuthorId, ImageUrl=I.ImageUrl, IsDeleted=I.IsDeleted, UpdateDate=I.UpdateDate}).ToList().Where(I=>I.IsDeleted==0).ToList();
                return authors;
            }
        }
        public AuthorViewModel GetById(Guid id)
        {
            return GetAll().Find(I=>I.AuthorId==id);
        }

        public void Add(AuthorViewModel entity)
        {
            var authors = GetAll();
            authors.Add(entity);
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(authors);
            }
        }
        public void Update(AuthorViewModel entity)
        {
            var authors = GetAll();

            var authorIndex = authors.FindIndex(I => I.AuthorId == entity.AuthorId);

            authors[authorIndex] = entity;


            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(authors);
            }
        }

        public void DeleteById(Guid id)
        {
            var authors = GetAll();

            var authorDeleted = authors.SingleOrDefault(I => I.AuthorId == id);

            if (authorDeleted != null)
                authorDeleted.IsDeleted = 1;

            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(authors);
            }

        }




    }
}
