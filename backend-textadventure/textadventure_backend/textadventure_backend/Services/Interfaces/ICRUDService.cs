using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace textadventure_backend.Services.Interfaces
{
    public interface ICRUDService<T>
    {
        Task<T> Create(T entity);
        Task<T> Delete(int id);
        Task<T> Find(int id);
        Task<IEnumerable<T>> Get();
        
        //Implement update still
        //Task<T> Update(int id, T entity);
    }
}
