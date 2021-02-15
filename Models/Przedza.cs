using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraceabilityWebApi.Models
{
    public class Przedza
    {
        public int ID { get; set; }
        public int ID_Wozka { get; set; }
        public int ID_Maszyny_PZ { get; set; }
        public string Nm { get; set; }
        public string Material { get; set; }
        public string Typ_cewki { get; set; }
        public string Kolor_cewki { get; set; }
        public int ID_Suszenia_1 { get; set; }
        public int ID_Suszenia_2 { get; set; }
        public int ID_Suszenia_3 { get; set; }
        public string Wilgotnosc_1 { get; set; }
        public string Wilgotnosc_2 { get; set; }
        public string Wilgotnosc_3 { get; set; }
        public int ID_Maszyny_PW { get; set; }
        public int ID_Operatora_PZ { get; set; }
        public int ID_Operatora_PZ_Susz { get; set; }
        public int ID_Operatora_Susz_Kom { get; set; }
        public int ID_Operatora_Kom_PW { get; set; }
        public int ID_Operatora_PW { get; set; }
        public string TS_PZ { get; set; }
        public string TS_SUSZ1 { get; set; }
        public string TS_SUSZ2 { get; set; }
        public string TS_SUSZ3 { get; set; }
        public string TS_PZ_Susz { get; set; }
        public string TS_Susz_Kom { get; set; }
        public string TS_Kom_PW { get; set; }
        public string TS_KOM1 { get; set; }
        public string TS_PW1 { get; set; }
        public string TS_OUT { get; set; }
        public string TS_MAG { get; set; }
        public string TS_MAG_PZ { get; set; }
        public int ID_Operatora_Zwrot { get; set; }
        public int ID_Operatora_PZ_Susz2 { get; set; }
        public int ID_Operatora_PZ_Susz3 { get; set; }
        public int ID_Operatora_PW_Kom { get; set; }
        public string TS_PW_Kom { get; set; }
        public int ID_Operatora_Kom_PW2 { get; set; }
        public int ID_Operatora_PW2 { get; set; }
        public string TS_PW2 { get; set; }
        public string TS_Kom_PW2 { get; set; }
        public string TS_KOM2 { get; set; }
        public string Material2 { get; set; }
        public string Typ_cewki2 { get; set; }
        public string Kolor_cewki2 { get; set; }
        public int ID_Maszyny_PW2 { get; set; }
        public string TS_Laczenie { get; set; }
        public string Koniec_partii { get; set; }
        public string Koniec_partii2 { get; set; }

        public string Nr_wozka { get; set; }
        public string Nr_wozka2 { get; set; }
        public string Nazwa_maszyny { get; set; }
        public string Nazwa_maszyny2 { get; set; }
        public string Numer_partii { get; set; }
        public string Numer_partii2 { get; set; }
        public string Lokalizacja { get; set; }
    }
}