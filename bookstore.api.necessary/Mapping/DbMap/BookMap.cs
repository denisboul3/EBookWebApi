using bookstore.api.Models;
using FluentNHibernate.Mapping;
using System.Reflection.Metadata;

namespace bookstore.api.Mapping.DbMap;

public class BookMap : ClassMap<BookModel>
{
    public BookMap()
    {
        Table("`Books`");

        Id(x => x.Id)
            .Column("Id")
            .Access.Property()
            .Not.Nullable()
            .GeneratedBy.GuidComb()
          ;

        Map(x => x.Name)
            .Column("Name")
            .CustomType("string")
            .Access.Property()
            .Unique()
            .CustomSqlType("varchar(64)")
            .Not.Nullable()
            .Length(512)
          ;

        Map(x => x.Author)
            .Column("Author")
            .CustomType("string")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(64)")
            .Not.Nullable()
            .Length(512)
          ;

        Map(x => x.ImgUri)
            .Column("ImgUri")
            .CustomType("string")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(1024)")
            .Nullable()
            .Length(128)
          ;

        Map(x => x.Genre)
            .Column("Genre")
            .CustomType("string")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(32)")
            .Nullable()
            .Length(128)
          ;
        HasMany<PriceModel>(x => x.Prices)
          .Access.Property()
          .AsSet()
          .Cascade.All()
          .LazyLoad()
          .Generic()
          .KeyColumns.Add("book_id", mapping => mapping.Name("book_id")
            .SqlType("uniqueidentifier")
            .Not.Nullable());
    }
}