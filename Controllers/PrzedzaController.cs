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
    public class PrzedzaController : ApiController
    {
        SqlConnection conn;
        private void connection()
        {
            string conString = ConfigurationManager.ConnectionStrings["getConnection"].ToString();
            conn = new SqlConnection(conString);
        }


        [HttpGet]
        public IEnumerable<Przedza> GetCoilsColor(string Nazwa_maszyny)
        {
            List<Przedza> list = new List<Przedza>();
            connection();
            SqlCommand command = new SqlCommand("spGetCoilsColor_Picker", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Nazwa_maszyny", Nazwa_maszyny);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Przedza item = new Przedza
                {
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();

            return list;
        }

        [HttpPost]
        public Response StartTrace(Przedza cart)
        {
            Response response = new Response();
            try
            {

                connection();
                SqlCommand com = new SqlCommand("sp1StartPrzedzaTrace", conn);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                com.Parameters.AddWithValue("@Nm", cart.Nm);
                com.Parameters.AddWithValue("@Material", cart.Material);
                com.Parameters.AddWithValue("@Typ_cewki", cart.Typ_cewki);
                com.Parameters.AddWithValue("@Kolor_cewki", cart.Kolor_cewki);
                //com.Parameters.AddWithValue("@ID_Operatora_PZ", cart.ID_Operatora_PZ);
                com.Parameters.AddWithValue("@Koniec_partii", cart.Koniec_partii);
                com.Parameters.AddWithValue("@Numer_partii", cart.Numer_partii);


                conn.Open();
                int i = com.ExecuteNonQuery();
                if (i >= 1)
                {
                    response.Message = "Wozek dodany pomyslnie";
                    response.Status = 1;
                }
                else
                {
                    response.Message = "Wózek nie zakończył trasy lub znajduje się już na etapie 'Przewijalnia'. Sprawdź listy wózków na danych etapach.";
                    response.Status = 0;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response EditTrace(Przedza cart)
        {
            Response response = new Response();
            try
            {

                connection();
                SqlCommand com = new SqlCommand("sp1EditTrace", conn);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                com.Parameters.AddWithValue("@Nm", cart.Nm);
                com.Parameters.AddWithValue("@Material", cart.Material);
                com.Parameters.AddWithValue("@Typ_cewki", cart.Typ_cewki);
                com.Parameters.AddWithValue("@Kolor_cewki", cart.Kolor_cewki);
                //com.Parameters.AddWithValue("@ID_Operatora_PZ", cart.ID_Operatora_PZ);
                com.Parameters.AddWithValue("@Koniec_partii", cart.Koniec_partii);
                com.Parameters.AddWithValue("@Numer_partii", cart.Numer_partii);


                conn.Open();
                int i = com.ExecuteNonQuery();
                if (i >= 1)
                {
                    response.Message = "Wozek dodany pomyslnie";
                    response.Status = 1;
                }
                else
                {
                    response.Message = "Wózek nie zakończył trasy lub znajduje się już na etapie 'Przewijalnia'. Sprawdź listy wózków na danych etapach.";
                    response.Status = 0;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response ToDryingFirst(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.Nazwa_maszyny))
                {
                    response.Message = "Nazwa maszyny jest obowiązkowa";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1ToDryingFirst", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);


                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub znajduje się już na etapie 'Przed Suszeniem'. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response EditSuszarniaCart(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.Nazwa_maszyny))
                {
                    response.Message = "Nazwa maszyny jest obowiązkowa";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1EditSuszarniaCart", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    com.Parameters.AddWithValue("@TS_SUSZ1", System.DateTime.Now.ToString());


                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub znajduje się już na etapie 'Przed Suszeniem'. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response ToDryingAgain(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.Nazwa_maszyny))
                {
                    response.Message = "Nazwa maszyny jest obowiązkowa";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.WilgotnoscOstatnia))
                {
                    response.Message = "Wilgotność jest obowiązkowa";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1ToDryingAgain", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    com.Parameters.AddWithValue("@Wilgotnosc", cart.WilgotnoscOstatnia);



                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub znajduje się już w suszarni. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response SendToChamber(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.Wilgotnosc_1))
                {
                    response.Message = "Wilgotność jest obowiązkowa";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1SendToChamber", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Wilgotnosc", cart.Wilgotnosc_1);


                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub znajduje się już w Komorze sezonowania. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response SendToPw(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.Nazwa_maszyny))
                {
                    response.Message = "Nazwa maszyny jest obowiązkowa";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1ToPW", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);


                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub znajduje się już na etapie 'Przewijalnia'. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response BackToKom(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1BackToKom", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);


                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub znajduje się już w Komorze sezonowania. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response ReturnCart(Przedza cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("sp1ReturnTraceCart", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Brak poprzedniego etapu lub wózek jest już pusty. Sprawdź listy wózków na danych etapach.";
                        response.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 0;
            }
            return response;
        }

    }
}
