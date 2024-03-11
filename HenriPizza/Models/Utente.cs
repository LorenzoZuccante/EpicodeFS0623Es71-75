using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HenriPizza.Models
{
    public class Utente
    {
        [Key]
        public  int UtenteId {  get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 caratteri")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Max 16 caratteri")]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max 30 caratteri")]
        public string Nome { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max 30 caratteri")]
        public string Cognome { get; set; }

        [Required]
        [Column(TypeName ="11,0")]
        public decimal Telefono { get; set; }

        public string Ruolo { get; set; }

        public ICollection<OrdineProdotto> OrdineProdotti { get; set; }
        public ICollection<RiepilogoOrdine> RiepilogoOrdini { get; set; }
    }
}