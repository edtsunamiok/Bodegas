using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Models
{
    public class Kardex
    {
        [Display(Name = "ID")]
        public int id_Kardex { get; set; }
        [Display(Name = "CODIGO")]
        public string Codigo { get; set; }
        [Display(Name = "PRODUCTO")]
        public string Producto { get; set; }
        [Display(Name = "CANTIDAD")]
        public int Cantidad { get; set; }
        [Display(Name = "PRECIO")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = true)]
        public decimal Precio { get; set; }
        [Display(Name = "TOTAL")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }
        public int id_producto { get; set; }

        internal AppDb Db { get; set; }

        public Kardex() { }
        internal Kardex(AppDb db)
        {
            Db = db;
        }

        public async Task UpdatekardexAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call add_existenciasKardex(@id_Kardex,@Cantidad,@Precio,@id_producto_)";
            SendParamsKardex(cmd);
            SendId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void SendParamsKardex(MySql.Data.MySqlClient.MySqlCommand cmd)
        {            
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Cantidad",
                DbType = DbType.Int32,
                Value = Cantidad
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Precio",
                DbType = DbType.Decimal,
                Value = Precio
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_producto_",
                DbType = DbType.Decimal,
                Value = id_producto
            }); 
        }

        private void SendId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_Kardex",
                DbType = DbType.Int32,
                Value = id_Kardex
            });
        }
    }
}
