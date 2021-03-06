using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Models
{
    public class MovimientosQuery
    {

        public AppDb Db { get; }

        public MovimientosQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<MovimientosModel>> allMovimientosAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call con_allMovimientos";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }


        public async Task<List<MovimientosModel>> allProductosExistenciasAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call con_Productos_Con_Existencias";
            return await ProductoAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<MovimientosModel>> ProductoAsync(DbDataReader reader)
        {
            var posts = new List<MovimientosModel>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new MovimientosModel(Db)
                    {
                        id_producto = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                      
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        

             public async Task<List<MovimientosModel>> allTipoMovimientoAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call con_TipoMovimiento";
            return await TipoMovAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<MovimientosModel>> TipoMovAsync(DbDataReader reader)
        {
            var posts = new List<MovimientosModel>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new MovimientosModel(Db)
                    {
                        id_TipoMovimiento = reader.GetInt32(0),
                        NombreMovimiento = reader.GetString(1),

                    };
                    posts.Add(post);
                }
            }
            return posts;
        }


        private async Task<List<MovimientosModel>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<MovimientosModel>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new MovimientosModel(Db)
                    {
                        id_movimiento = reader.GetInt32(0),
                        fecha = reader.GetDateTime(1),
                        NumeroPedido = reader.GetString(2),
                        Concepto = reader.GetString(3),
                        id_TipoMovimiento = reader.GetInt32(4),
                        NombreMovimiento = reader.GetString(5),
                        Estatus = reader.GetString(6)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        public async Task<List<MovimientosModel>> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call con_OneMovimiento(@id) ";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllDetalisAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result : null;
        }

        public async Task<MovimientosModel> FindMovOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call con_OneMovimiento(@id) ";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllDetalisAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }


        //public async Task<List<MovimientosModel>> FindMovOneAsync_(int id)
        //{
        //    using var cmd = Db.Connection.CreateCommand();
        //    cmd.CommandText = @"call con_OneMovimiento(@id) ";// SELECT `id_Producto`,`Codigo` ,`Producto`, `Activo` FROM `Producto` WHERE `id_Producto` = @id";
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@id",
        //        DbType = DbType.Int32,
        //        Value = id,
        //    });
        //    var result = await ReadAllDetalisAsync_(await cmd.ExecuteReaderAsync());
        //    return result.Count > 0 ? result : null;
        //}


        private async Task<List<MovimientosModel>> ReadAllDetalisAsync(DbDataReader reader)
        {
            var posts = new List<MovimientosModel>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new MovimientosModel(Db)
                    {

                    id_movimiento = reader.GetInt32(0),
                        fecha = reader.GetDateTime(1),
                        NumeroPedido = reader.GetString(2),
                        Concepto = reader.GetString(3),
                        id_movimientoDetalle = reader.GetInt32(4),
                        Cantidad = reader.GetInt32(5),
                        Precio = reader.GetDecimal(6),
                        total = reader.GetDecimal(7),
                        id_producto = reader.GetInt32(8),
                        Codigo = reader.GetString(9),
                        Producto = reader.GetString(10),
                        id_TipoMovimiento = reader.GetInt32(11),
                        NombreMovimiento = reader.GetString(12),
                        Estatus = reader.GetString(13)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        //private async Task<List<MovimientosModel>> ReadAllDetalisAsync_(DbDataReader reader)
        //{
        //    var posts = new List<MovimientosModel>();
        //    using (reader)
        //    {
        //        while (await reader.ReadAsync())
        //        {
        //            var post = new MovimientosModel(Db)
        //            {

        //                id_movimiento = reader.GetInt32(0),
        //                fecha = reader.GetDateTime(1),
        //                NumeroPedido = reader.GetString(2),
        //                Concepto = reader.GetString(3),
        //                id_movimientoDetalle = reader.GetInt32(4),
        //                Cantidad = reader.GetInt32(5),
        //                Precio = reader.GetDecimal(6),
        //                total = reader.GetDecimal(7),
        //                id_producto = reader.GetInt32(8),
        //                Codigo = reader.GetString(9),
        //                Producto = reader.GetString(10),
        //                id_TipoMovimiento = reader.GetInt32(11),
        //                NombreMovimiento = reader.GetString(12),
        //                Estatus = reader.GetString(13)
        //            };
        //            posts.Add(post);
        //        }
        //    }
        //    return posts;
        //}
    }
}
