using System.Data;
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
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable().Unique();

        Create.Table("employees")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("surname").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable().Unique()
            .WithColumn("companyid").AsInt64().NotNullable()
            .WithColumn("departmentid").AsInt64().NotNullable();

        Create.Table("passports")
            .WithColumn("id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("type").AsString().NotNullable()
            .WithColumn("number").AsString().NotNullable().Unique()
            .WithColumn("employeeid").AsInt64().Unique().NotNullable();
        
        Create.ForeignKey("FK_Passports_Employees")
            .FromTable("passports").ForeignColumn("employeeid")
            .ToTable("employees").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey("FK_Employees_Departments")
            .FromTable("employees").ForeignColumn("departmentid")
            .ToTable("departments").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("passports");
        Delete.Table("employees");
        Delete.Table("departments");
    }
}