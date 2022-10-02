using FFPT_Project.Data.Repository;
using System;
using System.Threading.Tasks;

namespace FFPT_Project.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<T> Repository<T>()
          where T : class;

        int Commit();

        Task<int> CommitAsync();
    }
}