using Cafeteria2025_API_REST.Models;
using Cafeteria2025_API_REST.Models.Dtos;
using Mapster;

namespace Cafeteria2025_API_REST.Configs
{
    public class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Categoria, GetCategoriaResponse>
                .NewConfig()
                .Map(dest => dest.id, src => src.IdCategoria)
                .Map(dest => dest.name, src => src.Descripcion)
                .Map(dest => dest.enabled, src => src.Activo);

            TypeAdapterConfig<Categoria, GetCategoriaResponse2>
                .NewConfig()
                .Map(dest => dest.id, src => src.IdCategoria)
                .Map(dest => dest.name, src => src.Descripcion);

            TypeAdapterConfig<PostCategoriaRequest, Categoria>
                .NewConfig()
                .Map(dest => dest.Descripcion, src => src.name);

            TypeAdapterConfig<PutCategoriaRequest, Categoria>
                .NewConfig()
                .Map(dest => dest.IdCategoria, src => src.id)
                .Map(dest => dest.Descripcion, src => src.name)
                .Map(dest => dest.Activo, src => src.enabled);
        }
    }
}
