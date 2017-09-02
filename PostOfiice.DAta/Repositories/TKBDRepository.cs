using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDRepository : IRepository<TKBDAmount>
    {
    }

    public class TKBDRepository : RepositoryBase<TKBDAmount>, ITKBDRepository
    {
        public TKBDRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override TKBDAmount Add(TKBDAmount entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(TKBDAmount entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}