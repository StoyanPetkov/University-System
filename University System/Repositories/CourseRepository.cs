using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class CourseRepository : BaseRepository<Course>
    {
        public CourseRepository() : base()
        { }

        public CourseRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override List<Course> GetAll(Expression<Func<Course, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override Course GetById(int id)
        {
            return base.GetById(id);
        }

        public override void Save(Course item)
        {
            base.Save(item);
        }

        public override void Delete(Course item)
        {
            base.Delete(item);
        }
    }
}