using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class GradeRepository : BaseRepository<Grade>
    {
        public GradeRepository() : base()
        { }

        public GradeRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override List<Grade> GetAll(Expression<Func<Grade, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override Grade GetById(int id)
        {
            return base.GetById(id);
        }

        public override void Delete(Grade item)
        {
            base.Delete(item);
        }

        public override void Save(Grade item)
        {
            base.Save(item);
        }
    }
}