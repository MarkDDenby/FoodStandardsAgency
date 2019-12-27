using AutoMapper;
using FoodStandardsAgency.Rating;
using FoodStandardsAgency.Models;
using ServiceClient.Models;

namespace FoodStandardsAgency.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Authority, AuthorityModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.LocalAuthorityId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.FriendlyName));

            CreateMap<AuthorityRating, RatingModel>()
                .ForMember(dest => dest.Percentage, opts => opts.MapFrom(src => src.Percentage))
                .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.RatingCount))
                .ForMember(dest => dest.RatingImagePath, opts => opts.MapFrom(src => src.RatingImagePath))
                .ForMember(dest => dest.RatingKey, opts => opts.MapFrom(src => src.RatingKey));
         }
    }
}
