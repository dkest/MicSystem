using Mic.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository.IRepositories
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
       where TKey : struct
    {
        IEnumerable<TEntity> GetAll();
        PageEnumerable<TEntity> GetPage(int page, int pageSize);
        TEntity GetById(TKey id);

        void Remove(TEntity item);
        void Save(TEntity item);
    }
}
