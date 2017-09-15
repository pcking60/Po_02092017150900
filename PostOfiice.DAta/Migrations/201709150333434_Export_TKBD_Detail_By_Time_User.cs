namespace PostOfiice.DAta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Export_TKBD_Detail_By_Time_User : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "Export_TKBD_Detail_By_Time_User",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String(),
                    currentUserId = p.String()
                },
                @"select
                    t.Month,
                    t.Account,
                    convert(decimal(16,2),t.Amount) as Amount,
                    t.CreatedBy
                from
                    TKBDAmounts t
                    inner join ApplicationUsers u
	                on t.CreatedBy = u.UserName
	                inner join PostOffices p
	                on u.POID = p.ID
	                inner join Districts d
	                on p.DistrictID = d.ID
                where t.Status=1 and u.Id=@currentUserId and (t.CreatedDate>=CAST(@fromDate as date) and t.CreatedDate<=cast(@toDate as date))
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("Export_TKBD_Detail_By_Time_User");
        }
    }
}