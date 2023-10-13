using bookstore.api.Models;
using FluentNHibernate.Mapping;

namespace bookstore.api.Mapping.DbMap;

public class UserMap : ClassMap<UserModel>
{
    public UserMap()
    {
        Table("`Users`");

        Id(x => x.Id)
            .Column("Id")
            .Access.Property()
            .Not.Nullable()
            .GeneratedBy.GuidComb()
          ;

        Map(x => x.Login)
            .Column("Login")
            .CustomType("string")
            .Access.Property()
            .Unique()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)//.Generated.Never().
          ;

        Map(x => x.Password)
            .Column("Password")
            .CustomType("string")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)
          ;

        Map(x => x.Email)
            .Column("Email")
            .CustomType("string")
            .Access.Property()
            .Generated.Never()
            .Unique()
            .CustomSqlType("varchar(128)")
            .Not.Nullable()
            .Length(128)
          ;

        Map(x => x.USDTBalance)
          .Column("USDTBalance")
          .CustomType("int")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("int")
          .Not.Nullable()
          .Length(128)
          ;

        Map(x => x.WalletAddress)
          .Column("WalletAddress")
          .CustomType("string")
          .Access.Property()
          .Generated.Never()
          //.Unique()
          .CustomSqlType("varchar(512)")
          .Not.Nullable()
          .Length(128)
          ;

        References(x => x.Role).Cascade.All().Not.Nullable();
    }
}