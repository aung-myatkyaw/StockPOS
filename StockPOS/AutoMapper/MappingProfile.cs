using AutoMapper;
using StockPOS.Models;
using StockPOS.Models.CreateModels;
using StockPOS.Models.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Productcategory, ProductCategoryCreateModel>().ReverseMap();

        CreateMap<Productcategory, ProductCategoryUpdateModel>().ReverseMap();

        CreateMap<Product, ProductCreateModel>().ReverseMap();

        CreateMap<Product, ProductUpdateModel>().ReverseMap();
        // Add more mappings as needed
    }
}
