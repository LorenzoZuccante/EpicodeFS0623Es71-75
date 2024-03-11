using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HenriPizza.Models
{
    public class Prodotto
    {
        [Key]
        public int ProdottoId { get; set; }

        [Required]
        [StringLength(30, ErrorMessage ="Max 30 caratteri")]
        public string NomeProdotto { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max 30 caratteri")]
        public string FotoProdotto { get; set; }

        [Required]
        [Column (TypeName ="decimal(6,2)")]
        [Range(1, 99, ErrorMessage ="Scegli un valore compreso tra 1 e 99")]
        public decimal PrezzoProdotto { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 caratteri")]
        public string TempoDiPreparazione { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        public string Ingredienti { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max 30 caratteri")]
        public string Categoria {  get; set; }

        public OrdineProdotto OrdineProdotto { get; set; }

    }
}