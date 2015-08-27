using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class BaseRepository<T> where T : BaseEntityWithId, new()
    {
        //Represent the context of the database.
        public DbContext Context { get; set; }

        //Represent a virtual table of the database.
        protected IDbSet<T> DbSet { get; set; }

        //Represent an instance of the class - UnitOfWork.
        public UnitOfWork _UnitOfWork {get;set;}

        //Represents base(empty) constructor of the base repository.
        public BaseRepository()
        {
            this.Context = new UniversitySystemContext();
            this.DbSet = this.Context.Set<T>();
        }

        //Represents a constructor that accepts UnitOfWork class as a parameter.
        //This constructor is called when we have to save data in more than one tables.
        public BaseRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentException("An instance of the UnitOfWork class is null", "UnitOfWork class miss");
            }
            this.Context = unitOfWork.Context;
            this.DbSet = this.Context.Set<T>();
            this._UnitOfWork = unitOfWork;
        }

        public IObjectContextAdapter GetObjectContextAdapter()
        {
            return (IObjectContextAdapter)this.Context;
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual List<T> GetAll(Expression<Func<T, bool>> filter=null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }

        #region CRUD OPERATIONS

        private void Insert(T item)
        {
            this.DbSet.Add(item);
            Context.SaveChanges();
        }

        private void Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Delete(T item)
        {
            DbSet.Remove(item);
            Context.SaveChanges();
        }

        public virtual void Save(T item)
        {
            if (item.Id <= 0)
            {
                Insert(item);
            }
            else
            {
                Update(item);
            }
        }
        #endregion

        public virtual void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }
    }
}