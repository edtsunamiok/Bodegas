using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Bodegas.Models
{
    public class KardexQuery
    {

        public AppDb Db { get; }

        public KardexQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Kardex>> allKardexAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call allKardex";
                return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Kardex>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Kardex>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Kardex(Db)
                    {
                        id_Kardex = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Producto = reader.GetString(2),
                        Cantidad = reader.GetInt32(3),
                        Precio = reader.GetDecimal(4),
                        Total = reader.GetDecimal(5),
                        id_producto = reader.GetInt32(6)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        public async Task<Kardex> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call con_OneKardex(@id) ";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }


    }
}
