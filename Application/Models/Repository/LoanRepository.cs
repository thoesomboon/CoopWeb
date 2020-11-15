using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Models.Repository;
//using Coop.Library;
//using Coop.Infrastructure.Helpers;

using System.Data.SqlClient;
using System.Data;
using System.Data.Objects;

namespace Coop.Models.Repository
{
    public interface ILoanRepository : IRepository<Loan>
    {
        IQueryable<LoanModel> ReadDetail();
        IQueryable<LoanModel> ReadDetail(string LonID);
        LoanModel Create(LoanModel model);
        bool Update(LoanModel model);
        bool UpdateOtxLoan(LoanModel model);
        LoanModel ReadLoanInfo(string LonId);
        TransactionResultModel Sp_BatMthLoanBal(int coopId, int userID, string BudgetYear, int Period);
        TransactionResultModel Sp_BatPeriodCalcChargeAmt(int coopId, DateTime calcDate, int userID, string workID);
        TransactionResultModel sp_BatTrfMilk2Loan(int copId, DateTime calcDate, int userID, string workID);
        TransactionResultModel Sp_BatYrLoanUnpayInt(int copId, DateTime calcDate);
        TransactionResultModel Sp_BatYrLoanBal(int coopId, int userID, string BudgetYear, int Period1, int Period2);
    }

    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        public LoanRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<LoanModel> ReadDetail()
        {
            var loan = from l in Read()
                select new LoanModel
                {
                    Filestatus = l.Filestatus,
                    CoopID = l.CoopID,
                    LoanID = l.LoanID,
                    LoanTypeID = l.LoanTypeID,
                    LoanDate = l.LoanDate,
                    //FirstInstallDate = l.FirstInstallDate,
                    LastContact = l.LastContact,
                    StartCalcInt = l.StartCalcInt,
                    LastCalcInt = l.LastCalcInt,
                    LastCalcCharge = l.LastCalcCharge,
                    IntType = l.IntType,
                    LoanAmt = l.LoanAmt,
                    BFBal = l.BFBal,
                    LoanBal = l.LoanBal,
                    BFInt = l.BFInt,
                    BFCharge = l.BFCharge,
                    AccInt = l.AccInt,
                    YTDAccInt = l.YTDAccInt,
                    UnpayInt = l.UnpayInt,
                    UnpayPrinciple = l.UnpayPrinciple,
                    UnpayCharge = l.UnpayCharge,
                    IntRate = l.IntRate,
                    ReasonID = l.ReasonID,
                    BFUnpayInt = l.BFUnpayInt,
                    BFUnpayCharge = l.BFUnpayCharge,
                    InstallAmt = l.InstallAmt,
                    InstallMethodID = l.InstallMethodID,
                    DiscIntFlag = l.DiscIntFlag,
                    BFDiscInt = l.BFDiscInt,
                    DiscInt = l.DiscInt,
                    BFUnpayDiscInt = l.BFUnpayDiscInt,
                    UnpayDiscInt = l.UnpayDiscInt
                    };
            return loan;
        }
        public IQueryable<LoanModel> ReadDetail(string LonID)
        {
            var loan = ReadDetail().Where(l => l.LoanID == LonID);
            return loan;
        }
        public LoanModel Create(LoanModel model)
        {
            LoanModel cModel = new LoanModel
            {
                Filestatus = model.Filestatus,
                CoopID = model.CoopID,
                LoanID = model.LoanID,
                LoanTypeID = model.LoanTypeID,
                IntType = model.IntType,
                LoanAmt = model.LoanAmt,
                LoanBal = model.LoanBal,
                BFBal = model.BFBal,
                BFInt = model.BFInt,
                BFCharge = model.BFCharge,
                AccInt = model.AccInt,
                YTDAccInt = model.YTDAccInt,
                UnpayInt = model.UnpayInt,
                UnpayPrinciple = model.UnpayPrinciple,
                UnpayCharge = model.UnpayCharge,
                IntRate = model.IntRate,
                ReasonID = model.ReasonID,
                PayFlag = model.PayFlag,
                BFUnpayInt = model.BFUnpayInt,
                BFUnpayCharge = model.BFUnpayCharge,
                InstallAmt = model.InstallAmt,
                InstallMethodID = model.InstallMethodID,
                DiscIntFlag = model.DiscIntFlag,
                BFDiscInt = model.BFDiscInt,
                DiscInt = model.DiscInt,
                BFUnpayDiscInt = model.BFUnpayDiscInt,
                UnpayDiscInt = model.UnpayDiscInt,
                TmpUnpayInt = model.TmpUnpayInt,
                TmpUnpayPrinciple = model.TmpUnpayPrinciple,
                TmpUnpayCharge = model.TmpUnpayCharge,
                TmpDiscInt = model.TmpDiscInt,
                TmpMilkAmt = model.TmpMilkAmt,

                LoanDate = model.LoanDate,
                //FirstInstallDate = model.FirstInstallDate,
                LastContact = model.LastContact,
                StartCalcInt = model.StartCalcInt,
                LastCalcInt = model.LastCalcInt,
                LastCalcCharge = model.LastCalcCharge,

                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = DateTime.Now
            };
            var cLoan = ModelHelper<Loan>.Apply(cModel);
            return ModelHelper<LoanModel>.Apply(ReadByCreate(cLoan));
        }
        public bool Update(LoanModel model)
        {
            var data = (from lon in Read()
                        where lon.LoanID == model.LoanID
                        select lon).FirstOrDefault();
            if (data == null) { return false; }

                data.Filestatus = model.Filestatus;
                data.CoopID = model.CoopID;
                data.LoanID = model.LoanID;
                data.MemberID = model.MemberID;
                data.LoanTypeID = model.LoanTypeID;
                data.LoanDate = model.LoanDate;
                //data.FirstInstallDate = model.FirstInstallDate;
                data.LastContact = model.LastContact;
                data.StartCalcInt = model.StartCalcInt;
                data.LastCalcInt = model.LastCalcInt;
                data.LastCalcCharge = model.LastCalcCharge;
                data.IntType = model.IntType;
                data.LoanAmt = model.LoanAmt;
                data.LoanBal = model.LoanBal;
                data.BFBal = model.BFBal;
                data.BFInt = model.BFInt;
                data.BFCharge = model.BFCharge;
                data.AccInt = model.AccInt;
                data.YTDAccInt = model.YTDAccInt;
                data.UnpayInt = model.UnpayInt;
                data.UnpayPrinciple = model.UnpayPrinciple;
                data.UnpayCharge = model.UnpayCharge;
                data.IntRate = model.IntRate;
                data.ReasonID = model.ReasonID;
                data.BFUnpayInt = model.BFUnpayInt;
                data.BFUnpayCharge = model.BFUnpayCharge;
                data.InstallAmt = model.InstallAmt;
                data.InstallMethodID = model.InstallMethodID;
                data.DiscIntFlag = model.DiscIntFlag;
                data.BFDiscInt = model.BFDiscInt;
                data.DiscInt = model.DiscInt;
                data.BFUnpayDiscInt = model.BFUnpayDiscInt;
                data.UnpayDiscInt = model.UnpayDiscInt;

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
        public bool UpdateOtxLoan(LoanModel model)
        {
            var data = (from lon in Read()
                        where lon.LoanID == model.LoanID
                        select lon).FirstOrDefault();
            if (data == null) { return false; }

                data.Filestatus = model.Filestatus;
                data.CoopID = model.CoopID;
                data.LoanID = model.LoanID;
                data.LastContact = model.LastContact;
                data.LastCalcInt = model.LastCalcInt;
                data.LastCalcCharge = model.LastCalcCharge;
                data.BFBal = model.BFBal;
                data.UnpayPrinciple = model.UnpayPrinciple;
                data.LoanBal = model.LoanBal;
                data.BFInt = model.BFInt;
                data.UnpayInt = model.UnpayInt;
                data.AccInt = model.AccInt;
                data.YTDAccInt = model.YTDAccInt;
                data.BFCharge = model.BFCharge;
                data.UnpayCharge = model.UnpayCharge;
                data.DiscInt = model.DiscInt;
                data.UnpayDiscInt = model.UnpayDiscInt;

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
        public LoanModel ReadLoanInfo(string LonId)
        {
            var q = from l in Read()
                    join m in _context.Member on l.MemberID equals m.MemberID
                    join lt in _context.LoanType on l.LoanTypeID equals lt.LoanTypeID
                    join i in _context.InstallMethod on l.InstallMethodID equals i.InstallMethodID
                    join r in _context.Reason on l.ReasonID equals r.ReasonID
                    where l.LoanID == LonId //&& m.CoopID == coopId
                    select new LoanModel
                    {
                        Filestatus = l.Filestatus,
                        CoopID = l.CoopID,
                        LoanID = l.LoanID,
                        LoanTypeID = l.LoanTypeID,
                        LoanTypeName = lt.LoanTypeName,
                        MemberID = l.MemberID,
                        Name = m.Name,
                        LoanDate = l.LoanDate,
                        //FirstInstallDate = l.FirstInstallDate,
                        LastContact = l.LastContact,
                        StartCalcInt = l.StartCalcInt,
                        LastCalcInt = l.LastCalcInt,
                        LastCalcCharge = l.LastCalcCharge,
                        IntType = l.IntType,
                        LoanAmt = l.LoanAmt,
                        LoanBal = l.LoanBal,
                        BFBal = l.BFBal,
                        BFInt = l.BFInt,
                        BFCharge = l.BFCharge,
                        AccInt = l.AccInt,
                        YTDAccInt = l.YTDAccInt,
                        UnpayInt = l.UnpayInt,
                        UnpayPrinciple = l.UnpayPrinciple,
                        UnpayCharge = l.UnpayCharge,
                        IntRate = l.IntRate,
                        ReasonID = l.ReasonID,
                        ReasonName = r.ReasonName,
                        BFUnpayInt = l.BFUnpayInt,
                        BFUnpayCharge = l.BFUnpayCharge,
                        InstallAmt = l.InstallAmt,
                        InstallMethodID = l.InstallMethodID,
                        InstallMethodName = i.InstallMethodName,
                        DiscIntFlag = l.DiscIntFlag,
                        BFDiscInt = l.BFDiscInt,
                        DiscInt = l.DiscInt,
                        BFUnpayDiscInt = l.BFUnpayDiscInt,
                        UnpayDiscInt = l.UnpayDiscInt
                    };
            return q.FirstOrDefault();
        }
        //TransactionResultModel Sp_BatMthLoanBal(int coopId, int userID, string BudgetYear, int Period);
        //TransactionResultModel Sp_BatPeriodCalcChargeAmt(int coopId, DateTime calcDate, int userID, string workID);

        public TransactionResultModel Sp_BatMthLoanBal(int coopId, int userID, string BudgetYear, int Period)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;
            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatMthLoanBal] @CoopID, @UserID, @BudgetYear, @Period"
                    , new SqlParameter("@CoopID", coopId)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@BudgetYear", BudgetYear)
                    , new SqlParameter("@Period", Period)).FirstOrDefault();
            return transactionResult;
        }
        public TransactionResultModel Sp_BatYrLoanBal(int coopId, int userID, string BudgetYear, int Period1, int Period2)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;
            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatYrLoanBal] @CoopID, @UserID, @BudgetYear, @Period1, @Period2"
                    , new SqlParameter("@CoopID", coopId)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@BudgetYear", BudgetYear)
                    , new SqlParameter("@Period1", Period1)
                    , new SqlParameter("@Period2", Period2)).FirstOrDefault();
            return transactionResult;
        }
        public TransactionResultModel Sp_BatPeriodCalcChargeAmt(int coopId, DateTime calcDate, int userID, string workID)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;
            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatPeriodCalcChargeAmt] @CoopID, @CalcDate, @UserID, @WorkStationId"
                    , new SqlParameter("@CoopID", coopId)
                    , new SqlParameter("@CalcDate", calcDate)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@WorkStationId", workID)).FirstOrDefault();
            return transactionResult;
        }
        public TransactionResultModel sp_BatTrfMilk2Loan(int copId, DateTime calcDate, int userID, string workID)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;

            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatTrfMilk2Loan] @CoopId, @CalcDate, @UserID, @WorkStationId"
                    , new SqlParameter("@CoopID", copId)
                    , new SqlParameter("@CalcDate", calcDate)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@WorkStationId", workID)).FirstOrDefault();
            return transactionResult;
        }
        public TransactionResultModel Sp_BatYrLoanUnpayInt(int copId, DateTime calcDate)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;

            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatYrLoanUnpayInt] @CoopId, @CalcDate"
                    , new SqlParameter("@CoopID", copId)
                    , new SqlParameter("@CalcDate", calcDate)).FirstOrDefault();
            return transactionResult;
        }
    }
}
