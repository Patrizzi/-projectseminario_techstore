using System;
using System.Collections.Generic;
using TechStore.DAL;
using TechStore.Entidades;

namespace TechStore.BLL
{
    public class ProductoBLL
    {
        private readonly ProductoDAL _productoDAL = new ProductoDAL();

        /// <summary>
        /// Obtiene el catálogo de productos con sus stocks actualizados.
        /// </summary>
        public List<Producto> ListarProductos(bool soloActivos = true)
        {
            return _productoDAL.Listar(soloActivos);
        }

        /// <summary>
        /// Obtiene la información detallada de un producto por su identificador.
        /// </summary>
        public Producto ObtenerPorId(int idProducto)
        {
            if (idProducto <= 0)
                throw new ArgumentException("El identificador del producto no es válido.", nameof(idProducto));

            return _productoDAL.ObtenerPorId(idProducto);
        }
    }
}

