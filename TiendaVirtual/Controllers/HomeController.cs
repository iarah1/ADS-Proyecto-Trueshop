using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["CurrentPage"] = 1;

            return View();
        }

        public ActionResult Products(int page, int cat)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            var data = tienda.GetProducts(page, cat);
            Session["CurrentPage"] = page;

            ViewBag.Products = data;

            return View();
        }

        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListProduct(string productIds)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            var data = tienda.GetListProducts(productIds);
            string jsonResult = JsonConvert.SerializeObject(data);
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult ProcesarOrden(Cliente cliente, List<PedidoDetalle> detalle)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            var pedidoId = tienda.ProcesarPedido(cliente, detalle);

            string jsonResult = JsonConvert.SerializeObject(pedidoId);
            return Json(jsonResult);
        }

        public ActionResult About()
        {
            ViewBag.Message = "TRUESHOP";

            return View();
        }
    }
}