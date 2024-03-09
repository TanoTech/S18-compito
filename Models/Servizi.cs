using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace S18_compito.Models
{
    public class Servizi
    {
        public int ID { get; set; }
        public int IDPrenotazione { get; set; }
        public string Servizio { get; set; }    
        public int Quantità { get; set; }   
        public SqlMoney Prezzo { get; set; }    
        public DateTime Data {  get; set; }
    }
}