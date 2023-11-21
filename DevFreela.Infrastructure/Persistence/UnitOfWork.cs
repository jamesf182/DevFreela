using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevFreela.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        private readonly DevFreelaDbContext _dbContext;
        public UnitOfWork(IProjectRepository projects, IUserRepository users, ISkillRepository skills, DevFreelaDbContext dbContext)
        {
            Projects = projects;
            Users = users;
            Skills = skills;
            _dbContext = dbContext;
        }

        public IProjectRepository Projects { get; }
        public IUserRepository Users { get; }
        public ISkillRepository Skills { get; }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

    }
}
