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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
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
        public decimal Precio { get; set; }
        [Display(Name = "TOTAL")]
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


       
    }
}
