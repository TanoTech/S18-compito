using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace S18_compito.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int IDCliente { get; set; }
        public int IDStanza { get; set; }
        public int Anno { get; set; }
        public int NumeroPrenotazione { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataPrenotazione { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckIn {  get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        public decimal Caparra {  get; set; }
        public decimal Tariffa { get; set; }
        public bool PensioneOmezza { get; set; }
        public bool Colazione { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string CustomerName { get; set; }
        public int RoomNumber { get; set; }
        public string RoomDescription { get; set; }
        public bool SingolaDoppia { get; set; }
    }
}