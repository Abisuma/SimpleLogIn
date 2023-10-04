using SimpleLogIn.Data;
using SimpleLogIn.Models;
using SimpleLogIn.Repository.IRepository;

namespace SimpleLogIn.Repository
{
    public class EmailModelRepository :  Repository<EmailModel>, IEmailModelRepository
    {
        private readonly AppDbContext _DbContext;
        public EmailModelRepository(AppDbContext appDb) : base(appDb)
        {
            _DbContext = appDb;
        }

        public void Save()
        {
            _DbContext.SaveChanges();
        }
    }
}
