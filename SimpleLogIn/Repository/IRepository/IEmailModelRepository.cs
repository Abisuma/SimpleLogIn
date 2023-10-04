using Microsoft.EntityFrameworkCore.Diagnostics;
using SimpleLogIn.Models;

namespace SimpleLogIn.Repository.IRepository
{
    public interface IEmailModelRepository:IRepository<EmailModel>
    {
        void Save();
    }
}
