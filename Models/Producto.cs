using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Models
{
    public class Producto
    {
        public int Id_Producto { get; set; }
        public string Producto_ { get; set; }
        public string Codigo { get; set; }
        public bool Activo { get; set; }

        internal AppDb Db { get; set; }

        public Producto() { }
        internal Producto(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call add_Nuevoproducto(@Producto,@Activo);"; //@"INSERT INTO `Producto` (`Producto`) VALUES (@Producto);";
            SendParamsProducto(cmd);

            await cmd.ExecuteNonQueryAsync();
            Id_Producto = (int)cmd.LastInsertedId;
        }


        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Producto` SET `Producto` = @Producto WHERE `id_Producto` = @id;";
            SendParamsProducto(cmd);
            SendId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Producto` WHERE `id_Producto` = @id;";
            SendId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void SendParamsProducto(MySql.Data.MySqlClient.MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Producto",
                DbType = DbType.String,
                Value = Producto_,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Activo",
                DbType = DbType.Boolean,
                Value = Producto_,
            });
        }
        
        private void SendId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id_Producto,
            });
        }
    }
}
