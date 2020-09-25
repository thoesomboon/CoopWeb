using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Models.POCO;

namespace Coop.Models.Repository
{
    public interface ITitleRepository : IRepository<Title>
    {
        IQueryable<TitleModel> ReadDetail();
        //IQueryable<TitleModel> TitleList();
    }
    public class TitleRepository : Repository<Title>, ITitleRepository
    {
        public TitleRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<TitleModel> ReadDetail()
        {
            var title = from t in Read()
                        select new TitleModel
                        {
                            TitleID = t.TitleID,
                            TitleName = t.TitleName
                        };
            return title;
        }
        //public IQueryable<TitleModel> TitleList()
        //{
        //    return (from t in _context.Title
        //            where t.IsActive
        //            select new TitleModel
        //            {
        //                TitleID = t.TitleID,
        //                TitleName = t.TitleName
        //            });
        //}
    }

    //public class TitleListRepository : ITitleListRepository
    //{
    //    private List<TitleModel> _titleList;
    //    public TitleListRepository()
    //    {
    //        _titleList = new List<TitleModel>()
    //        { 

    //        }
    //    }
    //    public IEnumerable<TitleModel> GetTitleList()
    //    {
    //        return _titleList;
    //    }
    //    public TitleModel GetTitle(int TitleID)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    //public IEnumerable<TitleModel> GetTitleList()
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}
}