using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Linq;
using System.Linq.Dynamic;
//using Coop.Models.Repositories;
//using Coop.Models.Repository;
using System.Web;

namespace Coop.Models.Repository
{
    public interface INoBookRepository : IRepository<NoBook>
    {
        IQueryable<NoBookModel> ReadDetail();
        IQueryable<NoBookModel> ReadByID(string AccNo);
        IQueryable<NoBookModel> ReadBySeqDesc(string AccNo);
        IQueryable<NoBookModel> ReadBySeq(string AccNo, int Seq);
        NoBookModel Create(NoBookModel model);
        bool Update(NoBookModel model);
        //NoBookModel LogNoBook(NoBookModel model);
        bool LogNoBook(string Status, int copID, string AccNo, int Seq, DateTime TDate, DateTime BDate, string TCode, string ACode, int INo, string CCode, decimal TAmt, decimal CAmt, decimal CfBal);
    }
    public class NoBookRepository : Repository<NoBook>, INoBookRepository
    {
        public NoBookRepository(CoopWebEntities context) : base(context) { }
        public IQueryable<NoBookModel> ReadDetail()
        {
            var nobook = from n in Read()
                            select new NoBookModel
                            {
                                //CreatedBy = n.CreatedBy,
                                //CreatedDate = n.CreatedDate,
                                //ModifiedBy = n.ModifiedBy,
                                //ModifiedDate = n.ModifiedDate,
                                Filestatus = n.Filestatus,
                                CoopID = n.CoopID,
                                AccountNo = n.AccountNo,
                                Seq = n.Seq,
                                TxnDate = n.TxnDate,
                                BackDate = n.BackDate,
                                TTxnCode = n.TTxnCode,
                                AbbCode = n.AbbCode,
                                ItemNo = n.ItemNo,
                                CDCode = n.CDCode,
                                TxnAmt = n.TxnAmt,
                                CfLedgerBal = n.CfLedgerBal,
                                ChequeAmt = n.ChequeAmt,
                                Tax = n.Tax
                            };
            return nobook;
        }
        public IQueryable<NoBookModel> ReadByID(string AccNo)
        {
            var nobook = ReadDetail().Where(n => n.AccountNo == AccNo).OrderBy(n=>n.Seq);
            return nobook;
        }
        public IQueryable<NoBookModel> ReadBySeqDesc(string AccNo)
        {
            var nobook = ReadDetail().Where(n => n.AccountNo == AccNo).OrderByDescending(n => n.Seq);
            return nobook;
        }
        public IQueryable<NoBookModel> ReadBySeq(string AccNo, int Seq)
        {
            var nobook = ReadDetail().Where(n => n.AccountNo == AccNo && n.Seq == Seq);
            return nobook;
        }
        public NoBookModel Create(NoBookModel nModel)
        {
            var nNoBook = ModelHelper<NoBook>.Apply(nModel);
            return ModelHelper<NoBookModel>.Apply(ReadByCreate(nNoBook));
        }
        public bool LogNoBook(string Status, int copID, string AccNo, int Seq, DateTime TDate, DateTime BDate, string TCode, string ACode, int INo, string CCode, decimal TAmt, decimal CAmt, decimal CfBal)
        {
            NoBookModel noBookData = new NoBookModel
            {
                Filestatus = Status,
                CoopID = copID,
                AccountNo = AccNo,
                Seq = Seq,
                TxnDate = TDate,
                BackDate = BDate,
                TTxnCode = TCode,
                AbbCode = ACode,
                ItemNo = INo,
                CDCode = CCode,
                TxnAmt = TAmt,
                ChequeAmt = CAmt,
                CfLedgerBal = CfBal,

                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = DateTime.Now
            };
            var cnoBook = ModelHelper<NoBook>.Apply(noBookData);
            int returnVal = 0;
            bool result = false;
            try
            {
                returnVal = _context.SaveChanges();
                result = returnVal > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool Update(NoBookModel model)
        {
            var data = (from dep in Read()
                        where dep.AccountNo == model.AccountNo
                        select dep).FirstOrDefault();
            if (data == null) { return false; }

            data.Filestatus = model.Filestatus;
            data.CoopID = model.CoopID;
            data.AccountNo = model.AccountNo;
            data.Seq = model.Seq;
            data.TxnDate = model.TxnDate;
            data.BackDate = model.BackDate;
            data.TTxnCode = model.TTxnCode;
            data.AbbCode = model.AbbCode;
            data.ItemNo = model.ItemNo;
            data.CDCode = model.CDCode;
            data.TxnAmt = model.TxnAmt;
            data.CfLedgerBal = model.CfLedgerBal;
            data.ChequeAmt = model.ChequeAmt;
            data.Tax = model.Tax;

            data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            data.ModifiedDate = System.DateTime.Now;

            int returnVal = 0;
            bool result = false;

            try
            {
                returnVal = _context.SaveChanges();
                result = returnVal > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        //public NoBookModel LogNoBook(NoBookModel nModel)
        //{
        //    if (nModel != null)
        //    {
        //        //int Seq = (from n in Read() where n.CoopID == coopId && n.AccountNo == strAccountNo).OrderByDescending(n => n.Seq).FirstOrDefault().Seq + 1;
        //        //int Seq = (from p in Read() where p.AccountNo == nModel.AccountNo select p.Seq).Max() + 1;
        //        NoBookModel noBookData = new NoBookModel
        //        {
        //            Filestatus = nModel.Filestatus,
        //            CoopID = nModel.CoopID,
        //            AccountNo = nModel.AccountNo,
        //            Seq = nModel.Seq,
        //            TxnDate = nModel.TxnDate,
        //            BackDate = nModel.BackDate,
        //            TTxnCode = nModel.TTxnCode,
        //            AbbCode = nModel.AbbCode,
        //            ItemNo = nModel.ItemNo,
        //            CDCode = nModel.CDCode,
        //            TxnAmt = nModel.TxnAmt,
        //            ChequeAmt = nModel.ChequeAmt,
        //            CfLedgerBal = nModel.CfLedgerBal,

        //            CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
        //            CreatedDate = nModel.CreatedDate ?? DateTime.Now,
        //            ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
        //            ModifiedDate = nModel.ModifiedDate ?? DateTime.Now
        //        };
        //        var cnoBook = ModelHelper<NoBook>.Apply(noBookData);
        //        return ModelHelper<NoBookModel>.Apply(ReadByCreate(cnoBook));      
        //    }
        //}

    }
}