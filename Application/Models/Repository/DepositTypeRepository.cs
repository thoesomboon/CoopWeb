using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Linq;
//using Coop.Models.Repositories;

namespace Coop.Models.Repository
{
    public interface IDepositTypeRepository : IRepository<DepositType>
    {
        IQueryable<DepositTypeModel> ReadDetail();
        IQueryable<DepositTypeModel> ReadDetail(String depositTypeID);
        bool Update(DepositTypeModel model);
        bool UpdateIssueAccNo(DepositTypeModel model);
        //DepositTypeModel Create(DepositTypeModel mModel);
    }
    public class DepositTypeRepository : Repository<DepositType>, IDepositTypeRepository
    {
        public DepositTypeRepository(CoopWebEntities context) : base(context) { }
        public IQueryable<DepositTypeModel> ReadDetail()
        {
            var depositType = from d in Read()
                    select new DepositTypeModel
                    {
                        Filestatus = d.Filestatus,
                        CoopID = d.CoopID,
                        DepositTypeID = d.DepositTypeID,
                        DepositTypeName = d.DepositTypeName,
                        TypeOfDeposit = d.TypeOfDeposit,
                        MinOpenAmt = d.MinOpenAmt,
                        MaxOpenAmt = d.MaxOpenAmt,
                        MinDepAmt = d.MinDepAmt,
                        MaxDepAmt = d.MaxDepAmt,
                        MinWithdrawAmt = d.MinWithdrawAmt,
                        MaxWithdrawAmt = d.MaxWithdrawAmt,
                        MinLedgerBal = d.MinLedgerBal,
                        ItemStatus = d.ItemStatus,
                        //MonthDepAmtStatus = d.MonthDepAmtStatus,
                        WithdrawApplyStatus = d.WithdrawApplyStatus,
                        MonthMaxWithdrawAmt = d.MonthMaxWithdrawAmt,
                        MonthMaxWithdrawTimes = d.MonthMaxWithdrawTimes,
                        MaxChargeAmt = d.MaxChargeAmt,
                        MinChargeAmt = d.MinChargeAmt,
                        WithdrawChargePercent = d.WithdrawChargePercent,
                        MinBalCalcInt = d.MinBalCalcInt,
                        WithdrawStatus = d.WithdrawStatus,
                        MaskOfAccountNo = d.MaskOfAccountNo,
                        LastAccountNo = d.LastAccountNo,
                        LastBookNo = d.LastBookNo,
                        PostIntTxnCode = d.PostIntTxnCode,
                        CloseAccountFee = d.CloseAccountFee,
                        MonthIntDue = d.MonthIntDue,
                        CalcIntType = d.CalcIntType,
                        CalcIntRate = d.CalcIntRate,
                        StepCalcIntFlag = d.StepCalcIntFlag,
                        StepCalcIntRate = d.StepCalcIntRate,
                        StepCalcIntType = d.StepCalcIntType,
                        StepCalcIntRate3 = d.StepCalcIntRate3,
                        StepCalcIntType3 = d.StepCalcIntType3,
                        StepCalcIntRate6 = d.StepCalcIntRate6,
                        StepCalcIntType6 = d.StepCalcIntType6,
                        StepCalcIntRate9 = d.StepCalcIntRate9,
                        StepCalcIntType9 = d.StepCalcIntType9,
                        StepCalcIntRate12 = d.StepCalcIntRate12,
                        StepCalcIntType12 = d.StepCalcIntType12,
                        StepCalcIntRate15 = d.StepCalcIntRate15,
                        StepCalcIntType15 = d.StepCalcIntType15,
                        StepCalcIntRate18 = d.StepCalcIntRate18,
                        StepCalcIntType18 = d.StepCalcIntType18,
                        StepCalcIntRate21 = d.StepCalcIntRate21,
                        StepCalcIntType21 = d.StepCalcIntType21,
                        BatchIntDueDate1 = d.BatchIntDueDate1,
                        BatchIntDueDate2 = d.BatchIntDueDate2,
                        BatchIntDueDate3 = d.BatchIntDueDate3,
                        BatchIntDueDate4 = d.BatchIntDueDate4
                    };
            return depositType;
        }
        public IQueryable<DepositTypeModel> ReadDetail(String depTypeID)
        { 
            var depositType = ReadDetail().Where(d=>d.DepositTypeID == depTypeID);
            return depositType;
        }
        public bool Update(DepositTypeModel model)
        {
            var c = ModelHelper<DepositType>.Apply(model);
            c.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            c.ModifiedDate = System.DateTime.Now;
            Update(c);
            var result = _context.SaveChanges();
            return result > 0;
        }
        public bool UpdateIssueAccNo(DepositTypeModel model)
        {
            var data = (from depType in Read()
                        where depType.DepositTypeID == model.DepositTypeID
                        select depType).FirstOrDefault();
            if (data == null) { return false; }

            data.DepositTypeID = model.DepositTypeID;
            data.LastAccountNo = (model.LastAccountNo);
            data.LastBookNo = model.LastBookNo;
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
        public IQueryable<DepositTypeModel> ReadByID(String depositTypeID)
        {
            var depositType = ReadDetail().Where(d => d.DepositTypeID == depositTypeID);
            return depositType;
        }
    }
}