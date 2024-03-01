using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicode_S5_L5_BackEnd_Project.Models
{
    public class Verbale
    {
        public int IdVerbale { get; set; }
        public DateTime DataViolazione { get; set; }
        public string IndirizzoViolazione { get; set; }
        public string NominativoAgente { get; set; }
        public DateTime DataTrascrizioneVerbale { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
        public int IdAnagrafica { get; set; }
        public int IdViolazione { get; set; }
        public int NumeroVerbaliTrascritti { get; internal set; }
        public string Nome { get; internal set; }
        public string Cognome { get; internal set; }
    }
}