//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orkidea.PollaExpress.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prediction
    {
        public string idCustomer { get; set; }
        public string userName { get; set; }
        public string userMailAddress { get; set; }
        public System.DateTime betTime { get; set; }
        public int idGame { get; set; }
        public int team1Score { get; set; }
        public int team2Score { get; set; }
        public bool saqueInicial { get; set; }
        public int primerTiroEsquina { get; set; }
        public int primeraTarjetaAmarilla { get; set; }
        public int primerCambio { get; set; }
        public int ganadorPrimerTiempo { get; set; }
        public bool anotacionesPrimerTiempo { get; set; }
        public bool anotacionesSegundoTiempo { get; set; }
        public bool tarjetaRoja { get; set; }
        public bool ambosAnotan { get; set; }
        public bool penalty { get; set; }
        public int equipoCobraPrimerTiroLibre { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Game Game { get; set; }
    }
}
