using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>
    {
        public SubjectRepository() : base()
        { }

        public SubjectRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override List<Subject> GetAll(Expression<Func<Subject, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override Subject GetById(int id)
        {
            return base.GetById(id);
        }

        public override void Delete(Subject item)
        {
            base.Delete(item);
        }

        public override void Save(Subject item)
        {
            base.Save(item);
        }
    }
}