using Coop.Entities;
using Coop.Models.Repository;
//using Coop.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Globalization;
using System.Data.SqlClient;

namespace Coop.Entities
{
    #region UnitOfwork
    public interface IUnitOfWork
    {
        int Save();
        IAccessPermissionRepository AccessPermission { get; }
        IAccessTransactionRepository AccessTransaction { get; }
        IAccountPeriodRepository AccountPeriod { get; }
        ICoopControlRepository CoopControl { get; }
        IDepositRepository Deposit { get; }
        IDepositTypeRepository DepositType { get; }
        IDistrictRepository District { get; }
        IInterestRepository Interest { get; }
        //IItemRepository Item { get; }
        ILoanRepository Loan { get; }
        ILoanDueRepository LoanDue { get; }
        ILoanTypeRepository LoanType { get; }
        IMemberRepository Member { get; }
        IMemberGroupRepository MemberGroup { get; }
        IMemberTypeRepository MemberType { get; }
        IModuleCategoriesRepository ModuleCategories { get; }
        IModuleRepository Modules { get; }
        IMonthBalanceDepositRepository MonthBalanceDeposit { get; }
        IMonthBalanceLoanRepository MonthBalanceLoan { get; }
        INoBookRepository NoBook { get; }
        //INoCardRepository NoCard { get; }
        IProvinceRepository Province { get; }
        IReasonRepository Reason { get; }
        IRequestLoanDueRepository RequestDue { get; }
        IRequestLoanRepository RequestLoan { get; }
        ISecurityRepository Security { get; }
        ISubDistrictRepository SubDistrict { get; }
        ITitleRepository Title { get; }
        ITtlfDepositRepository TtlfDeposit { get; }
        ITtlfLoanRepository TtlfLoan { get; }
        ITxnCodeRepository TxnCode { get; }
        IUserRepository Users { get; }
        IUserTypeRepository UserTypes { get; }
        IYearBalanceDepositRepository YearBalanceDeposit { get; }
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public CoopWebEntities _context; 
        private bool _disposed;
        public UnitOfWork()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["nDatabaseEntities"].ConnectionString;
            //_context = new nDatabaseEntities(connectionString);

