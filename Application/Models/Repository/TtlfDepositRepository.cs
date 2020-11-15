using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Models.Repository;

namespace Coop.Models.Repository
{
    public interface ITtlfDepositRepository : IRepository<TtlfDeposit>
    {
        IQueryable<TtlfDepositModel> ReadDetail();
        IQueryable<TtlfDepositModel> ReadDetail(String accNo, System.DateTime startDate, System.DateTime endDate);
        IQueryable<TtlfDepositModel> ReadDetail(String accNo, System.DateTime startDate);
        TtlfDepositModel LogTtlfDeposit(TtlfDepositModel model);
        bool LogTtlfDesposit(int copID, DateTime sDate, int Seq, string oProcess, string status, string memID, string depTypeID, string AccNo, DateTime bDate,
            decimal bfBal, decimal cr, decimal dr, decimal cfBal, decimal aInt, decimal chgAmt, int iNo, bool bFlag, string refNo, string tCode, string cCode,
            string oFlag, string iType);
        IQueryable<TtlfDepositModel> ReadBySeqDesc(System.DateTime startDate);
    }
    public class TtlfDepositRepository : Repository<TtlfDeposit>, ITtlfDepositRepository
    {
        public TtlfDepositRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<TtlfDepositModel> ReadDetail()
        {
            var ttlfDep = from t in Read()
                          select new TtlfDepositModel
                          {
                              CoopID = t.CoopID,
                              TxnDate = t.TxnDate,
                              TxnSeq = t.TxnSeq,
                              TxnTime = t.TxnTime,
                              UserID = t.UserID,
                              WorkstationID = t.WorkstationID,
                              OriginalProcess = t.OriginalProcess,
                              Filestatus = t.Filestatus,
                              MemberID = t.MemberID,
                              DepositTypeID = t.DepositTypeID,
                              AccountNo = t.AccountNo,
                              BackDate = t.BackDate,
                              BFLedgerBal = t.BFLedgerBal,
                              Debit = t.Debit,
                              Credit = t.Credit,
                              ChequeAmt = t.ChequeAmt,
                              CFLedgerBal = t.CFLedgerBal,
                              Fee = t.Fee,
                              AccInt = t.AccInt,
                              ChargeAmt = t.ChargeAmt,
                              IntDueAmt = t.IntDueAmt,
                              Tax = t.Tax,
                              ItemNo = t.ItemNo,
                              BookFlag = t.BookFlag,
                              ReferenceNo = t.ReferenceNo,
                              BudgetYear = t.BudgetYear,
                              Type = t.Type,
                              TTxnCode = t.TTxnCode,
                              CDCode = t.CDCode,
                              InstrumentType = t.InstrumentType,
                              ECFlag = t.ECFlag,
                              BranchId = t.BranchId,
                              OverrideID = t.OverrideID,
                              ChequeDate = t.ChequeDate,
                              BankID = t.BankID,
                              ClearingFlag = t.ClearingFlag,
                              ClearingDate = t.ClearingDate,
                              OCFlag = t.OCFlag
                          };
            return ttlfDep;
        }
        public IQueryable<TtlfDepositModel> ReadDetail(String accNo, System.DateTime startDate, System.DateTime endDate)
        {
            var ttlfDep = ReadDetail().Where(t => t.AccountNo == accNo || t.TxnDate >= startDate || t.TxnDate <= endDate);
            return ttlfDep;
        }
        public IQueryable<TtlfDepositModel> ReadDetail(String accNo, System.DateTime startDate)
        {
            var ttlfDep = ReadDetail().Where(t => t.AccountNo == accNo && t.TxnDate >= startDate).OrderByDescending(t => t.TxnSeq);
            return ttlfDep;
        }
        //public IQueryable<TtlfDepositModel> DepositTxnSeq(System.DateTime startDate)
        //{
        //    var ttlfDeposit = ReadDetail().Where(t => t.TxnDate == startDate).OrderByDescending(t => t.TxnSeq);
        //    return ttlfDeposit;
        //}
        public IQueryable<TtlfDepositModel> ReadBySeqDesc(System.DateTime startDate)
        {
            var ttlfDeposit = ReadDetail().Where(t => t.TxnDate == startDate).OrderByDescending(t => t.TxnSeq);
            return ttlfDeposit;
        }
        public TtlfDepositModel LogTtlfDeposit(TtlfDepositModel model)
        {
            var ttlfDeposit = ModelHelper<TtlfDeposit>.Apply(model);
            return ModelHelper<TtlfDepositModel>.Apply(ReadByCreate(ttlfDeposit));
        }
        public bool LogTtlfDesposit(int copID, DateTime sDate, int Seq, string oProcess, string status, string memID, string depTypeID, string AccNo, DateTime bDate,
            decimal bfBal, decimal cr, decimal dr, decimal cfBal, decimal aInt, decimal chgAmt, int iNo, bool bFlag, string refNo, string tCode, string cCode,
            string oFlag, string iType)
        {
            TtlfDepositModel tModel = new TtlfDepositModel
            {
                CoopID = copID,
                TxnDate = sDate,
                TxnSeq = Seq,
                TxnTime = DateTime.Now,
                //WorkstationID = otxDepModel.WorkstationID,
                //BranchId = otxDepModel.BranchId,
                OriginalProcess = oProcess,
                Filestatus = status,
                MemberID = memID,
                DepositTypeID = depTypeID,
                AccountNo = AccNo,
                BackDate = bDate,
                BFLedgerBal = bfBal,
                Debit = dr,
                Credit = cr,
                CFLedgerBal = cfBal,
                //Fee = otxDepModel.Fee,
                AccInt = aInt,
                ChargeAmt = chgAmt,
                IntDueAmt = 0,
                //Tax = otxDepModel.Tax,
                ItemNo = iNo,
                BookFlag = bFlag,
                ReferenceNo = refNo,
                BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                Type = depTypeID,
                TTxnCode = tCode,
                CDCode = cCode,
                OCFlag = oFlag,
                InstrumentType = iType
            };
            var ttlfDeposit = ModelHelper<TtlfDeposit>.Apply(tModel);
            //return ModelHelper<TtlfDepositModel>.Apply(ReadByCreate(ttlfDeposit));
            //return ModelHelper<NoBookModel>.Apply(ReadByCreate(cnoBook));
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
    }
}
