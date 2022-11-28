using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace StockPOS.Repository
{
    public interface IRepositoryBase<T>
    {
        EntityEntry EntryObject(T objT);
        IEnumerable<T> FindAll();
        Task<IEnumerable<T>> FindAllAsync();
        T FindByCompositeID(long ID1, long ID2);
        T FindByCompositeID(int ID1, int ID2);
        Task<T> FindByCompositeIDAsync(long ID1, long ID2);
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        bool AnyByCondition(Expression<Func<T, bool>> expression);
        Task<bool> AnyByConditionAsync(Expression<Func<T, bool>> expression);
        T FindByID(long ID);
        T FindByID(int ID);
        Task<T> FindByIDAsync(long ID);
        Task<T> FindByIDAsync(int ID);
        void Create(dynamic entity, bool flush = true);
        void Update(dynamic entity, bool flush = true);
        void Delete(dynamic entity, bool flush = true);
        void Save();
        Task<bool> IsExist(string ID);
        Task<bool> IsExist(int ID);
        Task CreateAsync(dynamic entity, bool flush = true);
        Task UpdateAsync(dynamic entity, bool flush = true);
        Task DeleteAsync(dynamic entity, bool flush = true);
        Task SaveAsync();
        Task<T> FindByIDAsync(string ID);
    }
}
