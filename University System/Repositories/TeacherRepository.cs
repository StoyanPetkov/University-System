using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>
    {
        public TeacherRepository() : base()
        { }

        public TeacherRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Teacher GetById(int id)
        {
            return base.GetById(id);
        }

        public override List<Teacher> GetAll(Expression<Func<Teacher, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override void Delete(Teacher item)
        {
            base.Delete(item);
        }

        public override void Save(Teacher item)
        {
            base.Save(item);
        }
    }
}