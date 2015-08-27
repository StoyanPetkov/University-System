using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class CourseSubjectRepository : BaseRepository<CourseSubject>
    {
        public CourseSubjectRepository() : base()
        { }

        public CourseSubjectRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override List<CourseSubject> GetAll(Expression<Func<CourseSubject, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override CourseSubject GetById(int id)
        {
            return base.GetById(id);
        }

        public override void Delete(CourseSubject item)
        {
            base.Delete(item);
        }

        public override void Save(CourseSubject item)
        {
            base.Save(item);
        }
    }
}