using System;

namespace PostOffice.Web.Models
{
    public class TKBDHistoryViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public decimal? Money { get; set; }
        public decimal? Rate { get; set; }
        public string UserId { get; set; }
        public string TransactionUser { get; set; }

        public string CreatedBy
        {
            get; set;
        }

        public DateTime? CreatedDate
        {
            get; set;
        }

        public string MetaDescription
        {
            get; set;
        }

        public string MetaKeyWord
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        }

        public string UpdatedBy
        {
            get; set;
        }

        public DateTime? UpdatedDate
        {
            get; set;
        }
    }
}