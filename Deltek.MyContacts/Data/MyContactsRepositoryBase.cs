using System.Threading;
using System.Threading.Tasks;

namespace Deltek.MyContacts.Data
{
    public abstract class MyContactsRepositoryBase
    {
        protected readonly MyContactsDbContext _dbContext;

        protected MyContactsRepositoryBase(MyContactsDbContext myContactsDbContext)
        {
            _dbContext = myContactsDbContext;
        }

        protected int Save()
        {
            return _dbContext.SaveChanges();
        }

        protected int Save(bool acceptAllChangesOnSuccess)
        {
            return _dbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        protected async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        protected async Task<int> SaveAsync(bool acceptAllChangesOnSuccess)
        {
            return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

        protected async Task<int> SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}