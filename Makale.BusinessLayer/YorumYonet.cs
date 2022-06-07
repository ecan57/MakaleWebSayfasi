using Makale.DataAccessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class YorumYonet
    {
        private Repository<Yorum> repoYorum = new Repository<Yorum>();
        public Yorum YorumBul(int id)
        {
            return repoYorum.Find(x => x.Id == id);
        }

        public int YorumUpdate(Yorum yorum)
        {
            return repoYorum.Update(yorum);
        }

        public int YorumSil(Yorum yorum)
        {
            return repoYorum.Delete(yorum);
        }

        public int YorumEkle(Yorum yorum)
        {
            return repoYorum.Insert(yorum);
        }
    }
}
