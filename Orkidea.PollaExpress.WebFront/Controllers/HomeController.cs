using Orkidea.PollaExpress.Business;
using Orkidea.PollaExpress.Entities;
using Orkidea.PollaExpress.WebFront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Orkidea.PollaExpress.Utilities;
using System.Configuration;
using System.IO;

namespace Orkidea.PollaExpress.WebFront.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            return View();
        }

        [Authorize]
        public ActionResult Ref(string id)
        {
            CustomerBiz cb = new CustomerBiz();
            Customer customer = cb.GetCustomer(id);

            ViewBag.id = id;
            ViewBag.logo = customer.logo;
            ViewBag.nombre = customer.nombre;

            return View();

            //return RedirectToAction("Game", new { id = id });
        }

        [Authorize]
        public ActionResult Game(string id)
        {
            GameBiz gb = new GameBiz();
            List<Game> lsGames = gb.GetGames();
            List<Game> lsGamesModel = new List<Game>();

            CustomerBiz cb = new CustomerBiz();
            Customer customer = cb.GetCustomer(id);

            ViewBag.id = id;
            ViewBag.logo = customer.logo;
            ViewBag.nombre = customer.nombre;

            DateTime fecha = new DateTime(DateTime.Now.AddHours(4).Year, DateTime.Now.AddHours(4).Month, DateTime.Now.AddHours(4).Day);
            //DateTime.Now.Hour.

            if (customer.mostrarTodo)
                lsGamesModel.AddRange(lsGames.Where(x => x.gameDate >= DateTime.Now).ToList());
            else
                lsGamesModel.Add(lsGames.Where(x => x.gameDate >= fecha).OrderBy(x => x.gameDate).First());

            return View(lsGamesModel);
        }

        [Authorize]
        public ActionResult trivia(string id)
        {
            string[] complexId = id.Split('|');

            CustomerBiz cb = new CustomerBiz();
            Customer customer = cb.GetCustomer(complexId[0]);

            GameBiz gb = new GameBiz();
            Game game = gb.GetGames().Where(x => x.id.Equals(int.Parse(complexId[1]))).First();

            ViewBag.id = customer.id;
            ViewBag.logo = customer.logo;
            ViewBag.partido = game.team1.ToUpper() + " Vs " + game.team2.ToUpper();
            ViewBag.team1 = game.team1;
            ViewBag.team2 = game.team2;


            VmPolla prediction = new VmPolla(game.team1, game.team2)
            {
                userName = User.Identity.GetUserName(),
                idCustomer = customer.id,
                idGame = game.id,
            };

            return View(prediction);
        }

        [HttpPost]
        public ActionResult trivia(string id, VmPolla prediction)
        {
            string[] complexId = id.Split('|');

            Prediction polla = new Prediction()
            {
                userMailAddress = prediction.userName,
                userName = prediction.userName,
                ambosAnotan = prediction.ambosAnotan == 1 ? true : false,
                anotacionesPrimerTiempo = prediction.anotacionesPrimerTiempo == 1 ? true : false,
                anotacionesSegundoTiempo = prediction.anotacionesSegundoTiempo == 1 ? true : false,
                betTime = DateTime.Now.AddHours(-4),
                idCustomer = prediction.idCustomer,
                equipoCobraPrimerTiroLibre = prediction.equipoCobraPrimerTiroLibre,
                ganadorPrimerTiempo = prediction.ganadorPrimerTiempo,
                idGame = prediction.idGame,
                penalty = prediction.penalty == 1 ? true : false,
                primeraTarjetaAmarilla = prediction.primeraTarjetaAmarilla,
                primerCambio = prediction.primerCambio,
                primerTiroEsquina = prediction.primerTiroEsquina,
                saqueInicial = prediction.saqueInicial == 1 ? true : false,
                tarjetaRoja = prediction.tarjetaRoja == 1 ? true : false,
                team1Score = prediction.team1Score,
                team2Score = prediction.team2Score
            };

            PredictionBiz pb = new PredictionBiz();

            pb.SavePrediction(polla);

            return RedirectToAction("thanks");
        }

        public ActionResult thanks()
        {
            return View();
        }

        public ActionResult GetLogo(string archivo)
        {
            MimeTypeBiz mtb = new MimeTypeBiz();

            string mimeType = "";
            int cuentaPuntos = 0;
            string[] nombreArchivo = archivo.Split('.');
            cuentaPuntos = nombreArchivo.Length;
            mimeType = mtb.GetMimeType(nombreArchivo[cuentaPuntos - 1]).mimetype1;
            //dynamically generate a file
            System.IO.MemoryStream ms;
            ms = AzureStorageHelper.getFile(archivo, "uploadedFiles");
            // return the file
            return File(ms.ToArray(), mimeType, archivo);
        }

        [Authorize]
        public ActionResult Customers()
        {
            string[] managers = Cryptography.Decrypt(ConfigurationManager.AppSettings["mngrs"].ToString()).Split('|');
            bool manager = false;

            foreach (string item in managers)
            {
                if (User.Identity.GetUserName() == item)
                    manager = true;
            }

            if (manager)
            {
                CustomerBiz cb = new CustomerBiz();
                return View(cb.GetCustomerList());
            }
            else
                return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult CreateCustomer()
        {
            string[] managers = Cryptography.Decrypt(ConfigurationManager.AppSettings["mngrs"].ToString()).Split('|');
            bool manager = false;

            foreach (string item in managers)
            {
                if (User.Identity.GetUserName() == item)
                    manager = true;
            }

            if (manager)
            {
                CustomerBiz cb = new CustomerBiz();
                return View();
            }
            else
                return RedirectToAction("index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateCustomer(VmCustomer oCustomer, IEnumerable<HttpPostedFileBase> files)
        {

            try
            {
                string fileExtension = Path.GetExtension(oCustomer.File.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                AzureStorageHelper.uploadFile(oCustomer.File.InputStream, fileName, "uploadedFiles");

                CustomerBiz cb = new CustomerBiz();

                cb.SaveCustomer(new Customer()
                {
                    id = oCustomer.id,
                    nombre = oCustomer.nombre,
                    email = oCustomer.email,
                    logo = fileName,
                    ganadoresPartido = oCustomer.ganadoresPartido,
                    mostrarTodo = oCustomer.mostrarTodo,
                    vendidoPor = oCustomer.vendidoPor
                });


                return RedirectToAction("Customers");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}