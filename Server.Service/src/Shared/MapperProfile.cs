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

        CreateMap<Category, CategoryReadDTO>();
        CreateMap<CategoryCreateDTO, Category>();
        CreateMap<CategoryUpdateDTO, Category>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<Product, ProductReadDTO>();
        CreateMap<ProductCreateDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<ProductImage, ProductImageReadDTO>();
        CreateMap<ProductImageCreateDTO, ProductImage>();
        CreateMap<ProductImageUpdateDTO, ProductImage>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        // CreateMap<Review, ReviewReadDTO>();
        // CreateMap<ReviewCreateDTO, Review>();
        // CreateMap<ReviewUpdateDTO, Review>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        // CreateMap<Order, OrderReadDTO>()
        //     .ForMember(dest => dest.User, opt => opt.MapFrom(s => s.User));
        // CreateMap<OrderCreateDTO, Order>()
        //     .ForMember(dest => dest.User, opt => opt.Ignore())
        //     .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(s => s.OrderProducts));
        // CreateMap<OrderUpdateDTO, Order>().ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        // CreateMap<OrderProduct, OrderProductReadDTO>();
        // CreateMap<OrderProductCreateDTO, OrderProduct>();
    }
}
