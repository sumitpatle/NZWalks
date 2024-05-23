using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository

    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsyn(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAsync()
        {
            return await dbContext.Walks.Include("DIfficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walks = await dbContext.Walks.Include("DIfficulty").Include("Region").
                FirstOrDefaultAsync(x => x.Id == id);
            if(walks == null)
            {
                return null;
            }
            return walks;
        }

        public async Task<Walk?> UpdatebyIdAsync(Guid id, Walk walkDomainModel)
        {
            var walks = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walks == null)
            {
                return null;
            }

            walks.Name = walkDomainModel.Name;
            walks.Description = walkDomainModel.Description;
            walks.LengthinKM = walkDomainModel.LengthinKM;
            walks.WalkImageUrl = walkDomainModel.WalkImageUrl;

            await dbContext.SaveChangesAsync();

            return walks;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(walk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }
    }
}
