using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class TiendaDBHandle
    {
        private SqlConnection con;
        private string conex;
        private void connection()
        {
            
            string constring = @"Server= localhost; Database= PickingADS;Integrated Security=SSPI;";
            conex = constring;
            con = new SqlConnection(constring);
        }

        public PickingUser PickerLogin(string username, string password)
        {
            PickingUser pickingUser = new PickingUser();
            connection();
            SqlCommand cmd = new SqlCommand("spPickerLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    pickingUser.EmpleadoId = dr.GetInt32(0);
                    pickingUser.Empleado = dr.GetString(1);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            //close data reader
            dr.Close();

            //close connection
            con.Close();

            return pickingUser;
        }

        public int GetTotalPages()
        {
            int TotalPages = 0;
            connection();
            SqlCommand cmd = new SqlCommand("spGetTotalPage", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    TotalPages = dr.GetInt32(0);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            //close data reader
            dr.Close();

            //close connection
            con.Close();

            return TotalPages;
        }

        public List<Product> GetProducts(int pageIndex, int categoryId)
        {
            connection();
            List<Product> products = new List<Product>();

            SqlCommand cmd = new SqlCommand("spGetProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                products.Add(
                    new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ItemCode = Convert.ToString(dr["ItemCode"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        ImageUrl = Convert.ToString(dr["ImageUrl"])
                    });
            }

            return products;
        }

        public List<Product> GetListProducts(string productIds)
        {
            connection();
            List<Product> products = new List<Product>();

            SqlCommand cmd = new SqlCommand("spGetListProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productIds", productIds);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                products.Add(
                    new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ItemCode = Convert.ToString(dr["ItemCode"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        ImageUrl = Convert.ToString(dr["ImageUrl"])
                    });
            }

            return products;
        }

        public int ProcesarPedido(Cliente cliente, List<PedidoDetalle> detalle)
        {
            int PedidoId = InsertPedido(cliente);

            if(PedidoId > 0)
            {
                foreach(var d in detalle)
                {
                    int DetalleId = InsertDetallePedido(PedidoId, d);
                }
            }

            return PedidoId;
        }

        private int InsertPedido(Cliente cliente)
        {
            string _cliente = cliente.Nombres + " " + cliente.Apellidos;

            int PedidoId = 0;
            connection();
            SqlCommand cmd = new SqlCommand("spCrearPedido", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cliente", _cliente);
            cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("@Total", cliente.TotalPedido);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PedidoId = dr.GetInt32(0);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            //close data reader
            dr.Close();

            //close connection
            con.Close();

            return PedidoId;
        }

        private int InsertDetallePedido(int PedidoId, PedidoDetalle detalle)
        {
            int DetalleId = 0;
            connection();
            SqlCommand cmd = new SqlCommand("spAddDetallePedido", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PedidoId", PedidoId);
            cmd.Parameters.AddWithValue("@ProductId", detalle.ProductId);
            cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    DetalleId = dr.GetInt32(0);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            //close data reader
            dr.Close();

            //close connection
            con.Close();

            return DetalleId;
        }


        public List<PickingPedido> GetListPedidosPick(int empleadoId)
        {
            connection();
            List<PickingPedido> listPedidos = new List<PickingPedido>();

            SqlCommand cmd = new SqlCommand("spGetPickPedidos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empleadoId", empleadoId);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                listPedidos.Add(
                    new PickingPedido
                    {
                        PedidoId = Convert.ToInt32(dr["PedidoId"]),
                        TotalProductos = Convert.ToInt32(dr["TotalProductos"]),
                        Fecha = Convert.ToString(dr["Fecha"]),
                        Estado = Convert.ToString(dr["Estado"])
                    });
            }

            return listPedidos;
        }

        public List<PickingPedidoDetalle> GetDetallePedidosPick(int PedidoId)
        {
            connection();
            List<PickingPedidoDetalle> listDetallePedido = new List<PickingPedidoDetalle>();

            SqlCommand cmd = new SqlCommand("spGetPickPedidoDetalle", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PedidoId", PedidoId);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                listDetallePedido.Add(
                    new PickingPedidoDetalle
                    {
                        PedidoId = Convert.ToInt32(dr["PedidoId"]),
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ItemCode = Convert.ToString(dr["ItemCode"]),
                        UpcCode = Convert.ToString(dr["UpcCode"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        LocationCode = Convert.ToString(dr["LocationCode"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"])
                    });
            }

            return listDetallePedido;
        }
    }
}

