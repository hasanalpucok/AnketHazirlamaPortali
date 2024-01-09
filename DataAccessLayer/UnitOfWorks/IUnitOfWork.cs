
using DataAccessLayer.Repository.Abstractions;

namespace DataAccessLayer.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
	{
		IRepository<T> GetRepository<T>() where T : class, new();

		Task<int> SaveAsync();
		int Save();
	}
}

