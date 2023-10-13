using AutoMapper;
using bookstore.api.DTO;
using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;

namespace bookstore.api.Mapping;

public class BookModelToBookProjectionMappingProfile : Profile
{
    public BookModelToBookProjectionMappingProfile()
    {
        ConfigureMapping();
    }

    private List<PriceProjection> GetPrices(BookModel src)
    {
        return src.Prices
            .Select(priceModel => new PriceProjection
            {
                Id = priceModel.Id,
                Price = priceModel.Price,
                ForDay = priceModel.ForDay,
                ForAllDays = priceModel.ForAllDays
            })
            .ToList();
    }

    public void ConfigureMapping()
    {
        CreateMap<BookModel, BookProjection>()
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.Price = GetPrices(src))
            .MaxDepth(1);

        CreateMap<PriceModel, PriceProjection>()
            .MaxDepth(1);
    }
}