            string connectionString = ConfigurationManager.ConnectionStrings["CoopWebEntities"].ConnectionString;
            //_context = new CoopWebEntities(connectionString);
            _context = new CoopWebEntities();
        }
        #region Generic Method
        public int Save()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed && disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
            _disposed = true;
        }

        #endregion

        #region Private Repository
        private AccessPermissionRepository _accessPermission;
        private AccessTransactionRepository _accessTransaction;
        private AccountPeriodRepository _accountPeriod;
        private CoopControlRepository _coopControl;
        private DepositRepository _deposit;
        private DepositTypeRepository _depositType;
        private DistrictRepository _district;
        //private ItemRepository _item;
        private InterestRepository _interest;
        private LoanRepository _loan;
        private LoanDueRepository _loanDue;
        private LoanTypeRepository _loanType;
        private MemberRepository _member;
        private MemberGroupRepository _memberGroup;
        private MemberTypeRepository _memberType;
        private ModuleCategoriesRepository _moduleCategories;
        private ModuleRepository _modules;
        private MonthBalanceDepositRepository _monthBalanceDeposit;
        private MonthBalanceLoanRepository _monthBalanceLoan;
        private NoBookRepository _noBook;
        //private NoCardRepository _noCard;
        private ProvinceRepository _province;
        private ReasonRepository _reason;
        private RequestLoanDueRepository _requestDue;
        private RequestLoanRepository _requestLoan;
        private SecurityRepository _security;
        private SubDistrictRepository _subdistrict;
        private TitleRepository _title;
        private TtlfDepositRepository _ttlfDeposit;
        private TtlfLoanRepository _ttlfLoan;
        private TxnCodeRepository _txnCode;
        private UserRepository _users;
        private UserTypeRepository _userTypes;
        private YearBalanceDepositRepository _yearBalanceDeposit;

        #endregion

        #region Public Repository
        public IAccessPermissionRepository AccessPermission
        {
            get { return _accessPermission ?? (_accessPermission = new AccessPermissionRepository(_context)); }
        }
        public IAccessTransactionRepository AccessTransaction
        {
            get { return _accessTransaction ?? (_accessTransaction = new AccessTransactionRepository(_context)); }
        }
        public IAccountPeriodRepository AccountPeriod
        {
            get { return _accountPeriod ?? (_accountPeriod = new AccountPeriodRepository(_context)); }
        }
        public ICoopControlRepository CoopControl
        {
            get { return _coopControl ?? (_coopControl = new CoopControlRepository(_context)); }
        }
        public IDepositRepository Deposit
        {
            get { return _deposit ?? (_deposit = new DepositRepository(_context)); }
        }
        public IDepositTypeRepository DepositType
        {
            get { return _depositType ?? (_depositType = new DepositTypeRepository(_context)); }
        }
        public IDistrictRepository District
        {
            get { return _district ?? (_district = new DistrictRepository(_context)); }
        }
        public ILoanRepository Loan
        {
            get { return _loan ?? (_loan = new LoanRepository(_context)); }
        }
        public ILoanDueRepository LoanDue
        {
            get { return _loanDue ?? (_loanDue = new LoanDueRepository(_context)); }
        }
        public ILoanTypeRepository LoanType
        {
            get { return _loanType ?? (_loanType = new LoanTypeRepository(_context)); }
        }
        public IMemberRepository Member
        {
            get { return _member ?? (_member = new MemberRepository(_context)); }
        }
        public IMemberGroupRepository MemberGroup
        {
            get { return _memberGroup ?? (_memberGroup = new MemberGroupRepository(_context)); }
        }
        public IMemberTypeRepository MemberType
        {
            get { return _memberType ?? (_memberType = new MemberTypeRepository(_context)); }
        }
        public IModuleRepository Modules
        {
            get { return _modules ?? (_modules = new ModuleRepository(_context)); }
        }
        public IModuleCategoriesRepository ModuleCategories
        {
            get { return _moduleCategories ?? (_moduleCategories = new ModuleCategoriesRepository(_context)); }
        }
        public IMonthBalanceDepositRepository MonthBalanceDeposit
        {
            get { return _monthBalanceDeposit ?? (_monthBalanceDeposit = new MonthBalanceDepositRepository(_context)); }
        }
        public IMonthBalanceLoanRepository MonthBalanceLoan
        {
            get { return _monthBalanceLoan ?? (_monthBalanceLoan = new MonthBalanceLoanRepository(_context)); }
        }
        public INoBookRepository NoBook
        {
            get { return _noBook ?? (_noBook = new NoBookRepository(_context)); }
        }
        //public INoCardRepository NoCard
        //{
        //    get { return _noCard ?? (_noCard = new NoCardRepository(_context)); }
        //}
        public IProvinceRepository Province
        {
            get { return _province ?? (_province = new ProvinceRepository(_context)); }
        }
        public IReasonRepository Reason
        {
            get { return _reason ?? (_reason = new ReasonRepository(_context)); }
        }
        public IRequestLoanDueRepository RequestDue
        {
            get { return _requestDue ?? (_requestDue = new RequestLoanDueRepository(_context)); }
        }
        public IRequestLoanRepository RequestLoan
        {
            get { return _requestLoan ?? (_requestLoan = new RequestLoanRepository(_context)); }
        }
        public ISecurityRepository Security
        {
            get { return _security ?? (_security = new SecurityRepository(_context)); }
        }
        public ITitleRepository Title
        {
            get { return _title ?? (_title = new TitleRepository(_context)); }
        }
        public ITtlfDepositRepository TtlfDeposit
        {
            get { return _ttlfDeposit ?? (_ttlfDeposit = new TtlfDepositRepository(_context)); }
        }
        public ITtlfLoanRepository TtlfLoan
        {
            get { return _ttlfLoan ?? (_ttlfLoan = new TtlfLoanRepository(_context)); }
        }
        public ITxnCodeRepository TxnCode
        {
            get { return _txnCode ?? (_txnCode = new TxnCodeRepository(_context)); }
        }
        public IUserRepository Users
        {
            get { return _users ?? (_users = new UserRepository(_context)); }
        }
        public IUserTypeRepository UserTypes
        {
            get { return _userTypes ?? (_userTypes = new UserTypeRepository(_context)); }
        }
        public ISubDistrictRepository SubDistrict
        {
            get { return _subdistrict ?? (_subdistrict = new SubDistrictRepository(_context)); }
        }
        public IInterestRepository Interest
        {
            get { return _interest ?? (_interest = new InterestRepository(_context)); }
        }
        public IYearBalanceDepositRepository YearBalanceDeposit
        {
            get { return _yearBalanceDeposit ?? (_yearBalanceDeposit = new YearBalanceDepositRepository(_context)); }
        }
        #endregion
    }
    #endregion
}