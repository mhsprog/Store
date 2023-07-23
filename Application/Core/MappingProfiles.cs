using Application.Products;
using AutoMapper;
using Domain;

namespace Application.Core;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateProductDto, Product>()
            .ReverseMap();
        CreateMap<Product, GetProductDto>()
            .ForMember(ds => ds.Creator,
            src => src.MapFrom(x => $"{x.Creator.FirstName} {x.Creator.LastName}"));
    }
}
