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
    public interface ITxnCodeRepository : IRepository<TxnCode>
    {
        IQueryable<TxnCodeModel> ReadDetail();
        //IQueryable<TxnCodeModel> ReadDetail(String tType, String tCode);
        IQueryable<TxnCodeModel> ReadDetail(String tCode, String progName);
        IQueryable<TxnCodeModel> ReadDetailByType(String tCode, String tType);
    }

    public class TxnCodeRepository : Repository<TxnCode>, ITxnCodeRepository
    {
        public TxnCodeRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<TxnCodeModel> ReadDetail()
        {
            var TxnCode = from t in Read()
                           select new TxnCodeModel
                           {
                               CreatedBy = t.CreatedBy,
                               CreatedDate = t.CreatedDate,
                               ModifiedBy = t.ModifiedBy,
                               ModifiedDate = t.ModifiedDate,
                               Filestatus = t.Filestatus,
                               CoopID = t.CoopID,
                               TxnType = t.TxnType,
                               TTxnCode = t.TTxnCode,
                               Descript = t.Descript,
                               AbbCode = t.AbbCode,
                               NBKAbbCode = t.NBKAbbCode,
                               CDCode = t.CDCode,
                               OCFlag = t.OCFlag,
                               ECFlag = t.ECFlag,
                               InstrumentType = t.InstrumentType,
                               TellerLevel = t.TellerLevel,
                               OverrideLevel = t.OverrideLevel,
                               ECOverrideLevel = t.ECOverrideLevel,
                               ProgramName = t.ProgramName,
                               AddDeleteFlag = t.AddDeleteFlag,
                               PrintSlip = t.PrintSlip,
                               CreditPostGLLedgerNoDR = t.CreditPostGLLedgerNoDR,
                               CreditPostGLLedgerNoCR = t.CreditPostGLLedgerNoCR,
                               DebitPostGLLedgerNoDR = t.DebitPostGLLedgerNoDR,
                               DebitPostGLLedgerNoCR = t.DebitPostGLLedgerNoCR,
                               FeePostGLLedgerNoDR = t.FeePostGLLedgerNoDR,
                               FeePostGLLedgerNoCR = t.FeePostGLLedgerNoCR,
                               ChequeCreditPostGLLedgerNoDR = t.ChequeCreditPostGLLedgerNoDR,
                               ChequeCreditPostGLLedgerNoCR = t.ChequeCreditPostGLLedgerNoCR,
                               ChequeDebitPostGLLedgerNoDR = t.ChequeDebitPostGLLedgerNoDR,
                               ChequeDebitPostGLLedgerNoCR = t.ChequeDebitPostGLLedgerNoCR,
                               IntPostGLLedgerNoDR = t.IntPostGLLedgerNoDR,
                               IntPostGLLedgerNoCR = t.IntPostGLLedgerNoCR,
                               IntCalcFlag = t.IntCalcFlag,
                               SlipID = t.SlipID,
                               IsActive = t.IsActive
        };
            return TxnCode;
        }
        //public IQueryable<TxnCodeModel> ReadDetail(String tType, String tCode)
        //{
        //    var txnCode = ReadDetail().Where(t => t.TxnType == tType || t.TTxnCode == tCode);
        //    return txnCode;
        //}
        public IQueryable<TxnCodeModel> ReadDetail(String tCode, String progName)
        {
            var txnCode = ReadDetail().Where(t => t.TTxnCode == tCode && t.ProgramName == progName);
            return txnCode;
        }
        public IQueryable<TxnCodeModel> ReadDetailByType(String tType, String tCode)
        {
            var txnCode = ReadDetail().Where(t => t.TTxnCode == tCode && t.TxnType == tType);
            return txnCode;
        }
    }
}
