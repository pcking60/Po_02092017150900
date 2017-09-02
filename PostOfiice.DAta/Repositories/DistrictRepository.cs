using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface IDistrictRepository : IRepository<District>
    {
    }

    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override District Add(District entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override District Delete(District entity)
        {
            return base.Delete(entity);
        }

        public override void Update(District entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}