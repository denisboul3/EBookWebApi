using bookstore.api.Models;
using FluentNHibernate.Mapping;

namespace bookstore.api.Mapping.DbMap;

public class PriceMap : ClassMap<PriceModel>
{
  public PriceMap()
  {
    Table("`Prices`");

    Id(x => x.Id)
      .Column("id")
      .CustomType("Guid")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .GuidComb()
      ;

    Map(x => x.Price)
      .Column("price")
      .CustomType("int")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("int")
      .Not.Nullable()
      .Length(6)
      ;

    Map(x => x.ForDay)
        .Column("forday")
        .CustomType("int")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("int")
        .Not.Nullable()
        .Length(1)
        ;
    Map(x => x.ForAllDays)
        .Column("foralldays")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("bit")
        .Not.Nullable()
        .Length(6)
        ;

        References(x => x.Book)
      .Class<BookModel>()
      .Access.Property()
      .Cascade.All()
      .LazyLoad()
      .Columns("book_id");
    }
}