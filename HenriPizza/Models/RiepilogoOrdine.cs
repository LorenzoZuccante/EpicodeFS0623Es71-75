using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HenriPizza.Models
{
    public class RiepilogoOrdine
    {
        [Key]
        public int RiepilogoOrdineId { get; set; }

        public int UtenteId { get; set; }

        public string DataOrdine { get; set; }

        public string IndirizzoSpedizione { get; set; }

        public string Nota { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Range(1, 299, ErrorMessage = "Scegli un valore compreso tra 1 e 299")]
        public decimal PrezzoTotale { get; set; }

        public string Stato { get; set; }

        public ICollection<OrdineProdotto> RiepilogoOrdini { get; set; }
        public Utente Utente { get; set; }


    }
}