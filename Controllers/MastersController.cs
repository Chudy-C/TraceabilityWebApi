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

        public Response BackToKom(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spBackToKom", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 94# " + Environment.NewLine + "Brak poprzedniego etapu lub znajduje się już w Komorze sezonowania. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
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

        [HttpGet]
        public IEnumerable<StorageCarts> CartState()
        {
            List<StorageCarts> list = new List<StorageCarts>();
            connection();
            SqlCommand command = new SqlCommand("spGetCartState", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                StorageCarts item = new StorageCarts
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    State = reader["Stan"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<Carts> GetCartInfo(string Nr_wozka)
        {
            List<Carts> list = new List<Carts>();
            Carts item = new Carts();
            connection();
            SqlCommand command = new SqlCommand("spGetCartInfo", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Nr_wozka", item.Nr_wozka);
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                item.Id_cart = Convert.ToInt32(reader["ID_Wozka"]);
                item.Id_machine_PZ = Convert.ToInt32(reader["ID_Maszyny_PZ"]);
                item.Nm = reader["Nm"].ToString();
                item.Material = reader["Material"].ToString();
                item.CoilType = reader["Typ_cewki"].ToString();
                item.CoilColor = reader["Kolor_cewki"].ToString();
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<CartsPZ> GetEmptyCarts()
        {
            List<CartsPZ> list = new List<CartsPZ>();
            connection();
            SqlCommand command = new SqlCommand("spGetEmptyCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CartsPZ item = new CartsPZ
                {
                    Nr_wozka = reader["Nr_wozka"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<StorageCarts> GetEmptyPW()
        {
            List<StorageCarts> list = new List<StorageCarts>();
            connection();
            SqlCommand command = new SqlCommand("spListEmptyPW", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                StorageCarts item = new StorageCarts
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    MaszynaPW = reader["MaszynaPW"].ToString(),
                    TS_MAG_PW = reader["TS_MAG_PW"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpPost]
        public Response GetEmptyStorageCart(StorageCarts storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spGetEmptyStorageCart", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    command.Parameters.AddWithValue("@MaszynaPW", storageCart.MaszynaPW);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        string[] textArray1 = new string[] { "Błąd 69# ", Environment.NewLine, "Nie udało się pobrać w\x00f3zka z maszyny.", Environment.NewLine, "W\x00f3zek został pobrany lub nie zakończył poprzedniej trasy." };
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

        [HttpPost]
        public Response GetFromStorage(StorageCarts storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spGetFromStorage", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    command.Parameters.AddWithValue("@Typ_cewki", storageCart.CoilType);
                    command.Parameters.AddWithValue("@Kolor_cewki", storageCart.CoilColor);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 67# " + Environment.NewLine + "Nie udało się pobrać w\x00f3zka z Magazynu. Został już dodany lub nie zakończył poprzedniego etapu";
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

        [HttpGet]
        public IEnumerable<StorageCarts> GetInStorage()
        {
            List<StorageCarts> list = new List<StorageCarts>();
            connection();
            SqlCommand command = new SqlCommand("spListInStorage", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                StorageCarts item = new StorageCarts
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    CoilType = reader["Typ_cewki"].ToString(),
                    CoilColor = reader["Kolor_cewki"].ToString(),
                    TS_MAG = reader["TS_MAG"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<CartsPZ> GetKomCarts()
        {
            List<CartsPZ> list = new List<CartsPZ>();
            connection();
            SqlCommand command = new SqlCommand("spGetKomCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CartsPZ item = new CartsPZ
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Nm = reader["Nm"].ToString(),
                    Material = reader["Material"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<CartsPZ> GetPoCarts()
        {
            List<CartsPZ> list = new List<CartsPZ>();
            connection();
            SqlCommand command = new SqlCommand("spGetPoCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CartsPZ item = new CartsPZ
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Nm = reader["Nm"].ToString(),
                    Material = reader["Material"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString(),
                    Wilgotnosc = reader["Wilgotnosc_1"].ToString(),
                    Wilgotnosc2 = reader["Wilgotnosc_2"].ToString(),
                    TS_KOM1 = reader["TS_KOM1"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<CartsPZ> GetPrzedCarts()
        {
            List<CartsPZ> list = new List<CartsPZ>();
            connection();
            SqlCommand command = new SqlCommand("spGetPrzedCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CartsPZ item = new CartsPZ
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Nm = reader["Nm"].ToString(),
                    Material = reader["Material"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString(),
                    TS_SUSZ1 = reader["TS_SUSZ1"].ToString(),
                    Nazwa_maszyny = reader["Suszenie1"].ToString(),
                    TS_SUSZ2 = reader["TS_SUSZ2"].ToString(),
                    Nazwa_maszyny2 = reader["Suszenie2"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<CartsPW> GetPwCarts()
        {
            List<CartsPW> list = new List<CartsPW>();
            connection();
            SqlCommand command = new SqlCommand("spGetPwCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CartsPW item = new CartsPW
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Nm = reader["Nm"].ToString(),
                    Material = reader["Material"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString(),
                    Nazwa_maszyny = reader["MaszynaPW1"].ToString(),
                    TS_PW1 = reader["TS_PW1"].ToString(),
                    TS_PW2 = reader["TS_PW2"].ToString(),
                    Nazwa_maszyny2 = reader["MaszynaPW2"].ToString(),
                    PartiaString = reader["Koniec_Partii"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        public IEnumerable<CartsPZ> GetPzCarts()
        {
            List<CartsPZ> list = new List<CartsPZ>();
            connection();
            SqlCommand command = new SqlCommand("spGetPzCarts", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CartsPZ item = new CartsPZ
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    Nm = reader["Nm"].ToString(),
                    Material = reader["Material"].ToString(),
                    Typ_cewki = reader["Typ_cewki"].ToString(),
                    Kolor_cewki = reader["Kolor_cewki"].ToString(),
                    TS_PZ = reader["TS_PZ"].ToString(),
                    PartiaString = reader["Koniec_Partii"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpGet]
        public IEnumerable<StorageCarts> GetStoragePZ()
        {
            List<StorageCarts> list = new List<StorageCarts>();
            connection();
            SqlCommand command = new SqlCommand("spListStoragePZ", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                StorageCarts item = new StorageCarts
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    CoilType = reader["Typ_cewki"].ToString(),
                    CoilColor = reader["Kolor_cewki"].ToString(),
                    TS_MAG_PZ = reader["TS_MAG_PZ"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        public Response ResetCart(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spResetCart", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 92# " + Environment.NewLine + "Błąd. Zgłoś problem do administratora.";
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

        public Response ReturnCart(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spReturnCart", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 93# " + Environment.NewLine + "Brak poprzedniego etapu lub w\x00f3zek jest już pusty. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
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

        [HttpPost]
        public Response ReturnEmptyStorageCart(StorageCarts storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spReturnStorageCart", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        string[] textArray1 = new string[] { "Błąd 66# ", Environment.NewLine, "Nie udało się zwr\x00f3cić pustego w\x00f3zka.", Environment.NewLine, "W\x00f3zek został oddany lub nie zakończył poprzedniego etapu." };
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

        public Response SendDmgLabelMail(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spLabelDmg", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    SqlParameter parameter = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    int num = (int)parameter.Value;
                    this.conn.Open();
                    if (command.ExecuteNonQuery() < 1)
                    {
                        response.Message = "Błąd 80# " + Environment.NewLine + "Aktualizacja danych nie przebiegła pomyślnie \n Poprzedni etap nie został zakończony";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.ValInt = num;
                        response.Status = 1;
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

        [HttpPost]
        public Response SendState(StorageCarts storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spSendState", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    command.Parameters.AddWithValue("@State", storageCart.State);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 59# " + Environment.NewLine + "Nie udało się zmienić stanu w\x00f3zka. Skontaktuj się z Administratorem";
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

        public Response SendToChamber(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
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
                    SqlCommand command = new SqlCommand("spSendToChamber", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    command.Parameters.AddWithValue("@Wilgotnosc", cart.Dampness);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 96# " + Environment.NewLine + "Brak poprzedniego etapu lub znajduje się już w Komorze sezonowania. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
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

        public Response SendToPw(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
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
                    SqlCommand command = new SqlCommand("spToPW", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    command.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Aktualizacja przebiegła pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 95# " + Environment.NewLine + "Brak poprzedniego etapu lub znajduje się już na etapie 'Przewijalnia'. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
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
        [HttpPost]
        public Response StartTrace(Carts cart)
        {
            Response response = new Response();
            try
            {
                connection();
                SqlCommand command = new SqlCommand("spStartTrace", this.conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID", cart.Id);
                command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                command.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                command.Parameters.AddWithValue("@Id_Maszyny", cart.Id_maszyny);
                command.Parameters.AddWithValue("@ID_Maszyny_PZ", cart.Id_machine_PZ);
                command.Parameters.AddWithValue("@Nm", cart.Nm);
                command.Parameters.AddWithValue("@Material", cart.Material);
                command.Parameters.AddWithValue("@Typ_cewki", cart.CoilType);
                command.Parameters.AddWithValue("@Kolor_cewki", cart.CoilColor);
                command.Parameters.AddWithValue("@ID_Operatora_PZ", cart.ID_Operator_PZ);
                command.Parameters.AddWithValue("@TS_PZ", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@Koniec_partii", cart.PartiaString);
                this.conn.Open();
                if (command.ExecuteNonQuery() >= 1)
                {
                    response.Message = "Wozek dodany pomyslnie";
                    response.Status = 1;
                }
                else
                {
                    response.Message = "Błąd 99# " + Environment.NewLine + "W\x00f3zek nie zakończył trasy lub znajduje się już na etapie 'Przewijalnia'. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
                    response.Status = 0;
                }
            }
            catch (Exception exception1)
            {
                response.Message = exception1.Message;
                response.Status = 0;
            }
            return response;
        }

        [HttpGet]
        public IEnumerable<StorageCarts> StorageCartValue()
        {
            List<StorageCarts> list = new List<StorageCarts>();
            connection();
            SqlCommand command = new SqlCommand("spStorageCartValues", this.conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            this.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                StorageCarts item = new StorageCarts
                {
                    Nr_wozka = reader["Nr_wozka"].ToString(),
                    CoilType = reader["Typ_cewki"].ToString(),
                    CoilColor = reader["Kolor_cewki"].ToString()
                };
                list.Add(item);
            }
            this.conn.Close();
            return list;
        }

        [HttpPost]
        public Response ToDryingAgain(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
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
                    SqlCommand command = new SqlCommand("spToDryingAgain", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    command.Parameters.AddWithValue("@ID_Maszyny", cart.Id_maszyny);
                    command.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    command.Parameters.AddWithValue("@Wilgotnosc", cart.Dampness);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 97# " + Environment.NewLine + "Brak poprzedniego etapu lub znajduje się już w suszarni. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
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

        public Response ToDryingFirst(Carts cart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(cart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
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
                    SqlCommand command = new SqlCommand("spToDryingFirst", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID", cart.Id);
                    command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                    command.Parameters.AddWithValue("@ID_Wozka", cart.Id_cart);
                    command.Parameters.AddWithValue("@ID_Maszyny", cart.Id_maszyny);
                    command.Parameters.AddWithValue("@Nazwa_maszyny", cart.Nazwa_maszyny);
                    command.Parameters.AddWithValue("@ID_Suszenia_1", cart.ID_Drying1);
                    command.Parameters.AddWithValue("@ID_Operatora_PZ_Susz", cart.ID_Operator_PZ_Susz);
                    command.Parameters.AddWithValue("@TS_SUSZ1", DateTime.Now.ToString());
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        response.Message = "Błąd 98# " + Environment.NewLine + "Brak poprzedniego etapu lub znajduje się już na etapie 'Przed Suszeniem'. Sprawdź listy w\x00f3zk\x00f3w na danych etapach.";
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

        [HttpPost]
        public Response ToStorage(StorageCarts storageCart)
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(storageCart.Nr_wozka))
                {
                    response.Message = "Numer w\x00f3zka jest obowiązkowy";
                    response.Status = 0;
                }
                else
                {
                    connection();
                    SqlCommand command = new SqlCommand("spSendToStorage", this.conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Nr_wozka", storageCart.Nr_wozka);
                    command.Parameters.AddWithValue("@Typ_cewki", storageCart.CoilType);
                    command.Parameters.AddWithValue("@Kolor_cewki", storageCart.CoilColor);
                    this.conn.Open();
                    if (command.ExecuteNonQuery() >= 1)
                    {
                        response.Message = "Wozek dodany pomyslnie";
                        response.Status = 1;
                    }
                    else
                    {
                        string[] textArray1 = new string[] { "Błąd 68# ", Environment.NewLine, "Nie udało się dodać w\x00f3zka do Magazynu.", Environment.NewLine, "W\x00f3zek został dodany lub nie rozpoczął poprzedniego etapu." };
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

        public Response UpdatePZ(CartsPZ cart)
        {
            Response response = new Response();
            try
            {
                connection();
                SqlCommand command = new SqlCommand("spUpdatePZ", this.conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Nr_wozka", cart.Nr_wozka);
                command.Parameters.AddWithValue("@Nr_wozka2", cart.Nr_wozka2);
                command.Parameters.AddWithValue("@Nm", cart.Nm);
                command.Parameters.AddWithValue("@Material", cart.Material);
                command.Parameters.AddWithValue("@Typ_cewki", cart.Typ_cewki);
                command.Parameters.AddWithValue("@Kolor_cewki", cart.Kolor_cewki);
                command.Parameters.AddWithValue("@Koniec_partii", cart.PartiaString);
                this.conn.Open();
                if (command.ExecuteNonQuery() >= 1)
                {
                    response.Message = "Wozek dodany pomyslnie";
                    response.Status = 1;
                }
                else
                {
                    response.Message = "Nie udało się zaktualizować w\x00f3zka.";
                    response.Status = 0;
                }
            }
            catch (Exception exception1)
            {
                response.Message = exception1.Message;
                response.Status = 0;
            }
            return response;
        }
    }
}
