using CsvHelper;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.ViewModels;
using System.Globalization;
using System.Net;

namespace LibraryManagementSystem.Models.Repositories
{
    public class UserRepository : ICsvRepository<User> // With csvrepository<T> interface
    {
        private readonly string _filePathCsv; // -> Csv file path for database 

        // ▼ Ctor when creating instance take file path of repository connection to csv ▼
        public UserRepository(string filePathCsv)
        {
            _filePathCsv = filePathCsv;
        }

        // ▼ This method is getting all values from database ▼
        public List<User> GetAll()
        {
            // ▼ This part, methods coming from CsvHelper package reading database ▼
            using (var reader = new StreamReader(_filePathCsv))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var users = csv.GetRecords<User>().ToList();
                
                return users;
            }
        }

        // ▼ This method is getting value from database match by id ▼
        public User GetById(Guid id)
        {
            return GetAll().Find(I => I.Id == id);
        }


        public void Add(User entity)
        {
            var users = GetAll();
            users.Add(entity);
            using (var writer = new StreamWriter(_filePathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(users);
            }
        }

        public void Update(User entity)
        {
           
        }

        public void DeleteById(Guid id)
        {
            
        }
    }
}
