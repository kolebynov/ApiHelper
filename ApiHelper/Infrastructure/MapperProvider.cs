using AutoMapper;
using Microsoft.Extensions.Options;
using RestApi.Configuration;
using System;

namespace RestApi.Infrastructure
{
    public class MapperProvider
    {
        public IMapper Mapper => _mapper.Value;

        private readonly Lazy<IMapper> _mapper;

        public MapperProvider(IOptions<RestApiOptions> options)
        {
            var externalMapperConfig = options.Value.MapperConfiguration;

            _mapper = new Lazy<IMapper>(() => new Mapper(new MapperConfiguration(config =>
            {
                config.ForAllMaps((map, expr) => expr.ForAllOtherMembers(opt =>
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

                externalMapperConfig?.Invoke(config);
            })));
        }
    }
}
