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
