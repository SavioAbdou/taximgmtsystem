namespace TaxiManagementSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lebanons",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        City = c.String(nullable: false, maxLength: 50),
                        District = c.String(nullable: false, maxLength: 40),
                        Area = c.String(nullable: false, maxLength: 20),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        LastModificationDate = c.DateTime(),
                        LastModificationUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ride",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Distance = c.Double(nullable: false),
                        BookingTime = c.DateTime(nullable: false),
                        RideStatus = c.Int(nullable: false),
                        IsUserRated = c.Boolean(nullable: false),
                        IsDriverRated = c.Boolean(nullable: false),
                        EstimatedPrice = c.Decimal(nullable: false, storeType: "money"),
                        UserId = c.String(maxLength: 128),
                        TaxiId = c.Guid(nullable: false),
                        SourceId = c.Guid(nullable: false),
                        DestinationId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        LastModificationDate = c.DateTime(),
                        LastModificationUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lebanons", t => t.DestinationId)
                .ForeignKey("dbo.Taxis", t => t.TaxiId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Lebanons", t => t.SourceId)
                .Index(t => t.UserId)
                .Index(t => t.TaxiId)
                .Index(t => t.SourceId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.Int(nullable: false),
                        DriverId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        RideId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        LastModificationDate = c.DateTime(),
                        LastModificationUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DriverId)
                .ForeignKey("dbo.Ride", t => t.RideId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.DriverId)
                .Index(t => t.UserId)
                .Index(t => t.RideId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        ProfilePic = c.Binary(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        DriverRating = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Taxis",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        TaxiStatus = c.Int(nullable: false),
                        DriverId = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreateUser = c.String(nullable: false, maxLength: 50),
                        LastModificationDate = c.DateTime(),
                        LastModificationUser = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DriverId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ride", "SourceId", "dbo.Lebanons");
            DropForeignKey("dbo.Ride", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "RideId", "dbo.Ride");
            DropForeignKey("dbo.Ride", "TaxiId", "dbo.Taxis");
            DropForeignKey("dbo.Taxis", "DriverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "DriverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ride", "DestinationId", "dbo.Lebanons");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Taxis", new[] { "DriverId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Ratings", new[] { "RideId" });
            DropIndex("dbo.Ratings", new[] { "UserId" });
            DropIndex("dbo.Ratings", new[] { "DriverId" });
            DropIndex("dbo.Ride", new[] { "DestinationId" });
            DropIndex("dbo.Ride", new[] { "SourceId" });
            DropIndex("dbo.Ride", new[] { "TaxiId" });
            DropIndex("dbo.Ride", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Taxis");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Ratings");
            DropTable("dbo.Ride");
            DropTable("dbo.Lebanons");
        }
    }
}
