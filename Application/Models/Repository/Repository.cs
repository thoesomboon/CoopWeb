using Coop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Coop.Models.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Read();
        T ReadByCreate(T t);
        T ReadById(object id);
        void Create(T t);
        void Update(T t);
        void Delete(T t);
        void Delete(int id);
    }
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CoopWebEntities _context;
        protected readonly DbSet<T> Dbset;
        public Repository(CoopWebEntities context)
        {
            _context = context;
            Dbset = context.Set<T>();
        }
        
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> fitter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = ""
            )
        {
            IQueryable<T> query = Dbset;

            if (fitter != null)
            {
                query = query.Include(fitter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperties);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual IQueryable<T> Read()
        {
            return Dbset;
        }

        public virtual T ReadByCreate(T t)
        {
            if (t == null) throw new ArgumentNullException("t");
            Dbset.Add(t);

            _context.SaveChanges();
            return t;
        }
        public virtual T ReadById(object id)
        {
            return Dbset.Find(id);
        }
        public virtual void Create(T t)
        {
            if (t == null) throw new ArgumentNullException("t");
            Dbset.Add(t);
        }
        public virtual void Update(T t)
        {
            if (t == null) throw new ArgumentNullException("t");
            Dbset.Attach(t);

            _context.Entry(t).State = EntityState.Modified;
        }
        public virtual void Delete(T t)
        {
            if (t == null) throw new ArgumentNullException("t");
            if (_context.Entry(t).State == EntityState.Detached)
            {
                Dbset.Attach(t);
            }
            Dbset.Remove(t);
        }
        public virtual void Delete(int id)
        {
            var t = ReadById(id);
            Delete(t);
        }
    }
}