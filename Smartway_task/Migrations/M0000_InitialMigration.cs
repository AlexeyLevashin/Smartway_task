using FluentMigrator;
using FluentMigrator.Oracle;
using FluentMigrator.Postgres;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Smartway_task.Migrations;

[Migration(1)]
public class M0000_InitialMigration : Migration
{
    public override void Up()
    {

        Create.Table("departments")
            .WithColumn("id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable().Unique();

        Create.Table("employees")
            .WithColumn("id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("surname").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable().Unique()
            .WithColumn("companyid").AsInt64().NotNullable()
            .WithColumn("departmentid").AsInt64().NotNullable().ForeignKey("departments", "id");

        Create.Table("passports")
            .WithColumn("id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("type").AsString().NotNullable()
            .WithColumn("number").AsString().NotNullable().Unique()
            .WithColumn("employeeid").AsInt64().ForeignKey("employees", "id");
        
    }

    public override void Down()
    {
        Delete.Table("passports");
        Delete.Table("employees");
        Delete.Table("departments");
    }
}