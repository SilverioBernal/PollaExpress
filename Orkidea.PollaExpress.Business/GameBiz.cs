using Orkidea.PollaExpress.DAL;
using Orkidea.PollaExpress.Entities;
using System;
using System.Collections.Generic;
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
    }
}
