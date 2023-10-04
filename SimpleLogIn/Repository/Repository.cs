using Microsoft.EntityFrameworkCore;
using SimpleLogIn.Data;
using SimpleLogIn.Repository.IRepository;
using System.Linq.Expressions;

namespace SimpleLogIn.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly AppDbContext db;

        internal DbSet<T> dbSet;

        public Repository(AppDbContext appDb)
        {
            db = appDb; 
           dbSet = db.Set<T>();    // _DbContext.EmailModels == dbSet
        }
        public void AddAUser(T user)
        {
            dbSet.Add(user);    
            
        }

        public IEnumerable<T> GetAllUser()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();  
           
        }

        public T GetAUser(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            return query.FirstOrDefault();

        }
    }
}
