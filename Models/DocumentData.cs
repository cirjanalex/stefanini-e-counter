using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace stefanini_e_counter.Models
{
    public static class DocumentData {

        public static Dictionary<DocumentFieldsEnum,string> DocumentFields = new Dictionary<DocumentFieldsEnum, string>{
            { DocumentFieldsEnum.Cerere_nr_inregistrare, "…………."},
            { DocumentFieldsEnum.Cerere_data, "………………"},
            { DocumentFieldsEnum.Nume_Prenume, "«Nume_Prenume»" },
            { DocumentFieldsEnum.CNP, "«CNP»"},
            { DocumentFieldsEnum.Act_Identitate, "«Cod»"},
            { DocumentFieldsEnum.Eliberat_de, "«Eliberat_de»"},
            { DocumentFieldsEnum.De_la, "«De_la»"},
            { DocumentFieldsEnum.Adresa_concatenata, "«Adresa_concatenata»"},
            { DocumentFieldsEnum.Data_angajare, "«Data_Angajare»"},
            { DocumentFieldsEnum.Zile_concediu_luate, "«Nr_zile»"},
            { DocumentFieldsEnum.Data_curenta, "31.10.2020"},
            { DocumentFieldsEnum.Medicul_de_familie, "«Medic_de_Familie»"}
        };

        private static int _nextId = 0; 

        public static Dictionary<DocumentFieldsEnum,string> NewData(string seed, string reason) {
            var newData = new Dictionary<DocumentFieldsEnum, string>();
            newData[DocumentFieldsEnum.Cerere_nr_inregistrare] = $"{Interlocked.Increment(ref _nextId)}";
            newData[DocumentFieldsEnum.Cerere_data] = DateTime.Now.ToShortDateString().ToString();
            newData[DocumentFieldsEnum.Nume_Prenume] = $"{seed}-NP";
            newData[DocumentFieldsEnum.CNP] = RandomNumberString(13);
            newData[DocumentFieldsEnum.Act_Identitate] = $"{seed}-CI";
            newData[DocumentFieldsEnum.Eliberat_de] = $"{seed}-Eliberat";
            newData[DocumentFieldsEnum.De_la] = $"{seed}-EliberatData";
            newData[DocumentFieldsEnum.Adresa_concatenata] = $"{seed}-Adresa";
            newData[DocumentFieldsEnum.Data_angajare] = $"{seed}-Angajare";
            newData[DocumentFieldsEnum.Zile_concediu_luate] = "0";
            newData[DocumentFieldsEnum.Data_curenta] = DateTime.Now.ToShortDateString().ToString();
            newData[DocumentFieldsEnum.Medicul_de_familie] = reason;

            return newData;
        }

        public static Dictionary<DocumentFieldsEnum,string> NewData(string seed) {
            return NewData(seed, $"{seed}-motiv");
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static string RandomNumberString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Dictionary<DocumentFieldsEnum,string> DataForAlexandraUngureanu(string documentType) {
            var newData = new Dictionary<DocumentFieldsEnum, string>();
            newData[DocumentFieldsEnum.Cerere_nr_inregistrare] = $"{Interlocked.Increment(ref _nextId)}";
            newData[DocumentFieldsEnum.Cerere_data] = DateTime.Now.ToShortDateString().ToString();
            newData[DocumentFieldsEnum.Nume_Prenume] = $"Alexandra Ungureanu";
            newData[DocumentFieldsEnum.CNP] = RandomNumberString(13);
            newData[DocumentFieldsEnum.Act_Identitate] = $"IF 123456";
            newData[DocumentFieldsEnum.Eliberat_de] = $"IPJCJ Ilfov";
            newData[DocumentFieldsEnum.De_la] = $"10.10.2015";
            newData[DocumentFieldsEnum.Adresa_concatenata] = $"Strada Florilor nr 89";
            newData[DocumentFieldsEnum.Data_angajare] = $"15.04.2014";
            newData[DocumentFieldsEnum.Zile_concediu_luate] = "0";
            newData[DocumentFieldsEnum.Data_curenta] = DateTime.Now.ToShortDateString().ToString();
            newData[DocumentFieldsEnum.Medicul_de_familie] = string.IsNullOrEmpty(documentType)?"calatorit la Suceava":documentType;

            return newData;
        }

        public static Dictionary<DocumentFieldsEnum,string> DataForDanMagirescu(string documentType) {
            var newData = new Dictionary<DocumentFieldsEnum, string>();
            newData[DocumentFieldsEnum.Cerere_nr_inregistrare] = $"{Interlocked.Increment(ref _nextId)}";
            newData[DocumentFieldsEnum.Cerere_data] = DateTime.Now.ToShortDateString().ToString();
            newData[DocumentFieldsEnum.Nume_Prenume] = $"Dan Magirescu";
            newData[DocumentFieldsEnum.CNP] = RandomNumberString(13);
            newData[DocumentFieldsEnum.Act_Identitate] = $"RD 898989";
            newData[DocumentFieldsEnum.Eliberat_de] = $"Sector 5 Bucuresti";
            newData[DocumentFieldsEnum.De_la] = $"01.03.2005";
            newData[DocumentFieldsEnum.Adresa_concatenata] = $"Bulevardul Revolutiei 53";
            newData[DocumentFieldsEnum.Data_angajare] = $"07.07.2001";
            newData[DocumentFieldsEnum.Zile_concediu_luate] = "0";
            newData[DocumentFieldsEnum.Data_curenta] = DateTime.Now.ToShortDateString().ToString();
            newData[DocumentFieldsEnum.Medicul_de_familie] = string.IsNullOrEmpty(documentType)?"medicul veterinar":documentType;

            return newData;
        }
        
    }
    public enum DocumentFieldsEnum
    {
        Cerere_nr_inregistrare,
        Cerere_data,
        Nume_Prenume,
        CNP,
        Act_Identitate,
        Eliberat_de,
        De_la,
        Adresa_concatenata,
        Data_angajare,
        Zile_concediu_luate,
        Data_curenta,
        Medicul_de_familie

    };
}

