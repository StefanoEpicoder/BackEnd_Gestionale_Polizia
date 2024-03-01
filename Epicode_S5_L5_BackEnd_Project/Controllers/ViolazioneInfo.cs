using System;

namespace Epicode_S5_L5_BackEnd_Project.Controllers
{
    internal class ViolazioneInfo
    {
        public string Cognome { get; internal set; }
        public string Nome { get; internal set; }
        public string IndirizzoViolazione { get; internal set; }
        public DateTime DataViolazione { get; internal set; }
        public decimal Importo { get; internal set; }
        public int DecurtamentoPunti { get; internal set; }
    }
}