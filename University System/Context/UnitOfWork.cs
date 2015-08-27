using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace University_System.Context
{
    public class UnitOfWork : IDisposable
    {
        private UniversitySystemContext context = new UniversitySystemContext();

        private DbContextTransaction transaction = null;

        public DbContext Context { get; private set; }

        public UnitOfWork()
        {
            this.transaction = context.Database.BeginTransaction();
            this.Context = context;
        }

        public void Commit()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
                this.transaction = null;
            }
        }

        public void RollBack()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction = null;
            }
        }

        public void Dispose()
        {
            Commit();
            context.Dispose();
        }
    }
}