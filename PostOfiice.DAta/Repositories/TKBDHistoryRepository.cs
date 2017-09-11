using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDHistoryRepository : IRepository<TKBDHistory>
    {
        IEnumerable<TKBDHistory> GetAllByUserName(string userName);

        IEnumerable<TKBDHistory> Get_By_Time(string fromDate, string toDate);

        IEnumerable<TKBDHistory> Get_By_Time_District(string fromDate, string toDate, int districtId);

        IEnumerable<TKBDHistory> Get_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId);

        IEnumerable<TKBDHistory> Get_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string selectedUser);
    }

    public class TKBDHistoryRepository : RepositoryBase<TKBDHistory>, ITKBDHistoryRepository
    {
        public TKBDHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<TKBDHistory> Get_By_Time(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<TKBDHistory>("Get_TKBD_By_Time @fromDate,@toDate", parameters);
        }

        public IEnumerable<TKBDHistory> Get_By_Time_District(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<TKBDHistory>("Get_TKBD_By_Time_District @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<TKBDHistory> Get_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<TKBDHistory>("Get_TKBD_By_Time_District @fromDate,@toDate,@districtId,@poId", parameters);
        }

        public IEnumerable<TKBDHistory> Get_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string selectedUser)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@selectedUser", selectedUser)
            };
            return DbContext.Database.SqlQuery<TKBDHistory>("Get_TKBD_By_Time_District @fromDate,@toDate,@districtId,@poId,@selectedUser", parameters);
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