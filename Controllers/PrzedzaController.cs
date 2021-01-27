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

        public Response StartTrace(Przedza cart)
        {
            Response response = new Response();
            try
            {

                connection();
                SqlCommand com = new SqlCommand("spStartTrace", conn);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@ID", cart.ID);
                com.Parameters.AddWithValue("@ID_Wozka", cart.ID_Wozka);
                com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                com.Parameters.AddWithValue("@Id_Maszyny", cart.ID_Maszyny_PZ);
                com.Parameters.AddWithValue("@ID_Maszyny_PZ", cart.ID_Maszyny_PZ);
                com.Parameters.AddWithValue("@Nm", cart.Nm);
                com.Parameters.AddWithValue("@Material", cart.Material);
                com.Parameters.AddWithValue("@Typ_cewki", cart.Typ_cewki);
                com.Parameters.AddWithValue("@Kolor_cewki", cart.Kolor_cewki);
                com.Parameters.AddWithValue("@ID_Operatora_PZ", cart.ID_Operatora_PZ);
                com.Parameters.AddWithValue("@TS_PZ", System.DateTime.Now.ToString());
                com.Parameters.AddWithValue("@Koniec_partii", cart.PartiaString);


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
                    SqlCommand com = new SqlCommand("spToDryingFirst", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID", cart.ID);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@ID_Wozka", cart.ID_Wozka);
                    com.Parameters.AddWithValue("@ID_Maszyny", cart.ID_Suszenia_1);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    com.Parameters.AddWithValue("@ID_Suszenia_1", cart.ID_Suszenia_1);
                    com.Parameters.AddWithValue("@ID_Operatora_PZ_Susz", cart.ID_Operatora_PZ_Susz);
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
                if (string.IsNullOrEmpty(cart.Wilgotnosc_1))
                {
                    response.Message = "Wilgotność jest obowiązkowa";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spToDryingAgain", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@ID_Wozka", cart.ID_Wozka);
                    com.Parameters.AddWithValue("@ID_Maszyny", cart.ID_Suszenia_1);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
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
                    SqlCommand com = new SqlCommand("spSendToChamber", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Wilgotnosc", cart.Dampness);


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
                    SqlCommand com = new SqlCommand("spToPW", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
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
                    SqlCommand com = new SqlCommand("spBackToKom", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
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
                    SqlCommand com = new SqlCommand("spReturnCart", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
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

        public Response ResetCart(Przedza cart)
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
                    SqlCommand com = new SqlCommand("spResetCart", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
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
                        response.Message = "Błąd. Zgłoś problem do administratora.";
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
