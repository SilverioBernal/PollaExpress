using Orkidea.PollaExpress.DAL;
using Orkidea.PollaExpress.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.PollaExpress.Business
{
    public class PredictionBiz
    {
        public void SavePrediction(Prediction prediction)
        {
            PredictionCRUD.SavePrediction(prediction);
        }
    }
}
