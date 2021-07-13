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
    public class ProdutoQuery
    {
        public AppDb Db { get; }

        public ProdutoQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Producto> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_Producto`, `Producto` FROM `Producto` WHERE `id_Producto` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Producto>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_Producto`, `Producto` FROM `Producto` ORDER BY `id_Producto` asc LIMIT 100;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Producto`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Producto>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Producto>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Producto(Db)
                    {
                        Id_Producto = reader.GetInt32(0),
                        Producto_ = reader.GetString(1),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }


    }
}
