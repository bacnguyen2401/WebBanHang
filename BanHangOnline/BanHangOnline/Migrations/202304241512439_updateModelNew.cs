namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModelNew : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_New", "Image", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_New", "SeoTitle", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_New", "SeoDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_New", "SeoKeywords", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_New", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tb_New", "SeoDescription", c => c.String());
            AlterColumn("dbo.tb_New", "SeoTitle", c => c.String());
            AlterColumn("dbo.tb_New", "Image", c => c.String());
        }
    }
}
