using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class TitleRepository : BaseRepository<Title>
    {
        public TitleRepository() : base()
        { }

        public TitleRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override List<Title> GetAll(Expression<Func<Title, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override Title GetById(int id)
        {
            return base.GetById(id);
        }

        public override void Delete(Title item)
        {
            base.Delete(item);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}