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
    public static class ScoreCRUD
    {
        /*CRUD score*/

        public static List<Score> GetScoreList()
        {

            List<Score> lstScore = new List<Score>();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstScore = ctx.Score.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstScore;
        }

        public static Score GetScoreByKey(int idGame)
        {
            Score oScore = new Score();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oScore =
                        ctx.Score.Where(x => x.idGame.Equals(idGame)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oScore;
        }

        public static void SaveScore(Score score)
        {

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the student exists
                    Score oScore = GetScoreByKey(score.idGame);

                    if (oScore != null)
                    {
                        // if exists then edit 
                        ctx.Score.Attach(oScore);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oScore, score);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Score.Add(score);
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

        public static void DeleteScore(int idGame)
        {
            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the school exists
                    Score oScore = GetScoreByKey(idGame);

                    if (oScore != null)
                    {
                        // if exists then edit 
                        ctx.Score.Attach(oScore);
                        ctx.Score.Remove(oScore);
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
