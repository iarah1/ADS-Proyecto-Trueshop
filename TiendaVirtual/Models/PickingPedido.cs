using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class PickingPedido
    {
        public int PedidoId { get; set; }
        public int TotalProductos { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }

    }

    public class PickingPedidoDetalle
    {
        public int PedidoId { get; set; }
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public string UpcCode { get; set; }
        public string ProductName { get; set; }
        public string LocationCode { get; set; }
        public int Cantidad { get; set; }

    }

    public class PickingUser
    {
        public int EmpleadoId { get; set; }
        public string Empleado { get; set; }
    }
}