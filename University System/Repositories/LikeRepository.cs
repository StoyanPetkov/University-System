using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class LikeRepository : BaseRepository<Like>
    {
        public LikeRepository() : base()
        { }

        public LikeRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Like GetById(int id)
        {
            return base.GetById(id);
        }

        public override List<Like> GetAll(Expression<Func<Like, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override void Save(Like item)
        {
            base.Save(item);
        }

        public override void Delete(Like item)
        {
            base.Delete(item);
        }
    }
}