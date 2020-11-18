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
            { DocumentFieldsEnum.Zile_concediu_luate, "……….."},
            { DocumentFieldsEnum.Data_curenta, "31.10.2020"},
            { DocumentFieldsEnum.Medicul_de_familie, "medicul de familie"}
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
            newData[DocumentFieldsEnum.Zile_concediu_luate] = "....0....";
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

