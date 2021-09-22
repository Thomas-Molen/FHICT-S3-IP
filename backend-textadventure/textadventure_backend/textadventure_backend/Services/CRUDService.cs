using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Context;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Services
{
    public class CRUDService<T> : ICRUDService<T> where T : DefaultModel
    {
        private readonly IContextFactory contextFactory;

        public CRUDService(IContextFactory _contextfactory)
        {
            contextFactory = _contextfactory;
        }

        public virtual async Task<T> Create(T entity)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var created = await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return created.Entity;
            }
        }

        public virtual async Task<T> Delete(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync(model => model.id == id);
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
        }

        public virtual async Task<T> Find(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync(model => model.id == id);
                return entity;
            }
        }

        public virtual async Task<IEnumerable<T>> Get()
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var entity = await _context.Set<T>().ToListAsync();
                return entity;
            }
        }
    }
}
