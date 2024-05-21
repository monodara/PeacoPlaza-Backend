using AutoMapper;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Service.src.Shared;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<ProductImage, ProductImageReadDto>();
        CreateMap<ProductImageCreateDto, ProductImage>();
        CreateMap<ProductImageUpdateDto, ProductImage>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        // CreateMap<Review, ReviewReadDto>();
        // CreateMap<ReviewCreateDto, Review>();
        // CreateMap<ReviewUpdateDto, Review>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        // CreateMap<Order, OrderReadDto>()
        //     .ForMember(dest => dest.User, opt => opt.MapFrom(s => s.User));
        // CreateMap<OrderCreateDto, Order>()
        //     .ForMember(dest => dest.User, opt => opt.Ignore())
        //     .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(s => s.OrderProducts));
        // CreateMap<OrderUpdateDto, Order>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        // CreateMap<OrderProduct, OrderProductReadDto>();
        // CreateMap<OrderProductCreateDto, OrderProduct>();
    }
}
