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
    public interface ILoanTypeRepository : IRepository<LoanType>
    {
        IQueryable<LoanTypeModel> ReadDetail();
        IQueryable<LoanTypeModel> ReadDetail(string LonID);
        bool NotActive(String LTypeID);
        LoanTypeModel Create(LoanTypeModel model);
        bool Update(LoanTypeModel model);
    }

    public class LoanTypeRepository : Repository<LoanType>, ILoanTypeRepository
    {
        public LoanTypeRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<LoanTypeModel> ReadDetail()
        {
            var LoanType = from l in Read()
                select new LoanTypeModel
                {
                    CreatedBy = l.CreatedBy,
                    CreatedDate = l.CreatedDate,
                    ModifiedBy = l.ModifiedBy,
                    ModifiedDate = l.ModifiedDate,
                    Filestatus = l.Filestatus,
                    CoopID = l.CoopID,
                    LoanTypeID = l.LoanTypeID,
                    LoanTypeName = l.LoanTypeName,
                    IntType = l.IntType,
                    NoMemberMonths = l.NoMemberMonths,
                    MinLoanAmt = l.MinLoanAmt,
                    MaxLoanAmt = l.MaxLoanAmt,
                    SecurityFlag = l.SecurityFlag,
                    CalcIntFlag = l.CalcIntFlag,
                    NoOfLoanYears = l.NoOfLoanYears,
                    LastLoanID = l.LastLoanID,
                    PrefixLoanID = l.PrefixLoanID,
                    LastRequestNo = l.LastRequestNo,
                    PrefixRequestNo = l.PrefixRequestNo,
                    ChargeRate = l.ChargeRate,
                    DiscIntRate = l.DiscIntRate,
                    DiscIntFlag = l.DiscIntFlag
                 };
            return LoanType;
        }
        public IQueryable<LoanTypeModel> ReadDetail(string LonTypeID)
        {
            var LoanType = ReadDetail().Where(l => l.LoanTypeID == LonTypeID);
            return LoanType;
        }
        public bool NotActive(String LTypeID)
        {
            var data = (from lType in Read()
                        where lType.LoanTypeID == LTypeID
                        select lType).FirstOrDefault();

            if (data == null) { return false; }

            data.Filestatus = "D";
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
        public LoanTypeModel Create(LoanTypeModel model)
        {
            LoanTypeModel cModel = new LoanTypeModel
            {
                Filestatus = model.Filestatus,
                CoopID = model.CoopID,
                LoanTypeID = model.LoanTypeID,
                LoanTypeName = model.LoanTypeName,
                PrefixLoanID = model.PrefixLoanID,
                MinLoanAmt = model.MinLoanAmt,
                MaxLoanAmt = model.MaxLoanAmt,
                IntType = model.IntType,
                LastLoanID = model.LastLoanID,
                LastRequestNo = model.LastRequestNo,
                ChargeRate = model.ChargeRate,
                DiscIntRate = model.DiscIntRate,

                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = DateTime.Now
            };
            var cLoan = ModelHelper<LoanType>.Apply(cModel);
            return ModelHelper<LoanTypeModel>.Apply(ReadByCreate(cLoan));
        }
        public bool Update(LoanTypeModel model)
        {
            var c = ModelHelper<LoanType>.Apply(model);
            c.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            c.ModifiedDate = System.DateTime.Now;
            Update(c);
            var result = _context.SaveChanges();
            return result > 0;
        }
    }
}
