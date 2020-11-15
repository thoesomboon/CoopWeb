using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.Repository
{
    public interface ICoopControlRepository : IRepository<CoopControl>
    {
        IQueryable<CoopControlModel> ReadDetail();
        IQueryable<CoopControlModel> ReadDetail(int CopID);
        bool Update(CoopControlModel model);
        bool UpdateReceiptNo(CoopControlModel model);
    }
    public class CoopControlRepository : Repository<CoopControl>, ICoopControlRepository
    {
        public CoopControlRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<CoopControlModel> ReadDetail()
        {
            var CoopControl = from m in Read()
                      select new CoopControlModel
                      {
                          Filestatus = m.Filestatus,
                          CoopID = m.CoopID,
                          CoopName = m.CoopName,
                          Address = m.Address,
                          Province = m.Province,
                          PostalCode = m.PostalCode,
                          Telephone = m.Telephone,
                          Fax = m.Fax,
                          PrevBudgetYear = m.PrevBudgetYear,
                          BudgetYear = m.BudgetYear,
                          AccountPeriod = m.AccountPeriod,
                          SystemLogin = m.SystemLogin,
                          PrevSystemDate = m.PrevSystemDate,
                          SystemDate = m.SystemDate,
                          NextSystemDate = m.NextSystemDate,
                          StartBudgetDate = m.StartBudgetDate,
                          EndBudgetDate = m.EndBudgetDate,
                          PrevStartBudgetDate = m.PrevStartBudgetDate,
                          PrevEndBudgetDate = m.PrevEndBudgetDate,
                          PrevMthProcDate = m.PrevMthProcDate,
                          ThisMthProcDate = m.ThisMthProcDate,
                          NextMthProcDate = m.NextMthProcDate,
                          MaskMemberId = m.MaskMemberId,
                          ShareBookValue = m.ShareBookValue,
                          DaysINYear = m.DaysINYear,
                          RoundIntMethod = m.RoundIntMethod,
                          ManagerName = m.ManagerName,
                          LastReceiptBookNo = m.LastReceiptBookNo,
                          LastReceiptRunNo = m.LastReceiptRunNo
                      };
            return CoopControl;
        }

        public IQueryable<CoopControlModel> ReadDetail(int CopID)
        {
            var Coop = ReadDetail().Where(m => m.CoopID == CopID);

            return Coop;
        }
        public bool DayClose(CoopControlModel model)
        {
            var uCoop = (from c in Read()
                         where c.CoopID == model.CoopID
                         select c).FirstOrDefault();
            if (uCoop == null) { return false; }
            uCoop.PrevSystemDate = model.PrevSystemDate;
            uCoop.SystemDate = model.SystemDate;
            uCoop.NextSystemDate = model.NextSystemDate;

            uCoop.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            uCoop.ModifiedDate = System.DateTime.Now;

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
        public bool Update(CoopControlModel model)
        {
            var uCoop = (from c in Read()
                        where c.CoopID == model.CoopID
                        select c).FirstOrDefault();
            if (uCoop == null) { return false; }
                uCoop.Filestatus = model.Filestatus;
                uCoop.CoopID = model.CoopID;
                uCoop.CoopName = model.CoopName;
                uCoop.Address = model.Address;
                uCoop.Province = model.Province;
                uCoop.PostalCode = model.PostalCode;
                uCoop.Telephone = model.Telephone;
                uCoop.Fax = model.Fax;
                uCoop.PrevBudgetYear = model.PrevBudgetYear;
                uCoop.BudgetYear = model.BudgetYear;
                uCoop.AccountPeriod = model.AccountPeriod;
                //uCoop.SystemLogin = model.SystemLogin;
                uCoop.PrevSystemDate = model.PrevSystemDate;
                uCoop.SystemDate = model.SystemDate;
                uCoop.NextSystemDate = model.NextSystemDate;
                uCoop.StartBudgetDate = model.StartBudgetDate;
                uCoop.EndBudgetDate = model.EndBudgetDate;
                uCoop.PrevStartBudgetDate = model.PrevStartBudgetDate;
                uCoop.PrevEndBudgetDate = model.PrevEndBudgetDate;
                uCoop.PrevMthProcDate = model.PrevMthProcDate;
                uCoop.ThisMthProcDate = model.ThisMthProcDate;
                uCoop.NextMthProcDate = model.NextMthProcDate;
                uCoop.MaskMemberId = model.MaskMemberId;
                uCoop.ShareBookValue = model.ShareBookValue;
                uCoop.DaysINYear = model.DaysINYear;
                uCoop.RoundIntMethod = model.RoundIntMethod;
                uCoop.ManagerName = model.ManagerName;
                uCoop.LastReceiptBookNo = model.LastReceiptBookNo;
                uCoop.LastReceiptRunNo = model.LastReceiptRunNo;

                uCoop.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                uCoop.ModifiedDate = System.DateTime.Now;
                        
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

        public bool UpdateReceiptNo(CoopControlModel model)
        {
            var uCoop = (from c in Read()
                         where c.CoopID == model.CoopID
                         select c).FirstOrDefault();
            if (uCoop == null) { return false; }
            uCoop.Filestatus = model.Filestatus;
            uCoop.CoopID = model.CoopID;
            uCoop.LastReceiptBookNo = model.LastReceiptBookNo;
            uCoop.LastReceiptRunNo = model.LastReceiptRunNo;
            uCoop.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            uCoop.ModifiedDate = System.DateTime.Now;

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
