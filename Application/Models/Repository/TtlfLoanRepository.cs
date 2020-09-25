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
    public interface ITtlfLoanRepository : IRepository<TtlfLoan>
    {
        IQueryable<TtlfLoanModel> ReadDetail();
        IQueryable<TtlfLoanModel> ReadDetail(String LonID, System.DateTime startDate);
        IQueryable<TtlfLoanModel> ReadDetail(String LonID, System.DateTime startDate, System.DateTime endDate);
        IQueryable<TtlfLoanModel> ReadBySeqDesc(System.DateTime startDate);
        TtlfLoanModel LogTtlfLoan(TtlfLoanModel model);
    }

    public class TtlfLoanRepository : Repository<TtlfLoan>, ITtlfLoanRepository
    {
        public TtlfLoanRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<TtlfLoanModel> ReadDetail()
        {
            var ttlfLoan = from t in Read()
                              select new TtlfLoanModel
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
                                  LoanID = t.LoanID,
                                  BackDate = t.BackDate,
                                  BFBal = t.BFBal,
                                  Amt1 = t.Amt1,
                                  Amt2 = t.Amt2,
                                  IntCalc = t.IntCalc,
                                  IntAmt = t.IntAmt,
                                  ChargeCalc = t.ChargeCalc,
                                  ChargeAmt = t.ChargeAmt,
                                  PrincipleAmt = t.PrincipleAmt,
                                  UnpayInt = t.UnpayInt,
                                  UnpayCharge = t.UnpayCharge,
                                  BFInt = t.BFInt,
                                  BFCharge = t.BFCharge,
                                  PayFlag = t.PayFlag,
                                  OCFlag = t.OCFlag,
                                  AbbCode = t.AbbCode,
                                  TTxnCode = t.TTxnCode,
                                  CDCode = t.CDCode,
                                  ChequeNo = t.ChequeNo,
                                  RcptBookNo = t.RcptBookNo,
                                  RcptRunNo = t.RcptRunNo,
                                  ECFlag = t.ECFlag,
                                  OverrideID = t.OverrideID
                              };
            return ttlfLoan;
        }
        public IQueryable<TtlfLoanModel> ReadDetail(String LonID, System.DateTime startDate)
        {
            var ttlfLoan = ReadDetail().Where(t => t.LoanID == LonID || t.TxnDate >= startDate);
            return ttlfLoan;
        }
        public IQueryable<TtlfLoanModel> ReadDetail(String LonID, System.DateTime startDate, System.DateTime endDate)
        {
            var ttlfLoan = ReadDetail().Where(t => t.LoanID == LonID || t.TxnDate >= startDate || t.TxnDate <= endDate);
            return ttlfLoan;
        }
        public IQueryable<TtlfLoanModel> ReadBySeqDesc(System.DateTime startDate)
        {
            var ttlfLoan = ReadDetail().Where(t => t.TxnDate == startDate).OrderByDescending(t => t.TxnSeq);
            return ttlfLoan;
        }
        //public bool CreateLog(LogTtlnScoopModel model)
        //{
        //    var c = ModelHelper<ttlnscoop>.Apply(model);
        //    Create(c);
        //    var result = _context.SaveChanges();
        //    return result > 0;
        //}
        public TtlfLoanModel LogTtlfLoan(TtlfLoanModel model)
        {
            var ttlfLoan = ModelHelper<TtlfLoan>.Apply(model);
            return ModelHelper<TtlfLoanModel>.Apply(ReadByCreate(ttlfLoan));
        }
    }
}
