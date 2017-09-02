using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDHistoryRepository : IRepository<TKBDHistory> {
        IEnumerable<TKBDHistory> GetAllByUserName(string userName);
    }

    public class TKBDHistoryRepository : RepositoryBase<TKBDHistory>, ITKBDHistoryRepository
    {
        
        public TKBDHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<TKBDHistory> GetAllByUserName(string userName)
        {
           
            var pos = from po in DbContext.PostOffices
                      join u in DbContext.Users
                      on po.ID equals u.POID
                      where u.UserName == userName
                      select po.ID;
            int p = pos.First();

            var listTKBDHistories = (from po in DbContext.PostOffices
                               join u in DbContext.Users
                               on po.ID equals u.POID
                               join h in DbContext.TKBDHistories
                               on u.Id equals h.UserId
                               where po.ID == p
                               select h).AsEnumerable();

            
            return listTKBDHistories;
        }
    }
}