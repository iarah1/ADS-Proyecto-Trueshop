using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class PickingController : Controller
    {
        // GET: Picking

        public ActionResult PickingLogin()
        {
            
            return View();
        }


        [HttpPost]
        public ActionResult PickingLogin(string userName, string password)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            PickingUser empleado = tienda.PickerLogin(userName, password);
            
            if (empleado.EmpleadoId > 0)
            {
                Session["User"] = userName;
                Session["EmpleadoId"] = empleado.EmpleadoId;
                Session["Empleado"] = empleado.Empleado;

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("PickingLogin");
            }            
        }
        public ActionResult Index()
        {
            string user = Convert.ToString(Session["User"]);
            int empleadoId = Convert.ToInt32(Session["EmpleadoId"]);

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("PickingLogin");
            }

            TiendaDBHandle tienda = new TiendaDBHandle();
            var pedidos = tienda.GetListPedidosPick(empleadoId);

            ViewBag.Pedidos = pedidos;
            return View();
        }

        public ActionResult Detalle(int PedidoId)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            var pedido = tienda.GetDetallePedidosPick(PedidoId);

            ViewBag.DetallePedido = pedido;
            return View();
        }

        [HttpPost]
        public JsonResult GetDetallePedido(int PedidoId)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            var pedido = tienda.GetDetallePedidosPick(PedidoId);

            string jsonResult = JsonConvert.SerializeObject(pedido);
            return Json(jsonResult);
        }

    }
}