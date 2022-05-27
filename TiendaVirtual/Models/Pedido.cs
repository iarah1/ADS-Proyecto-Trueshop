using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class Cliente
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public decimal TotalPedido { get; set; }
    }

    public class PedidoDetalle
    {
        public int ProductId { get; set; }
        public int Cantidad { get; set; }
    }
}