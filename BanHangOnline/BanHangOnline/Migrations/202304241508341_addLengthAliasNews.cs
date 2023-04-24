namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLengthAliasNews : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_New", "Alias", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_New", "Alias", c => c.String());
        }
    }
}
