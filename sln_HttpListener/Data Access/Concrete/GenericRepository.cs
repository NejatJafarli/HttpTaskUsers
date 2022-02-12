using sln_HttpListener.Data_Model.Abstract;
using sln_HttpListener.Data_Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace sln_HttpListener.Data_Model.Concrete
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected MyDbContext Data=new MyDbContext();

        public void Delete(T entity)
        {
            Data.Set<T>().Remove(entity);
            SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression) => Data.Set<T>().Where(expression);

        public IQueryable<T> GetAll() => from entity in Data.Set<T>() select entity;

        public void Insert(T entity)
        {
            Data.Set<T>().Add(entity);
            SaveChanges();
        }

        public void SaveChanges()=>Data.SaveChanges();

        public void Update(T entity)
        {
            Data.Set<T>().Update(entity);
            SaveChanges ();
        }
    }
}
