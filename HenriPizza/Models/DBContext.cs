using System.Data.Entity;
using System.Linq;
using HenriPizza.Models;

namespace HenriPizza.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=DBContext")
        {
        }

        public DbSet<Utente> Utenti { get; set; }
        public DbSet<RiepilogoOrdine> RiepilogoOrdini { get; set; }
        public DbSet<OrdineProdotto> OrdineProdotti { get; set; }
        public DbSet<Prodotto> Prodotti { get; set; }


    }
}
