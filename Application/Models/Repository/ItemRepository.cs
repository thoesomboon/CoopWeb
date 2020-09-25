using System;
using System.Linq;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
//using Coop.Library;
using System.Web;
using System.Collections.Generic;

namespace Coop.Models.Repository
{
    public interface IItemRepository : IRepository<Item>
    {
        IQueryable<ItemModel> ReadDetail();
        //IQueryable<ItemModel> ReadByID(string AccNo);
        //IQueryable<ItemModel> ReadByItem(string AccNo, int INo);
        //bool LogItem(
        //    string strAccountNo,
        //    System.DateTime txnDate,
        //    System.DateTime depositDate,
        //    System.DateTime dueDate,
        //    double depositAmt);
        //bool CreateLogItem(ItemModel itemData);
        //bool Update(ItemModel model);
        //bool Delete(ItemModel model);
        //bool Close(ItemModel model);
    }
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<ItemModel> ReadDetail()
        {
            var item = from i in Read()
                       select new ItemModel
                       {
                           CreatedBy = i.CreatedBy,
                           CreatedDate = i.CreatedDate,
                           ModifiedBy = i.ModifiedBy,
                           ModifiedDate = i.ModifiedDate,
                           Filestatus = i.Filestatus,
                           CoopID = i.CoopID,
                           AccountNo = i.AccountNo,
                           ItemNo = i.ItemNo,
                           TxnDate = i.TxnDate,
                           DepositDate = i.DepositDate,
                           DepositAmt = i.DepositAmt,
                           DepositBal = i.DepositBal,
                           DueDate = i.DueDate,
                           IntDueAmt = i.IntDueAmt,
                           Tax = i.Tax,
                           ItemNoNew = i.ItemNoNew,
                           TempAccInt = i.TempAccInt
                       };
            return item;
        }
        //public IQueryable<ItemModel> ReadByID(string AccNo)
        //{
        //    var item = ReadDetail().Where(i => i.AccountNo == AccNo);
        //    return item;
        //}
        //public IQueryable<ItemModel> ReadByItem(string AccNo, int INo)
        //{
        //    var item = ReadDetail().Where(i => i.AccountNo == AccNo || i.ItemNo == INo);
        //    return item;
        //}        
        //public bool LogItem(
        //    string strAccountNo,
        //    System.DateTime txnDate,
        //    System.DateTime depositDate,
        //    System.DateTime dueDate,
        //    double depositAmt)
        //{
        //    if (string.IsNullOrWhiteSpace(strAccountNo))
        //    {
        //        return false;
        //    }
        //    bool result = false;

        //    int coopId = AuthorizeHelper.Current.CoopControls().CoopID;
        //    System.DateTime systemDate = AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
        //    //int intScoopTxnSeq = (new UnitOfWork()).CoopControl.IntScoopTxnSeq();
        //    int userId = AuthorizeHelper.Current.UserAccount().UserID;
        //    int monthIntDue = (from t in _context.DepositType where t.CoopID == coopId && t.Filestatus == "A" select t.MonthIntDue).FirstOrDefault() ?? 0;
        //    txnDate = txnDate.Equals(default(DateTime)) ? systemDate : txnDate;
        //    depositDate = depositDate.Equals(default(DateTime)) ? txnDate : depositDate;
        //    dueDate = dueDate.Equals(default(DateTime)) ? depositDate.AddMonths(monthIntDue) : dueDate;
        //    int intItemNo = _context.Item.Where(i => i.CoopID == coopId && i.AccountNo == strAccountNo).OrderByDescending(i => i.ItemNo).FirstOrDefault().ItemNo;

        //    ItemModel itemData = new ItemModel
        //    {
        //        CreatedBy = userId,
        //        CreatedDate = System.DateTime.Now,
        //        ModifiedBy = userId,
        //        ModifiedDate = System.DateTime.Now,
        //        Filestatus = "A",
        //        CoopID = coopId,
        //        AccountNo = strAccountNo,
        //        ItemNo = intItemNo + 1,
        //        TxnDate = txnDate,
        //        DepositDate = depositDate,
        //        DueDate = dueDate,
        //        DepositAmt = (decimal)depositAmt,
        //        DepositBal = (decimal)depositAmt
        //    };

