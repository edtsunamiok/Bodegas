using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Bodegas.Models
{
    public class MovimientosModel
    {
        public int id_movimiento { get; set; }
        //[DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "FECHA")]
        public DateTime fecha { get; set; }
        [Display(Name = "NUMERO MOVIMIENTO")]
        public string NumeroPedido { get; set; }
        [Display(Name = "CONCEPTO")]
        public string Concepto { get; set; }
        public int id_movimientoDetalle { get; set; }
        [Display(Name = "CANTIDAD")]
        public int Cantidad { get; set; }
        [Display(Name = "PRECIO")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = true)]
        public decimal Precio { get; set; }
        [Display(Name = "TOTAL")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = true)]
        public decimal total { get; set; }
        public int id_producto { get; set; }
        [Display(Name = "CODIGO PRODUCTO")]
        public string Codigo { get; set; }
        [Display(Name = "PRODUCTO")]
        public string Producto { get; set; }
        public int id_TipoMovimiento { get; set; }
        [Display(Name = "TIPO MOVIMIENTO")]
        public string NombreMovimiento { get; set; }
        [Display(Name = "Estatus")]
        public string Estatus { get; set; }

        internal AppDb Db { get; set; }

        public MovimientosModel() { }
        internal MovimientosModel(AppDb db)
        {
            Db = db;
        }


        public async Task Update_m_movimientoAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call upd_m_movimiento(@id_movimiento)";// UPDATE `Producto` SET `Producto` = @Producto WHERE `id_Producto` = @id;";
            
            SendId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"call add_NuevoMovimientoE_S(@id_Producto_ ,@id_Tipomovimiento_,@Concepto_,@Cantidad_,@Precio_);"; //@"INSERT INTO `Producto` (`Producto`) VALUES (@Producto);";
            SendParamsInsert(cmd);

                await cmd.ExecuteNonQueryAsync();
            id_producto = (int)cmd.LastInsertedId;
        }

        private void SendParamsInsert(MySql.Data.MySqlClient.MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_Producto_",
                DbType = DbType.Int32,
                Value = id_producto
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_Tipomovimiento_",
                DbType = DbType.Int32,
                Value = id_TipoMovimiento
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Concepto_",
                DbType = DbType.String,
                Value = Concepto
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Cantidad_",
                DbType = DbType.Int32,
                Value = Cantidad
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Precio_",
                DbType = DbType.Decimal,
                Value = Precio
            });
        }
            //public async Task Update_d_movimientoAsync()
            //{
            //    using var cmd = Db.Connection.CreateCommand();
            //    cmd.CommandText = @"call add_existenciasKardex(@id_movimiento,@Cantidad,@Precio,@id_producto_)";// UPDATE `Producto` SET `Producto` = @Producto WHERE `id_Producto` = @id;";
            //    SendParams_d_movimiento(cmd);
            //    SendId(cmd);
            //    await cmd.ExecuteNonQueryAsync();
            //}




            //    cmd.Parameters.Add(new MySqlParameter
            //    {
            //        ParameterName = "@id_producto_",
            //        DbType = DbType.Decimal,
            //        Value = id_producto
            //    });
            //}

            private void SendId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_movimiento",
                DbType = DbType.Int32,
                Value = id_movimiento
            });
        }
    }
}
