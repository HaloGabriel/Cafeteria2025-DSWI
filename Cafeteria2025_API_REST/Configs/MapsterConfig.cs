using Cafeteria2025_API_REST.Models;
using Cafeteria2025_API_REST.Models.Dtos.Request;
using Cafeteria2025_API_REST.Models.Dtos.Response;
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

            TypeAdapterConfig<Producto, GetProductoResponse>
                .NewConfig()
                .Map(dest => dest.id, src => src.IdProducto)
                .Map(dest => dest.name, src => src.Nombre)
                .Map(dest => dest.categoryName, src => src.CateProd != null ?
                                                       src.CateProd.Descripcion :
                                                       string.Empty)
                .Map(dest => dest.price, src => src.PrecioBase)
                .Map(dest => dest.stock, src => src.Stock);

            TypeAdapterConfig<Producto, GetProductoResponse2>
                .NewConfig()
                .Map(dest => dest.id, src => src.IdProducto)
                .Map(dest => dest.name, src => src.Nombre)
                .Map(dest => dest.description, src => src.Descripcion)
                .Map(dest => dest.sizeName, src => src.TmnProd != null ?
                                                   src.TmnProd.Nombre :
                                                   string.Empty)
                .Map(dest => dest.categoryName, src => src.CateProd != null ?
                                                       src.CateProd.Descripcion :
                                                       string.Empty)
                .Map(dest => dest.price, src => src.PrecioBase)
                .Map(dest => dest.stock, src => src.Stock)
                .Map(dest => dest.imageUrl, src => src.ImagenUrl)
                .Map(dest => dest.customizable, src => src.EsPersonalizable)
                .Map(dest => dest.enabled, src => src.Activo)
                .Map(dest => dest.registerDate, src => src.FechaRegistro);

            TypeAdapterConfig<Producto, GetProductoResponse3>
                .NewConfig()
                .Map(dest => dest.id, src => src.IdProducto)
                .Map(dest => dest.name, src => src.Nombre)
                .Map(dest => dest.description, src => src.Descripcion)
                .Map(dest => dest.price, src => src.PrecioBase)
                .Map(dest => dest.idSize, src => src.TmnProd != null ?
                                                 src.TmnProd.IdTamano : 0)
                .Map(dest => dest.stock, src => src.Stock)
                .Map(dest => dest.idCategory, src => src.CateProd != null ?
                                                 src.CateProd.IdCategoria : 0)
                .Map(dest => dest.imageUrl, src => src.ImagenUrl)
                .Map(dest => dest.customizable, src => src.EsPersonalizable)
                .Map(dest => dest.enabled, src => src.Activo);

            TypeAdapterConfig<PostProductoRequest, Producto>
                .NewConfig()
                .Map(dest => dest.Nombre, src => src.name)
                .Map(dest => dest.Descripcion, src => src.description)
                .Map(dest => dest.PrecioBase, src => src.price)
                .Map(dest => dest.Stock, src => src.stock)
                .Map(dest => dest.ImagenUrl, src => src.imageUrl)
                .Map(dest => dest.EsPersonalizable, src => src.customizable)
                .AfterMapping((src, dest) =>
                {
                    dest.CateProd = new Categoria { IdCategoria = src.idCategory };

                    if (src.idSize.HasValue)
                        dest.TmnProd = new Tamano { IdTamano = (byte)src.idSize.Value };
                    else
                        dest.TmnProd = null;
                });


            TypeAdapterConfig<PutProductoRequest, Producto>
                .NewConfig()
                .Map(dest => dest.IdProducto, src => src.id)
                .Map(dest => dest.Nombre, src => src.name)
                .Map(dest => dest.Descripcion, src => src.description)
                .Map(dest => dest.PrecioBase, src => src.price)
                .Map(dest => dest.Stock, src => src.stock)
                .Map(dest => dest.ImagenUrl, src => src.imageUrl)
                .Map(dest => dest.EsPersonalizable, src => src.customizable)
                .Map(dest => dest.Activo, src => src.enabled)
                .Map(dest => dest.UsuarioActualizacion, src => src.userUpdate)
                .AfterMapping((src, dest) =>
                {
                    dest.CateProd = new Categoria { IdCategoria = src.idCategory };

                    if (src.idSize.HasValue)
                        dest.TmnProd = new Tamano { IdTamano = (byte)src.idSize.Value };
                    else
                        dest.TmnProd = null;
                });

        }
    }
}
