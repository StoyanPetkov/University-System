using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository() : base()
        { }

        public StudentRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Student GetById(int id)
        {
            return base.GetById(id);
        }

        public override List<Student> GetAll(Expression<Func<Student, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override void Delete(Student item)
        {
            base.Delete(item);
        }

        public override void Save(Student item)
        {
            base.Save(item);
        }
    }
}