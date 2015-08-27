using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using University_System.Context;
using University_System.Entities;

namespace University_System.Repositories
{
    public class AdministratorRepository : BaseRepository<Administrator>
    {
        public AdministratorRepository() : base()
        { }

        public AdministratorRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Administrator GetById(int id)
        {
            return base.GetById(id);
        }

        public override List<Administrator> GetAll(Expression<Func<Administrator, bool>> filter = null)
        {
            return base.GetAll(filter);
        }

        public override void Save(Administrator item)
        {
            base.Save(item);
        }

        public override void Delete(Administrator item)
        {
            base.Delete(item);
        }
    }
}