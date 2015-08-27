using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class ArticleRepository : BaseRepository<Article>
    {
        public ArticleRepository() : base()
        { }

        public ArticleRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Article GetById(int id)
        {
            return base.GetById(id);
        }

        public override List<Article> GetAll(Expression<Func<Article, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override void Delete(Article item)
        {
            base.Delete(item);
        }

        public override void Save(Article item)
        {
            base.Save(item);
        }
    }
}