namespace PostOfiice.DAta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Export_TKBD_By_Time_User : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
               "Export_TKBD_By_Time_User",
               p => new
               {
                   fromDate = p.String(),
                   toDate = p.String(),
                   currentUserId = p.String()
               },
               @"select
	                t.Month,
	                convert(int, count(t.Account)) as Quantity,
	                convert(decimal(16,2),
	                sum(t.Amount)) as DTTL,
	                t.CreatedBy
                from TKBDAmounts t
                    inner join ApplicationUsers u
	                on t.CreatedBy = u.UserName
                where t.Status=1 and u.Id=@currentUserId and (t.CreatedDate>=CAST(@fromDate as date) and t.CreatedDate<=cast(@toDate as date))
                group by t.Month, t.CreatedBy");
        }

        public override void Down()
        {
            DropStoredProcedure("Export_TKBD_By_Time_User");
        }
    }
}