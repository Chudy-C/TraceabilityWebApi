using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public class Carts
    {
        #region Przedza [Tablica]
        public int Id { get; set; }
        public int Id_cart { get; set; }
        public int Id_machine_PZ { get; set; }
        public string Nm { get; set; }
        public string Material { get; set; }
        public string CoilType { get; set; }
        public string CoilColor { get; set; }
        public int ID_Operator_PZ { get; set; }
        public int ID_Drying1 { get; set; }
        public int ID_Drying2 { get; set; }
        public int ID_Drying3 { get; set; }
        public string Dampness { get; set; }
        public string Dampness1 { get; set; }
        public string Dampness2 { get; set; }
        public string Dampness3 { get; set; }
        public int Id_machine_PW { get; set; }
        public int ID_Operator_PZ_Susz { get; set; }
        public int ID_Operator_Susz_Kom { get; set; }
        public int ID_Operator_Kom_PW { get; set; }
        public int ID_Operator_PW { get; set; }
        public int ID_Operator_Zwrot { get; set; }
        public string TS_PZ { get; set; }
        public string TS_Susz2 { get; set; }
        public string TS_Susz3 { get; set; }
        public string TS_PZ_Susz { get; set; }
        public string TS_Susz_Kom { get; set; }
        public string TS_Kom_PW { get; set; }
        public string TS_PW { get; set; }
        public string TS_OUT { get; set; }
        public int ID_Operatora_Zwrot { get; set; }
        public int ID_Operatora_PZ_Susz2 { get; set; }
        public int ID_Operatora_PZ_Susz3 { get; set; }
        public int ID_Operatora_Kom_PW2 { get; set; }
        public int ID_Operatora_PW_Kom { get; set; }
        public int ID_Operatora_PW2 { get; set; }
        public string TS_PW_Kom { get; set; }
        public string TS_PW2 { get; set; }
        public string TS_Kom_PW2 { get; set; }
        public string Waga_przedzy { get; set; }
        public string Material2 { get; set; }
        public string CoilType2 { get; set; }
        public string CoilColor2 { get; set; }

        
        #endregion

        #region Wozek[Tablica]
        public string Nr_wozka { get; set; }
        public string Waga_wozka { get; set; }

        public string Nr_wozka1 { get; set; }
        public string Nr_wozka2 { get; set; }
        public string Nr_wozka3 { get; set; }
        public string Nr_wozka4 { get; set; }
        public string Nr_wozka5 { get; set; }
        public string Nr_wozka6 { get; set; }
        public string Nr_wozka7 { get; set; }
        public string Nr_wozka8 { get; set; }
        public string Nr_wozka9 { get; set; }
        public string PartiaString { get; set; }


        #endregion

        #region Operator[Tablica]
        public int ID_operatora { get; set; }
        public int Nr_operatora { get; set; }
        #endregion

        #region Maszyna[Tablica]
        public int Id_maszyny { get; set; }
        public string Nazwa_maszyny { get; set; }
        public string Dzial { get; set; }
        #endregion


    }
}