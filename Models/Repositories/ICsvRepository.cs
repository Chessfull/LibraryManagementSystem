namespace LibraryManagementSystem.Models.Repositories
{
    public interface ICsvRepository<T> where T : class // Interface for my csv repositories, restrict with class and generic <T> 
    {
       List<T> GetAll();
       T GetById(Guid id);
       void Add(T entity);

       void Update(T entity);
       void DeleteById(Guid id);
       
    }
}
