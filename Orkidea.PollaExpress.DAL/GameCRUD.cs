using Orkidea.PollaExpress.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.PollaExpress.DAL
{
    public static class GameCRUD
    {
        /*CRUD game*/

        public static List<Game> GetGameList()
        {

            List<Game> lstGame = new List<Game>();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstGame = ctx.Game.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstGame;
        }

        public static Game GetGameByKey(int idGame)
        {
            Game oGame = new Game();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oGame =
                        ctx.Game.Where(x => x.id.Equals(idGame)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oGame;
        }

        public static void SaveGame(Game game)
        {

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the student exists
                    Game oGame = GetGameByKey(game.id);

                    if (oGame != null)
                    {
                        // if exists then edit 
                        ctx.Game.Attach(oGame);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oGame, game);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Game.Add(game);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (DbEntityValidationException e)
            {
                StringBuilder oError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    oError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        oError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                string msg = oError.ToString();
                throw new Exception(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void DeleteGame(int idGame)
        {
            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the school exists
                    Game oGame = GetGameByKey(idGame);

                    if (oGame != null)
                    {
                        // if exists then edit 
                        ctx.Game.Attach(oGame);
                        ctx.Game.Remove(oGame);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    throw new Exception("No se puede eliminar esta sede porque existe información asociada a esta.");
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
