using CsvHelper;
using LibraryManagementSystem.Models.Entities;
using LibraryManagementSystem.Models.ViewModels;
using System.Globalization;
using System.Net;

namespace LibraryManagementSystem.Models.Repositories
{
    public class UserRepository : ICsvRepository<User>
    {
        private readonly string _filePathCsv;

        public UserRepository(string filePathCsv)
        {
            _filePathCsv = filePathCsv;
        }



        public List<User> GetAll()
        {
            using (var reader = new StreamReader(_filePathCsv))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var users = csv.GetRecords<User>().ToList();
                
                return users;
            }
        }

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
