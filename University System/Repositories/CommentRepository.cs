using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository() : base()
        { }

        public CommentRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Comment GetById(int id)
        {
            return base.GetById(id);
        }

        public override List<Comment> GetAll(Expression<Func<Comment, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override void Delete(Comment item)
        {
            base.Delete(item);
        }

        public override void Save(Comment item)
        {
            base.Save(item);
        }
    }
}