using Orkidea.PollaExpress.DAL;
using Orkidea.PollaExpress.Entities;
using Orkidea.PollaExpress.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.PollaExpress.Business
{
    public class GameBiz
    {
        public List<Game> GetGames()
        {
            return GameCRUD.GetGameList();
        }

        public void SaveGame(Game game)
        {
            GameCRUD.SaveGame(game);
        }

        public void SaveGameScore(Score score, string rootPath)
        {
            ScoreCRUD.SaveScore(score);

            FindWinners(score.idGame, rootPath);
        }

        private void FindWinners(int idGame, string rootPath)
        {
            List<Prediction> lsPollas = PredictionCRUD.GetPredictionList(idGame);
            Score score = ScoreCRUD.GetScoreByKey(idGame);

            #region Calculo de puntaje
            foreach (Prediction item in lsPollas)
            {
                int puntaje = 0;

                if (item.team1Score.Equals(score.team1Score))
                    puntaje += 30;
                if (item.team2Score.Equals(score.team2Score))
                    puntaje += 30;

                //----
                if (item.saqueInicial.Equals(score.saqueInicial))
                    puntaje += 40;

                //----
                if (item.primerTiroEsquina.Equals(score.primerTiroEsquina))
                    puntaje += 40;

                //----
                if (item.primeraTarjetaAmarilla.Equals(score.primeraTarjetaAmarilla))
                    puntaje += 40;

                //----
                if (item.primerCambio.Equals(score.primerCambio))
                    puntaje += 40;

                //----
                if (item.ganadorPrimerTiempo.Equals(score.ganadorPrimerTiempo))
                    puntaje += 40;

                //----
                if (item.anotacionesPrimerTiempo.Equals(score.anotacionesPrimerTiempo))
                    puntaje += 40;

                //----
                if (item.anotacionesSegundoTiempo.Equals(score.anotacionesSegundoTiempo))
                    puntaje += 40;

                //----
                if (item.tarjetaRoja.Equals(score.tarjetaRoja))
                    puntaje += 40;

                //----
                if (item.ambosAnotan.Equals(score.ambosAnotan))
                    puntaje += 40;

                //----
                if (item.penalty.Equals(score.penalty))
                    puntaje += 40;

                //----
                if (item.equipoCobraPrimerTiroLibre.Equals(score.equipoCobraPrimerTiroLibre))
                    puntaje += 40;


                item.puntaje = puntaje;

                PredictionCRUD.SavePrediction(item);
            }
            #endregion

            CustomerBiz cb = new CustomerBiz();
            List<Customer> customers = cb.GetCustomerList();

            foreach (Customer item in customers)
            {
                List<Prediction> lsPollasGanadoras = new List<Prediction>();
                int numGanadores = item.ganadoresPartido;

                if (numGanadores >= lsPollas.Where(x => x.idGame.Equals(idGame) && x.idCustomer == item.id).Count())
                    lsPollasGanadoras.AddRange(lsPollas.Where(x => x.idGame.Equals(idGame) && x.idCustomer == item.id).OrderByDescending(x => x.puntaje).Take(numGanadores).ToList());
                else
                    lsPollasGanadoras.AddRange(lsPollas.Where(x => x.idGame.Equals(idGame) && x.idCustomer == item.id).OrderByDescending(x => x.puntaje).ToList());

                StringBuilder ganadores = new StringBuilder();

                foreach (Prediction item2 in lsPollasGanadoras)
                {
                    ganadores.AppendLine(string.Format("- {0} puntaje: {1}", item2.userMailAddress, item2.puntaje));
                }

                SendMail(item, idGame, ganadores.ToString(), rootPath);
            }
        }        

        private void SendMail(Customer cliente, int idPartido, string ganadores, string rootPath)
        {
            Game partido = GameCRUD.GetGameByKey(idPartido);

            // send notification
            List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();

            if (ConfigurationManager.AppSettings["testMail"].ToString() == "N")
                to.Add(new System.Net.Mail.MailAddress(cliente.email));
            else
                to.Add(new System.Net.Mail.MailAddress("silverio.bernal@orkidea.co"));

            Dictionary<string, string> dynamicValues = new Dictionary<string, string>();
            dynamicValues.Add("[Cliente]", cliente.nombre);
            dynamicValues.Add("[Partido]", string.Format("{0} vs {1}  del día {2}", partido.team1, partido.team2, partido.gameDate.ToString("yyyy-MM-dd")));
            dynamicValues.Add("[Ganadores]", ganadores);

            MailingHelper.SendMail(to, string.Format("Ganadores partido {0} vs {1}  del día {2}", partido.team1, partido.team2, partido.gameDate.ToString("yyyy-MM-dd")),
                rootPath + ConfigurationManager.AppSettings["emailGanadorHtml"].ToString(),
                rootPath + ConfigurationManager.AppSettings["emailGanadorTxt"].ToString(),"", dynamicValues);
        }
    }
}
