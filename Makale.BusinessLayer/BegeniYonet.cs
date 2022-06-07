using Makale.DataAccessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class BegeniYonet
    {
        Repository<Begeni> repoBegeni = new Repository<Begeni>();
        public Begeni BegeniGetir(int notid, int kullaniciid)
        {
            return repoBegeni.Find(x => x.Makale.Id == notid && x.Kullanici.Id == kullaniciid);
        }
        public IQueryable<Begeni> ListQueryable()
        {
            return repoBegeni.ListQueryable();
        }
        public List<Begeni> List(Expression<Func<Begeni, bool>> kosul)
        {
            return repoBegeni.List(kosul);
        }
        public int BegeniEkle(Begeni begeni)
        {
            return repoBegeni.Insert(begeni);
        }
        public int BegeniSil(Begeni begeni)
        {
            return repoBegeni.Delete(begeni);
        }
    }
}
