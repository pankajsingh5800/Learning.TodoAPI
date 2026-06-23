using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo.Domain.RepositoryInterface;
using Todo.Infrastructure.Persistence.Entities;


namespace Todo.Infrastructure.Repository
{
    public class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain>
        where TDomain : class
        where TEntity : class
    {
        private readonly TodoAppDbContext _todoAppDbContext;
        private readonly IMapper _mapper;

        public GenericRepository(TodoAppDbContext todoAppDbContext, IMapper mapper)
        {
            this._todoAppDbContext = todoAppDbContext;
            this._mapper = mapper;
        }
        public async Task AddAsync(TDomain domain)
        {
            var entity = _mapper.Map<TEntity>(domain);
            await _todoAppDbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CommitAsync()
        {
            return await _todoAppDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            return await _todoAppDbContext.Set<TEntity>()
                .ProjectTo<TDomain>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDomain?> GetByIdAsync(object id)
        {
            var entity = await _todoAppDbContext.Set<TEntity>().FindAsync(id); // id shoul primary key
            return entity == null ? null : _mapper.Map<TDomain>(entity);
        }
    }
}
