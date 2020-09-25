using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Linq;
//using Coop.Models.Repositories;
//using Coop.Models.Repository;
using System.Web;

namespace Coop.Models.Repository
{
    public interface INoCardRepository : IRepository<NoCard>
    {
        IQueryable<NoCardModel> ReadDetail();
        IQueryable<NoCardModel> ReadByID(string AccNo);
        IQueryable<NoCardModel> ReadBySeq(string AccNo, int Seq);
        void Create(NoCardModel model);
        void Update(NoCardModel model);
        bool LogNoCard(
                    string strAccountNo,
                    DateTime dtBackDate,
                    string strTxnCode,
                    string strCDCode,
                    double dblCFLedgerBal,
                    double dblTxnAmt,
                    double dblChqAmt,
                    int dblLastItemNo,
                    string strAbbCode,
                    double dblAccInt);
        //bool CreateLog(NoCardModel NoCardModel);
    }

    public class NoCardRepository : Repository<NoCard>, INoCardRepository
    {
        public NoCardRepository(CoopWebEntities context) : base(context) { }
        public IQueryable<NoCardModel> ReadDetail()
        {
            var nocard = from n in Read()
                         select new NoCardModel
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
                             TxnCode = n.TTxnCode,
                             AbbCode = n.AbbCode,
                             ItemNo = n.ItemNo,
                             CDCode = n.CDCode,
                             TxnAmt = n.TxnAmt,
                             CfLedgerBal = n.CfLedgerBal,
                             ChequeAmt = n.ChequeAmt,
                             Tax = n.Tax,
                             AccInt = n.AccInt
                         };
            return nocard;
        }
        public IQueryable<NoCardModel> ReadByID(string AccNo)
        {
            var nocard = ReadDetail().Where(n => n.AccountNo == AccNo);
            return nocard;
        }
        public IQueryable<NoCardModel> ReadBySeq(string AccNo, int Seq)
        {
            var nocard = ReadDetail().Where(n => n.AccountNo == AccNo || n.Seq == Seq);
            return nocard;
        }
        public void Create(NoCardModel model)
        {
            var c = ModelHelper<NoCard>.Apply(model);
            c.CreatedBy = AuthorizeHelper.Current.UserAccount().UserID;
            c.CreatedDate = DateTime.Now;
            ReadByCreate(c);
        }

        public void Update(NoCardModel model)
        {
            var c = ModelHelper<NoCard>.Apply(model);
            c.ModifiedDate = DateTime.Now;
            c.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            Update(c);
        }
        public bool LogNoCard(
            string strAccountNo,
            DateTime dtBackDate,
            string strTxnCode,
            string strCDCode,
            double dblCFLedgerBal,
            double dblTxnAmt,
            double dblChqAmt,
            int dblLastItemNo,
            string strAbbCode,
            double dblAccInt)
        {
            if (string.IsNullOrWhiteSpace(strAccountNo))
            {
                return false;
            }
            bool result = false;
            int coopId = AuthorizeHelper.Current.CoopControls().CoopID;
            System.DateTime systemDate = AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            int Seq = (from p in Read() where p.CoopID == coopId && p.AccountNo == strAccountNo select p.Seq).Max() + 1;

            NoCardModel noCardData = new NoCardModel
            {
                //create_user_id = userId,
                //create_by = DateTime.Now,
                //user_id = userId,
                //update_by = DateTime.Now,
                Filestatus = "A",
                CoopID = coopId,
                AccountNo = strAccountNo,
                Seq = Seq,
                TxnDate = systemDate,
                BackDate = dtBackDate,
                TxnCode = strTxnCode,
                AbbCode = strAbbCode,
                ItemNo = dblLastItemNo,
                CDCode = strCDCode,
                TxnAmt = (decimal)dblTxnAmt,
                ChequeAmt = (decimal)dblChqAmt,
                CfLedgerBal = (decimal)dblCFLedgerBal,
                AccInt = (decimal)dblAccInt
            };
            result = CreateLogNoCard(noCardData);
            return result;
        }

        public bool CreateLogNoCard(NoCardModel NoCardModel)
        {
            if (NoCardModel == null)
            {
                return false;
            }
            int userId = AuthorizeHelper.Current.UserAccount().UserID;

            NoCardModel noBookData = new NoCardModel
            {
                //CreatedBy = userId,
                CreatedDate = NoCardModel.CreatedDate ?? DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = NoCardModel.ModifiedDate ?? DateTime.Now,
                Filestatus = NoCardModel.Filestatus,
                CoopID = NoCardModel.CoopID,
                AccountNo = NoCardModel.AccountNo,
                Seq = NoCardModel.Seq == 0 ? NoCardModel.Seq + 1 : NoCardModel.Seq,
                TxnDate = NoCardModel.TxnDate,
                BackDate = NoCardModel.BackDate,
                TxnCode = NoCardModel.TTxnCode,
                AbbCode = NoCardModel.AbbCode,
                ItemNo = NoCardModel.ItemNo,
                CDCode = NoCardModel.CDCode,
                TxnAmt = NoCardModel.TxnAmt,
                ChequeAmt = NoCardModel.ChequeAmt,
                CfLedgerBal = NoCardModel.CfLedgerBal,
                AccInt = NoCardModel.AccInt
            };

            var c = ModelHelper<NoCard>.Apply(noBookData);
            Create(c);
            int result = _context.SaveChanges();

            return result > 0;
        }
    }
}