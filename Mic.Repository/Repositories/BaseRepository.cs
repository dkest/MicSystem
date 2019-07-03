//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Mic.Repository.Repositories
//{
//    /// <summary>
//    /// 泛型仓储，实现泛型仓储接口
//    /// </summary>
//    /// <typeparam name="TEntity"></typeparam>
//    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
//        where TEntity : class, IEntity<TKey>
//        where TKey : struct
//    {
//        public virtual IEnumerable<TEntity> GetAll()
//        {
//            return NPocoDatabase.Instance.Fetch<TEntity>();
//        }

//        public virtual PageEnumerable<TEntity> GetPage(int page, int pageSize)
//        {
//            return CreatePageEnumerable(NPocoDatabase.Instance.Page<TEntity>(page, pageSize, "ORDER BY Id DESC"));
//        }

//        public virtual TEntity GetById(TKey id)
//        {
//            return NPocoDatabase.Instance.FirstOrDefault<TEntity>("WHERE Id = @0", id);
//        }

//        public virtual void Save(TEntity item)
//        {
//            if (NPocoDatabase.Instance.Exists<TEntity>((object)item.Id)) NPocoDatabase.Instance.Update(item);
//            else NPocoDatabase.Instance.Insert(item);
//        }

//        public virtual void Remove(TEntity item)
//        {
//            NPocoDatabase.Instance.Delete(item);
//        }

//        protected static PageEnumerable<TEntity> CreatePageEnumerable(Page<TEntity> page)
//        {
//            return new PageEnumerable<TEntity>
//            {
//                CurrentPage = (int)page.CurrentPage,
//                Items = page.Items,
//                ItemsPerPage = (int)page.ItemsPerPage,
//                TotalItems = (int)page.TotalItems,
//                TotalPages = (int)page.TotalPages
//            };
//        }
//    }
//}
