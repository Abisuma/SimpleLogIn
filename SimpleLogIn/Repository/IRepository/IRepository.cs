using System.Linq.Expressions;

namespace SimpleLogIn.Repository.IRepository
{
    public interface IRepository<T>where T: class
    {
        IEnumerable<T> GetAllUser();
        T GetAUser(Expression<Func<T, bool>> filter);
        void AddAUser(T user);  
    }
}
