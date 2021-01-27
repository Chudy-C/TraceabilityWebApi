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
    public class MastersController : ApiController
    {

        SqlConnection conn;

        private void connection()
        {
            string conString = ConfigurationManager.ConnectionStrings["getConnection"].ToString();
            conn = new SqlConnection(conString);
        }

        public IEnumerable<Przedza> StorageCartValue()
        {

            List<Przedza> cartData = new List<Przedza>();

            connection();
            SqlCommand com = new SqlCommand("spStorageCartValues", conn);
            com.CommandType = CommandType.StoredProcedure;
            conn.Open();

            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Przedza storageCart = new Przedza();
                storageCart.Nr_wozka = reader["Nr_wozka"].ToString();
                storageCart.Typ_cewki = reader["Typ_cewki"].ToString();
                storageCart.Kolor_cewki = reader["Kolor_cewki"].ToString();

                cartData.Add(storageCart);
            }

            conn.Close();
            return cartData;
        }


        #region Podstawowe funkcje aplikacji (śledządze wózki)
        public Response StartTrace(Carts cart)
        {
            Response response = new Response();
            try
            {

                connection();
                SqlCommand com = new SqlCommand("spStartTrace", conn);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@ID", cart.Id);
                com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                com.Parameters.AddWithValue("@Id_Maszyny", cart.Id_maszyny);
                com.Parameters.AddWithValue("@ID_Maszyny_PZ", cart.Id_machine_PZ);
                com.Parameters.AddWithValue("@Nm", cart.Nm);
                com.Parameters.AddWithValue("@Material", cart.Material);
                com.Parameters.AddWithValue("@Typ_cewki", cart.CoilType);
                com.Parameters.AddWithValue("@Kolor_cewki", cart.CoilColor);
                com.Parameters.AddWithValue("@ID_Operatora_PZ", cart.ID_Operator_PZ);
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

        public Response ToDryingFirst(Carts cart)
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
                    com.Parameters.AddWithValue("@ID", cart.Id);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@ID_Maszyny", cart.Id_maszyny);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    com.Parameters.AddWithValue("@ID_Suszenia_1", cart.ID_Drying1);
                    com.Parameters.AddWithValue("@ID_Operatora_PZ_Susz", cart.ID_Operator_PZ_Susz);
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

        public Response ToDryingAgain(Carts cart)
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
                if (string.IsNullOrEmpty(cart.Dampness))
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
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@ID_Maszyny", cart.Id_maszyny);
                    com.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
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

        public Response SendToChamber(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy";
                    response.Status = 0;
                }
                if (string.IsNullOrEmpty(cart.Dampness))
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

        public Response SendToPw(Carts cart)
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

        public Response BackToKom(Carts cart)
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

        public Response ReturnCart(Carts cart)
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

        public Response ResetCart(Carts cart)
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


/* FUSE FUNCTIONS
        public Response FuseOne(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka, do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseOne", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseTwo(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseTwo", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseThree(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseThree", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka2);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseFour(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka4))
                {
                    response.Message = "Numer wózka(4), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseFour", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka4", cart.Nr_wozka4);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseFive(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka4))
                {
                    response.Message = "Numer wózka(4), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka5))
                {
                    response.Message = "Numer wózka(5), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseFive", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka4", cart.Nr_wozka4);
                    com.Parameters.AddWithValue("@Nr_wozka5", cart.Nr_wozka5);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseSix(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka4))
                {
                    response.Message = "Numer wózka(4), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka5))
                {
                    response.Message = "Numer wózka(5), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka6))
                {
                    response.Message = "Numer wózka(6), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseSix", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka4", cart.Nr_wozka4);
                    com.Parameters.AddWithValue("@Nr_wozka5", cart.Nr_wozka5);
                    com.Parameters.AddWithValue("@Nr_wozka6", cart.Nr_wozka6);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseSeven(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka4))
                {
                    response.Message = "Numer wózka(4), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka5))
                {
                    response.Message = "Numer wózka(5), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka6))
                {
                    response.Message = "Numer wózka(6), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka7))
                {
                    response.Message = "Numer wózka(7), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseSeven", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka4", cart.Nr_wozka4);
                    com.Parameters.AddWithValue("@Nr_wozka5", cart.Nr_wozka5);
                    com.Parameters.AddWithValue("@Nr_wozka6", cart.Nr_wozka6);
                    com.Parameters.AddWithValue("@Nr_wozka7", cart.Nr_wozka7);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseEight(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka4))
                {
                    response.Message = "Numer wózka(4), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka5))
                {
                    response.Message = "Numer wózka(5), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka6))
                {
                    response.Message = "Numer wózka(6), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka7))
                {
                    response.Message = "Numer wózka(7), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka8))
                {
                    response.Message = "Numer wózka(8), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseEight", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka4", cart.Nr_wozka4);
                    com.Parameters.AddWithValue("@Nr_wozka5", cart.Nr_wozka5);
                    com.Parameters.AddWithValue("@Nr_wozka6", cart.Nr_wozka6);
                    com.Parameters.AddWithValue("@Nr_wozka7", cart.Nr_wozka7);
                    com.Parameters.AddWithValue("@Nr_wozka8", cart.Nr_wozka8);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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

        public Response FuseNine(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer wózka,z którego pobierany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka1))
                {
                    response.Message = "Numer wózka(1), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka2))
                {
                    response.Message = "Numer wózka(2), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka3))
                {
                    response.Message = "Numer wózka(3), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka4))
                {
                    response.Message = "Numer wózka(4), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka5))
                {
                    response.Message = "Numer wózka(5), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka6))
                {
                    response.Message = "Numer wózka(6), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka7))
                {
                    response.Message = "Numer wózka(7), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka8))
                {
                    response.Message = "Numer wózka(8), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }

                if (string.IsNullOrEmpty(cart.Nr_wozka9))
                {
                    response.Message = "Numer wózka(9), do którego dokładany jest materiał, jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand com = new SqlCommand("spFuseNine", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    com.Parameters.AddWithValue("@Nr_wozka1", cart.Nr_wozka1);
                    com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                    com.Parameters.AddWithValue("@Nr_wozka3", cart.Nr_wozka3);
                    com.Parameters.AddWithValue("@Nr_wozka4", cart.Nr_wozka4);
                    com.Parameters.AddWithValue("@Nr_wozka5", cart.Nr_wozka5);
                    com.Parameters.AddWithValue("@Nr_wozka6", cart.Nr_wozka6);
                    com.Parameters.AddWithValue("@Nr_wozka7", cart.Nr_wozka7);
                    com.Parameters.AddWithValue("@Nr_wozka8", cart.Nr_wozka8);
                    com.Parameters.AddWithValue("@Nr_wozka9", cart.Nr_wozka9);

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Sprawdź czy numer wózka jest prawidłowy oraz czy jest połączenie WiFi";
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
*/

        #endregion

        #region Inne
        public Response UpdatePZ(CartsPZ cart)
        {
            Response response = new Response();
            try
            {

                connection();
                SqlCommand com = new SqlCommand("spUpdatePZ", conn);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                com.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                com.Parameters.AddWithValue("@Nm", cart.Nm);
                com.Parameters.AddWithValue("@Material", cart.Material);
                com.Parameters.AddWithValue("@Typ_cewki", cart.Typ_cewki);
                com.Parameters.AddWithValue("@Kolor_cewki", cart.Kolor_cewki);
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
                    response.Message = "Nie udało się zaktualizować wózka.";
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
        public Response SendDmgLabelMail(Carts cart)
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
                    SqlCommand com = new SqlCommand("spLabelDmg", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);

                    var returnParameter = com.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    int returnValue = (int)returnParameter.Value;

                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.ValInt = returnValue;
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Aktualizacja danych nie przebiegła pomyślnie \n Poprzedni etap nie został zakończony";
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


        #endregion
    }
}
