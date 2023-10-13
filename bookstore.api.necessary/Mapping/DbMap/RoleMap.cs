using bookstore.api.Models;
using FluentNHibernate.Mapping;

namespace bookstore.api.Mapping.DbMap;

public class RoleMap : ClassMap<RoleModel>
{
    public RoleMap()
    {
        this.Table("`Roles`");

        this.Id(x => x.Id)
            .Column("id")
            .Access.Property()
            //.Not.Nullable()
            .GeneratedBy.GuidComb()
          ;

        this.Map(x => x.Name)
          .Column("name")
          .CustomType("string")
          .Unique()
          .Access.Property()
          .CustomSqlType("varchar(128)")
          //.Not.Nullable()
          .Length(128);

        this.Map(x => x.CreatedDate)
          .Column("created_date")
          .Access.Property()
          .Generated.Never()
          .Not.Nullable()
          ;

        this.Map(x => x.CreatedBy)
          .Column("created_by")
          .CustomType("Guid")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;
    }
}