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
    public class StorageController : ApiController
    {
        SqlConnection conn;
        private void connection()
        {
            string conString = ConfigurationManager.ConnectionStrings["getConnection"].ToString();
            conn = new SqlConnection(conString);
        }

        [HttpPost]
        public Response ToStorage(Przedza storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else if (string.IsNullOrEmpty(storageCart.Nazwa_maszyny))
                {
                    response.Message = "Nazwa maszyny jest obowiązkowa. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else if (string.IsNullOrEmpty(storageCart.Typ_cewki))
                {
                    response.Message = "Typ cewki jest obowiązkowy. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else if (string.IsNullOrEmpty(storageCart.Kolor_cewki))
                {
                    response.Message = "Kolor cewki jest obowiązkowy. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("sp1SendToStorage", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    command.Parameters.AddWithValue("@Nazwa_maszyny", storageCart.Nazwa_maszyny);
                    command.Parameters.AddWithValue("@Typ_cewki", storageCart.Typ_cewki);
                    command.Parameters.AddWithValue("@Kolor_cewki", storageCart.Kolor_cewki);
                    conn.Open();
                    int i = command.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        string[] textArray1 = new string[] { "Błąd 68# ", Environment.NewLine, "Nie udało się dodać wózka do Magazynu.", Environment.NewLine, "Wózek nie zakończył poprzedniego etapu." };
                        response.Message = string.Concat(textArray1);
                        response.Status = 0;
                    }
                }
            }
            catch (Exception exception1)
            {
                response.Message = exception1.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response GetFromStorage(Przedza storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer wózka jest obowiązkowy. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else if (string.IsNullOrEmpty(storageCart.Typ_cewki))
                {
                    response.Message = "Typ cewki jest obowiązkowy. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else if (string.IsNullOrEmpty(storageCart.Kolor_cewki))
                {
                    response.Message = "Kolor cewki jest obowiązkowy. Uzupełnij brakujące dane";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("sp1GetFromStorage", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    command.Parameters.AddWithValue("@Typ_cewki", storageCart.Typ_cewki);
                    command.Parameters.AddWithValue("@Kolor_cewki", storageCart.Kolor_cewki);
                    conn.Open();
                    int i = command.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        string[] textArray1 = new string[] { "Błąd 68# ", Environment.NewLine, "Nie udało się dodać wózka do Magazynu PZ.", Environment.NewLine, "Wózek nie zakończył poprzedniego etapu." };
                        response.Message = string.Concat(textArray1);
                        response.Status = 0;
                    }
                }
            }
            catch (Exception exception1)
            {
                response.Message = exception1.Message;
                response.Status = 0;
            }
            return response;
        }
        [HttpPut]
        public Response ReturnEmptyStorageCart(Przedza cart)
        {
            Response response = new Response();
            try
            {

                connection();
                SqlCommand com = new SqlCommand("sp1ReturnStorageCart", conn);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);

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
    }
}
