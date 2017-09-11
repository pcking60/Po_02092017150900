namespace PostOfiice.DAta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Get_TKBD_By_Time : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "Get_TKBD_By_Time",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String()
                },
                @"select
	                h.Id,
	                h.Account,
	                h.CustomerId,
	                h.Name,
	                convert(datetime,h.TransactionDate) as TransactionDate,
	                convert(decimal(16,2), h.Money) as Money,
	                convert(decimal(16,2), h.Rate) as Rate,
	                h.CreatedBy as ApplicationUser,
	                h.UserId
                from TKBDHistories h
                where
	                h.Status=1 and (h.TransactionDate>=CAST(@fromDate as date) and h.TransactionDate<=cast(@toDate as date))");
        }

        public override void Down()
        {
            DropStoredProcedure("Get_TKBD_By_Time");
        }
    }
}