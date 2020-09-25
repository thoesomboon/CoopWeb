using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Linq;
//using System.Collections.Generic;
//using System.Web;
//using System.Data.SqlClient;
//using System.Globalization;

namespace Coop.Models.Repository
{
    public interface IMemberRepository : IRepository<Member>
    {
        IQueryable<MemberModel> ReadDetail();
        IQueryable<MemberModel> ReadDetail(String memID);
        //void Create(MemberModel model);
        MemberModel Create(MemberModel model);
        bool Update(MemberModel model);
        MemberModel ReadMember(string memId);
        //DateTime DateInCE(string strDate);
    }
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<MemberModel> ReadDetail()
        {
            var member = from m in Read()
                      select new MemberModel
                      {
                          MemberID = m.MemberID,
                          TitleID = m.TitleID,
                          //TitleName = m.TitleName,
                          Name = m.Name,
                          Address = m.Address,
                          SubDistrictID = m.SubDistrictID,
                          DistrictID = m.DistrictID,
                          ProvinceID = m.ProvinceID,
                          PostalCode = m.PostalCode,
                          BirthDate = m.BirthDate,
                          ApplyDate = m.ApplyDate,
                          Salary = m.Salary,
                          Telephone = m.Telephone,
                          Mobile = m.Mobile,
                          EMail = m.EMail,
                          IdCard = m.IdCard,
                          LineID = m.LineID
                          //MemberStatusID = m.MemberStatus,
                          //MemberTypeID = m.MemberTypeID,
                          //CreatedBy = m.CreatedBy,
                          //CreatedDate = m.CreatedDate,
                          //ModifiedBy = m.ModifiedBy,
                          //ModifiedDate = m.ModifiedDate
                      };
            return member;
        }
        public IQueryable<MemberModel> ReadDetail(string memID)
        {
            var member = ReadDetail().Where(m => m.MemberID == memID);
            return member;
        }
        public MemberModel Create(MemberModel model)
        {
            MemberModel cModel = new MemberModel
            {
                MemberID = model.MemberID, //identity
                TitleID = model.TitleID,
                Name = model.Name,
                Address = model.Address,
                SubDistrictID = model.SubDistrictID,
                DistrictID = model.DistrictID,
                ProvinceID = model.ProvinceID,
                PostalCode = model.PostalCode,
                BirthDate = model.BirthDate,
                ResignDate = model.ResignDate,
                ApplyDate = model.ApplyDate,
                Salary = model.Salary,
                Telephone = model.Telephone,
                Mobile = model.Mobile,
                EMail = model.EMail,
                IdCard = model.IdCard,
                LineID = model.LineID,

                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = System.DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = System.DateTime.Now

            };
            var cMember = ModelHelper<Member>.Apply(cModel);
            return ModelHelper<MemberModel>.Apply(ReadByCreate(cMember));
        }
        public bool Update(MemberModel model)
        {
            var data = (from mem in Read()
                        where mem.MemberID == model.MemberID
                        select mem).FirstOrDefault();
            if (data == null) { return false; }

                data.MemberID = model.MemberID; /// primary key
                data.TitleID = model.TitleID;
                data.Name = model.Name;
                data.Address = model.Address;
                data.SubDistrictID = model.SubDistrictID;
                data.DistrictID = model.DistrictID;
                data.ProvinceID = model.ProvinceID;
                data.PostalCode = model.PostalCode;
                data.BirthDate = model.BirthDate;
                data.ApplyDate = model.ApplyDate;
                data.ResignDate = model.ResignDate;
                data.Salary = model.Salary;
                data.Telephone = model.Telephone;
                data.Mobile = model.Mobile;
                data.EMail = model.EMail;
                data.IdCard = model.IdCard;
                data.LineID = model.LineID;

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
        public MemberModel ReadMember(string memId)
        {
            var q = from m in Read()
                    join t in _context.Title on m.TitleID equals t.TitleID
                    join mt in _context.MemberType on m.MemberTypeID equals mt.MemberTypeID //into jtm from kjtm in jtm.DefaultIfEmpty()
                    join mg in _context.MemberGroup on m.MemberGroupID equals mg.MemberGroupID //into jtr from jjtr in jtr.DefaultIfEmpty()
                    join sd in _context.SubDistrict on m.SubDistrictID equals sd.SubDistrictID //into sd from mmmg in sd.DefaultIfEmpty()
                    join d in _context.District on m.DistrictID equals d.DistrictID //into sd from mmmg in sd.DefaultIfEmpty()
                    join p in _context.Province on m.ProvinceID equals p.ProvinceID //into sd from mmmg in sd.DefaultIfEmpty()
                    where m.MemberID == memId //&& m.CoopID == coopId
                    select new MemberModel
                    {
                        Filestatus = m.Filestatus,
                        CoopID = m.CoopID,
                        MemberID = m.MemberID,
                        TitleID = m.TitleID,
                        TitleName = t.TitleName,
                        Name = m.Name,
                        Address = m.Address,
                        SubDistrictID = m.SubDistrictID,
                        SubDistrictName = sd.SubDistrictName,
                        DistrictID = m.DistrictID,
                        DistrictName = d.DistrictName,
                        ProvinceID = m.ProvinceID,
                        ProvinceName = p.ProvinceName,
                        PostalCode = m.PostalCode,
                        Telephone = m.Telephone,
                        Mobile = m.Mobile,
                        EMail = m.EMail,
                        IdCard = m.IdCard,
                        LineID = m.LineID,
                        BirthDate = m.BirthDate,
                        //BirthDateTH = m.BirthDate,
                        ApplyDate = m.ApplyDate,
                        //ApplyDateTH = m.ApplyDateTH,
                        ResignDate = m.ResignDate,
                        //ResignDateTH = m.ResignDateTH,
                        Salary = m.Salary,
                        MemberGroupID = m.MemberGroupID,
                        MemberGroupName = mg.MemberGroupName,
                        MemberTypeID = m.MemberTypeID,
                        MemberTypeName = mt.MemberTypeName
                    };
            return q.FirstOrDefault();
        }
        //public DateTime DateInCE(string strDate)
        //{
        //    if (string.IsNullOrEmpty(strDate) || strDate.Length != 10)
        //    {
        //        return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
        //    }
        //    var intDT = strDate.ToString().Replace("12:00:00 AM", "");
        //    var year = Convert.ToInt32((intDT.Split('/')[2])) - 543;
        //    //var year = Convert.ToInt32((intDT.Split('/')[2])) - 543;
        //    var month = Convert.ToInt32(intDT.Split('/')[1]);
        //    var day = Convert.ToInt32(intDT.Split('/')[0]);
        //    var date = new DateTime(year, month, day);
        //    return date;
        //}
    }
}