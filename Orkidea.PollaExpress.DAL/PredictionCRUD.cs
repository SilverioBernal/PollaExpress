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
    public static class PredictionCRUD
    {
        /*CRUD prediction*/

        public static List<Prediction> GetPredictionList()
        {

            List<Prediction> lstPrediction = new List<Prediction>();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstPrediction = ctx.Prediction.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstPrediction;
        }

        public static Prediction GetPredictionByKey(string idCustomer, string userName, int idGame)
        {
            Prediction oPrediction = new Prediction();

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oPrediction =
                        ctx.Prediction.Where(x => x.idCustomer.Equals(idCustomer) && x.userName == userName && x.idGame.Equals(idGame)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPrediction;
        }

        public static void SavePrediction(Prediction prediction)
        {

            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the student exists
                    Prediction oPrediction = GetPredictionByKey(prediction.idCustomer, prediction.userName, prediction.idGame);

                    if (oPrediction != null)
                    {
                        // if exists then edit 
                        ctx.Prediction.Attach(oPrediction);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oPrediction, prediction);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Prediction.Add(prediction);
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

        public static void DeletePrediction(string idCustomer, string userName, int idGame)
        {
            try
            {
                using (var ctx = new PollaExpressDBEntities())
                {
                    //verify if the school exists
                    Prediction oPrediction = GetPredictionByKey(idCustomer, userName, idGame);

                    if (oPrediction != null)
                    {
                        // if exists then edit 
                        ctx.Prediction.Attach(oPrediction);
                        ctx.Prediction.Remove(oPrediction);
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
