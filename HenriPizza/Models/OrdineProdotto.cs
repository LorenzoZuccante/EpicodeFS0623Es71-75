using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HenriPizza.Models
{
    public class OrdineProdotto
    {
        [Key]
        public int OrdineProdottoId { get; set; }

        public int IdOrdine { get; set;}

        public int IdProdotto { get; set;}

        public int IdUser { get; set;}

        [Required]
        [Range(1,10, ErrorMessage ="Scegli un valore  compreso tra 1 e 10")]
        public int Quantita { get; set;}

        public decimal PrezzoOrdine { get; set;}

        public RiepilogoOrdine RiepilogoOrdini { get; set;}
        public Utente Utente { get; set;}
        public Prodotto Prodotto { get; set;}

    }
}