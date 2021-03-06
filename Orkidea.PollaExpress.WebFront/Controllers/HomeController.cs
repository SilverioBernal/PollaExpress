﻿using Orkidea.PollaExpress.Business;
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

            DateTime fecha = new DateTime(DateTime.Now.AddHours(-5).Year, DateTime.Now.AddHours(-5).Month, DateTime.Now.AddHours(-5).Day);            

            if (customer.mostrarTodo)
                lsGamesModel.AddRange(lsGames.Where(x => x.gameDate >= fecha).ToList());
            else
                lsGamesModel.Add(lsGames.Where(x => x.gameDate >= fecha && (x.gameHour.Hours> fecha.Hour && x.gameHour.Minutes> fecha.Minute)).OrderBy(x => x.gameDate).First());

            return View(lsGamesModel);
        }

        public ActionResult CreateGame()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(Game game)
        {
            GameBiz gb = new GameBiz();

            gb.SaveGame(game);
            return RedirectToAction("GameScore");
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
                idCustomer = prediction.idCustomer,
                idGame = prediction.idGame,
                userName = prediction.userName,
                userMailAddress = prediction.userName,
                betTime = DateTime.Now.AddHours(-5),
                team1Score = prediction.team1Score,
                team2Score = prediction.team2Score,
                saqueInicial = prediction.saqueInicial == 1 ? true : false,
                primerTiroEsquina = prediction.primerTiroEsquina,
                primeraTarjetaAmarilla = prediction.primeraTarjetaAmarilla,
                primerCambio = prediction.primerCambio,
                ganadorPrimerTiempo = prediction.ganadorPrimerTiempo,
                anotacionesPrimerTiempo = prediction.anotacionesPrimerTiempo == 1 ? true : false,
                anotacionesSegundoTiempo = prediction.anotacionesSegundoTiempo == 1 ? true : false,
                tarjetaRoja = prediction.tarjetaRoja == 1 ? true : false,
                ambosAnotan = prediction.ambosAnotan == 1 ? true : false,
                penalty = prediction.penalty == 1 ? true : false,
                equipoCobraPrimerTiroLibre = prediction.equipoCobraPrimerTiroLibre,
                puntaje= 0
            };

            PredictionBiz pb = new PredictionBiz();

            pb.SavePrediction(polla);

            return RedirectToAction("thanks", new { id= complexId[0]});
        }

        public ActionResult thanks(string id)
        {
            CustomerBiz cb = new CustomerBiz();
            Customer customer = cb.GetCustomer(id);

            ViewBag.id = id;
            ViewBag.logo = customer.logo;
            ViewBag.nombre = customer.nombre;

            return View();
        }

        public ActionResult GameScore()
        {
            GameBiz gb = new GameBiz();
            List<Game> lsGames = gb.GetGames();            

            return View(lsGames);
        }

        public ActionResult score(int id)
        {            
            GameBiz gb = new GameBiz();
            Game game = gb.GetGames().Where(x => x.id.Equals(id)).First();

            ViewBag.team1 = game.team1;
            ViewBag.team2 = game.team2;


            VmPolla prediction = new VmPolla(game.team1, game.team2)
            {                
                idGame = game.id,                 
            };

            return View(prediction);
        }

        [HttpPost]
        public ActionResult score(int id, VmPolla score)
        {
            score.idGame = id;

            GameBiz gb = new GameBiz();

            gb.SaveGameScore(new Score() {                
                idGame = score.idGame,                
                team1Score = score.team1Score,
                team2Score = score.team2Score,
                saqueInicial = score.saqueInicial == 1 ? true : false,
                primerTiroEsquina = score.primerTiroEsquina,
                primeraTarjetaAmarilla = score.primeraTarjetaAmarilla,
                primerCambio = score.primerCambio,
                ganadorPrimerTiempo = score.ganadorPrimerTiempo,
                anotacionesPrimerTiempo = score.anotacionesPrimerTiempo == 1 ? true : false,
                anotacionesSegundoTiempo = score.anotacionesSegundoTiempo == 1 ? true : false,
                tarjetaRoja = score.tarjetaRoja == 1 ? true : false,
                ambosAnotan = score.ambosAnotan == 1 ? true : false,
                penalty = score.penalty == 1 ? true : false,
                equipoCobraPrimerTiroLibre = score.equipoCobraPrimerTiroLibre,
            }, Server.MapPath("~"));

            return View("index");
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