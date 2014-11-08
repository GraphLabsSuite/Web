using AutoMapper;

namespace GraphLabs.Site.Utils
{
    /// <summary> Маппер </summary>
    public class Mapper
    {
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return AutoMapper.Mapper.CreateMap<TSource, TDestination>();
        }
    }
}
