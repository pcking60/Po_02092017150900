using PostOffice.Common.ViewModels.ExportModel;
using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDRepository : IRepository<TKBDAmount>
    {
        IEnumerable<TKBD_Export_Template> Export_By_Time(string fromDate, string toDate);
        IEnumerable<TKBD_Export_Template> Export_By_Time_User(string fromDate, string toDate, string currentUserId);
        IEnumerable<TKBD_Export_Template> Export_By_Time_District(string fromDate, string toDate, int districtId);
        IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId);
        IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string userId);

        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time(string fromDate, string toDate);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_User(string fromDate, string toDate, string currentUserId);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District(string fromDate, string toDate, int districtId);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string userId);
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

        public IEnumerable<TKBD_Export_Template> Export_By_Time(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time @fromDate,@toDate", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_District(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_District @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_District_PO @fromDate,@toDate,@districtId,@poId", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_District_PO_User @fromDate,@toDate,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_User(string fromDate, string toDate, string currentUserId)
        {
            var parameters = new SqlParameter[]
             {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@currentUserId", currentUserId)
             };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_User @fromDate,@toDate,@currentUserId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time @fromDate,@toDate", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_District @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_District_Po @fromDate,@toDate,@districtId,@poId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[]
           {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
           };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_District_Po_User @fromDate,@toDate,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_User(string fromDate, string toDate, string currentUserId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@currentUserId", currentUserId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_User @fromDate,@toDate,@currentUserId", parameters);
        }

        public override void Update(TKBDAmount entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}