        //    result = CreateLogItem(itemData);
        //    return result;
        //}

        //public bool CreateLogItem(ItemModel itemData)
        //{
        //    ItemModel item = new ItemModel
        //    {
        //        CreatedBy = itemData.CreatedBy,
        //        CreatedDate = itemData.CreatedDate,
        //        ModifiedBy = itemData.ModifiedBy,
        //        ModifiedDate = itemData.ModifiedDate,
        //        Filestatus = itemData.Filestatus,
        //        CoopID = itemData.CoopID,
        //        AccountNo = itemData.AccountNo,
        //        ItemNo = itemData.ItemNo,
        //        TxnDate = itemData.TxnDate,
        //        DepositDate = itemData.DepositDate,
        //        DueDate = itemData.DueDate,
        //        DepositAmt = (decimal)itemData.DepositAmt,
        //        DepositBal = (decimal)itemData.DepositAmt
        //    };

        //    var c = ModelHelper<Item>.Apply(item);
        //    Create(c);
        //    int result = _context.SaveChanges();

        //    return result > 0;
        //}
        //public bool Update(ItemModel model)
        //{
        //    var data = (from it in Read()
        //                where it.AccountNo == model.AccountNo && it.ItemNo == model.ItemNo
        //                select it).FirstOrDefault();

        //    if (data == null) { return false; }

        //    data.AccountNo = model.AccountNo; /// primary key
        //    data.Filestatus = model.Filestatus;
        //    data.TxnDate = model.TxnDate;
        //    data.DepositDate = model.DepositDate;
        //    data.DepositAmt = model.DepositAmt;
        //    data.DepositBal = model.DepositBal;
        //    data.DueDate = model.DueDate;
        //    data.IntDueAmt = model.IntDueAmt;
        //    data.Tax = model.Tax;
        //    data.ItemNoNew = model.ItemNoNew;
        //    data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
        //    data.ModifiedDate = System.DateTime.Now;

        //    int returnVal = 0;
        //    bool result = false;

        //    try
        //    {
        //        returnVal = _context.SaveChanges();
        //        result = returnVal > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}

        //public bool Delete(ItemModel model)
        //{
        //    var data = (from it in Read()
        //                where it.AccountNo == model.AccountNo && it.ItemNo == model.ItemNo
        //                select it).FirstOrDefault();

        //    if (data == null) { return false; }

        //    data.AccountNo = model.AccountNo; /// primary key
        //    data.Filestatus = "D";
        //    data.TxnDate = model.TxnDate;
        //    data.DepositDate = model.DepositDate;
        //    data.DepositAmt = model.DepositAmt;
        //    data.DepositBal = model.DepositBal;
        //    data.DueDate = model.DueDate;
        //    data.IntDueAmt = model.IntDueAmt;
        //    data.Tax = model.Tax;
        //    data.ItemNoNew = model.ItemNoNew;
        //    data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
        //    data.ModifiedDate = System.DateTime.Now;

        //    int returnVal = 0;
        //    bool result = false;

        //    try
        //    {
        //        returnVal = _context.SaveChanges();
        //        result = returnVal > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}
        //public bool Close(ItemModel model)
        //{
        //    var data = (from it in Read()
        //                where it.AccountNo == model.AccountNo && it.ItemNo == model.ItemNo
        //                select it).FirstOrDefault();

        //    if (data == null) { return false; }

        //    data.AccountNo = model.AccountNo; /// primary key
        //    data.Filestatus = "C";
        //    data.TxnDate = model.TxnDate;
        //    data.DepositDate = model.DepositDate;
        //    data.DepositAmt = model.DepositAmt;
        //    data.DepositBal = model.DepositBal;
        //    data.DueDate = model.DueDate;
        //    data.IntDueAmt = model.IntDueAmt;
        //    data.Tax = model.Tax;
        //    data.ItemNoNew = model.ItemNoNew;
        //    data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
        //    data.ModifiedDate = System.DateTime.Now;

        //    int returnVal = 0;
        //    bool result = false;

        //    try
        //    {
        //        returnVal = _context.SaveChanges();
        //        result = returnVal > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}
    }
}
