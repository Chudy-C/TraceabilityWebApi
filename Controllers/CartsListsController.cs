using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraceabilityWebApi.Models;
using System.Net.Mail;

namespace TraceabilityWebApi.Controllers
{
    public class CartsListsController : ApiController
    {

        SqlConnection conn;
        private void connection()
        {
            string conString = ConfigurationManager.ConnectionStrings["getConnection"].ToString();
            conn = new SqlConnection(conString);
        }

        [HttpGet]
        public IEnumerable<Przedza> GetPzCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("spGetPzCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();
                cart.TS_PZ = reader["TS_PZ"].ToString();
                cart.PartiaString = reader["Koniec_Partii"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
        [HttpGet]
        public IEnumerable<Przedza> GetSuszarniaCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("spGetPrzedCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();
                cart.TS_SUSZ1 = reader["TS_SUSZ1"].ToString();
                cart.Nazwa_maszyny = reader["Suszenie1"].ToString();
                cart.TS_SUSZ2 = reader["TS_SUSZ2"].ToString();
                cart.Nazwa_maszyny2 = reader["Suszenie2"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
        [HttpGet]
        public IEnumerable<Przedza> GetPoCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("spGetPoCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();
                cart.Wilgotnosc_1 = reader["Wilgotnosc_1"].ToString();
                cart.Wilgotnosc_2 = reader["Wilgotnosc_2"].ToString();
                cart.TS_Kom_PW = reader["TS_KOM1"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
        [HttpGet]
        public IEnumerable<Przedza> GetKomCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("spGetKomCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
        [HttpGet]
        public IEnumerable<Przedza> GetPwCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("spGetPwCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();
                cart.Nazwa_maszyny = reader["MaszynaPW1"].ToString();
                cart.TS_PW = reader["TS_PW1"].ToString();
                cart.TS_PW2 = reader["TS_PW2"].ToString();
                cart.Nazwa_maszyny2 = reader["MaszynaPW2"].ToString();
                cart.PartiaString = reader["Koniec_Partii"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
        [HttpGet]
        public IEnumerable<Przedza> GetEmptyCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("spGetEmptyCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
        [HttpGet]
        public IEnumerable<Carts> GetCartInfo(string Nr_wozka)
        {
            List<Carts> cartsData = new List<Carts>();
            Carts cart = new Carts();
            connection();

            SqlCommand com = new SqlCommand("spGetCartInfo", conn);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                cart.Id_cart = Convert.ToInt32(reader["ID_Wozka"]);
                cart.Id_machine_PZ = Convert.ToInt32(reader["ID_Maszyny_PZ"]);
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.CoilType = reader["Typ_cewki"].ToString();
                cart.CoilColor = reader["Kolor_cewki"].ToString();

                cartsData.Add(cart);
            }
            conn.Close();
            return cartsData;
        }
    }
}
