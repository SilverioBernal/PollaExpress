using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.PollaExpress.WebFront.Models
{
    public class VmPolla
    {
        public string idCustomer { get; set; }
        public string userName { get; set; }
        public string userMailAddress { get; set; }
        public System.DateTime betTime { get; set; }
        public int idGame { get; set; }
        public int team1Score { get; set; }
        public int team2Score { get; set; }
        public int saqueInicial { get; set; }
        public int primerTiroEsquina { get; set; }
        public int primeraTarjetaAmarilla { get; set; }
        public int primerCambio { get; set; }
        public int ganadorPrimerTiempo { get; set; }
        public int anotacionesPrimerTiempo { get; set; }
        public int anotacionesSegundoTiempo { get; set; }
        public int tarjetaRoja { get; set; }
        public int ambosAnotan { get; set; }
        public int penalty { get; set; }
        public int equipoCobraPrimerTiroLibre { get; set; }


        public string[] lsSimpleTeams { get; set; }
        public string[] lsComplexTeams { get; set; }
        public string[] lsYesNoAnswer { get; set; }
        public VmPolla()
        {

        }

        public VmPolla(string team1, string team2)
        {
            lsSimpleTeams = (string.Format("{0}|{1}", team1, team2)).Split('|');
            lsComplexTeams = (string.Format("{0}|{1}|Ninguno", team1, team2)).Split('|');
            lsYesNoAnswer = (string.Format("{0}|{1}", "Si", "No")).Split('|');
        }
    }
}