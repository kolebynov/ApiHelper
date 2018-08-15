using AutoMapper;
using System;

namespace RestApi.Infrastructure
{
    public class MapperProvider
    {
        public IMapper Mapper => _mapper.Value;

        private readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() => new Mapper(new MapperConfiguration(config =>
        {
            config.ForAllMaps((map, expr) => expr.ForAllMembers(opt =>
            {
                if (map.SourceType.GetProperty(opt.DestinationMember.Name) != null)
                {
                    opt.MapFrom(opt.DestinationMember.Name);
                }
                else
                {
                    opt.Ignore();
                }
            }));
        })));
    }
}
