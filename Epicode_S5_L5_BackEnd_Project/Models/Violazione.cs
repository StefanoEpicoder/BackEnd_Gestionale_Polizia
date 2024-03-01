using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicode_S5_L5_BackEnd_Project.Models
{
    public class Violazione
    {
        public int IdViolazione { get; set; }
        public string Descrizione { get; set; }

        public string ViolazioneCompleta => $"{IdViolazione} - {Descrizione}";

        public string Cognome { get; internal set; }
        public string Nome { get; internal set; }
        public string IndirizzoViolazione { get; internal set; }
        public DateTime DataViolazione { get; internal set; }
        public decimal Importo { get; internal set; }
        public int DecurtamentoPunti { get; internal set; }
    }
}