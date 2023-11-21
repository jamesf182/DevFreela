using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly DevFreelaDbContext _dbContext;
        public SkillRepository(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _dbContext.Skills.ToListAsync();
        }

        public async Task AddSkillFromProject(Project project)
        {            
            var words = project.Description.Split(' ');
            var length = words.Length;

            var skill = $"{project.Id} - {words[length - 1]}";

            await _dbContext.Skills.AddAsync(new Skill(skill));
        }
    }
}
