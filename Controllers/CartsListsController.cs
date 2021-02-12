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

            SqlCommand com = new SqlCommand("sp1GetPzCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nazwa_maszyny = reader["MaszynaPZ"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();
                cart.TS_PZ = reader["TS_PZ"].ToString();
                cart.Numer_partii= reader["Numer_partii"].ToString();
                cart.Koniec_partii = reader["Koniec_Partii"].ToString();

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

            SqlCommand com = new SqlCommand("sp1GetSuszarniaCarts", conn);

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
        public IEnumerable<Przedza> GetKomoraCarts()
        {
            List<Przedza> cartsData = new List<Przedza>();

            connection();

            SqlCommand com = new SqlCommand("sp1GetKomoraCarts", conn);

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
                cart.Numer_partii = reader["Numer_partii"].ToString();
                cart.Koniec_partii = reader["Koniec_partii"].ToString();
                cart.TS_KOM1 = reader["TS_KOM1"].ToString();

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

            SqlCommand com = new SqlCommand("sp1GetPwCarts", conn);

            com.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza cart = new Przedza();

                cart.Nr_wozka = reader["Nr_wozka"].ToString();
                cart.Nazwa_maszyny = reader["MaszynaPW1"].ToString();
                cart.Numer_partii = reader["Numer_partii"].ToString();
                cart.Nm = reader["Nm"].ToString();
                cart.Material = reader["Material"].ToString();
                cart.Typ_cewki = reader["Typ_cewki"].ToString();
                cart.Kolor_cewki = reader["Kolor_cewki"].ToString();
                cart.TS_PW1 = reader["TS_PW1"].ToString();
                cart.Koniec_partii = reader["Koniec_Partii"].ToString();

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

            SqlCommand com = new SqlCommand("sp1GetEmptyCarts", conn);

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
        public IEnumerable<Przedza> GetPwStorageCarts()
        {
            List<Przedza> list = new List<Przedza>();
            connection();
            SqlCommand command = new SqlCommand("sp1GetPwStorageCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Przedza item = new Przedza
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString(),
                    Nazwa_maszyny = reader["MaszynaPW"].ToString(),
                    TS_MAG = reader["TS_MAG"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<Przedza> GetPzStorageCarts()
        {
            List<Przedza> list = new List<Przedza>();
            connection();
            SqlCommand command = new SqlCommand("sp1GetPzStorageCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Przedza item = new Przedza
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString(),
                    TS_MAG_PZ = reader["TS_MAG_PZ"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
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

        [HttpPut]
        public Response EditEmptyCart(Przedza przedzaCart)
        {

            Response response = new Response();

            if (string.IsNullOrEmpty(przedzaCart.Nr_wozka))
            {
                response.Message = "Numer wózka jest obowiązkowy";
                response.Status = 0;
            }
            else
            {
                connection();
                SqlCommand command = new SqlCommand("spUpdateNumberEmptyCart", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Nr_wozka", przedzaCart.Nr_wozka);
                command.Parameters.AddWithValue("@Nr_wozka2", przedzaCart.Nr_wozka2);

                conn.Open();
                int i = command.ExecuteNonQuery();
                conn.Close();
                if (i >= 1)
                {
                    response.Message = "Pomyślnie zmieniono numer wózka";
                    response.Status = 1;
                }
                else
                {
                    response.Message = "Nie udało się zmienić nazwy wózka";
                    response.Status = 0;
                }
            }
            return response;
        }

        [HttpPost]
        public Response AddEmptyCart(Przedza przedzaCart)
        {

            Response response = new Response();

            if (string.IsNullOrEmpty(przedzaCart.Nr_wozka))
            {
                response.Message = "Numer wózka jest obowiązkowy";
                response.Status = 0;
            }
            else
            {
                connection();
                SqlCommand command = new SqlCommand("sp1AddNumberEmptyCart", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Nr_wozka", przedzaCart.Nr_wozka);

                conn.Open();
                int i = command.ExecuteNonQuery();
                conn.Close();
                if (i >= 1)
                {
                    response.Message = "Pomyślnie zmieniono numer wózka";
                    response.Status = 1;
                }
                else
                {
                    response.Message = "Nie udało się zmienić nazwy wózka";
                    response.Status = 0;
                }
            }
            return response;
        }

        [HttpDelete]
        public Response RemoveEmptyCart(string Nr_wozka)
        {
            {

                Response response = new Response();

                if (string.IsNullOrEmpty(Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("sp1DeleteNumberEmptyCart", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Nr_wozka", Nr_wozka);

                    conn.Open();
                    int i = command.ExecuteNonQuery();
                    conn.Close();
                    if (i >= 1)
                    {
                        response.Message = "Pomyślnie zmieniono numer wózka";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Nie udało się zmienić nazwy wózka";
                        response.Status = 0;
                    }
                }
                return response;
            }
        }

    }
}
