USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatMthLoanBal]    Script Date: 04/11/2563 20:23:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatYrLoanBal] 3, 4, '2560', 1, 6

Alter PROCEDURE [dbo].[BatYrLoanBal]
	 @CoopID int, 
	 --@LoanTypeID nvarchar(5),
	 @UserId int,
	 @BudgetYear nvarchar(4),
	 @Period1 int,
	 @Period2 int
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SystemDate datetime
	DECLARE @MaxRow int
	DECLARE @StartDate datetime
	DECLARE @EndDate datetime
	--DECLARE @StartDate nvarchar(10)	= '2013-06-01'
	--DECLARE @EndDate nvarchar(10) = '2013-06-30'


	SELECT	@SystemDate = SystemDate FROM Control.CoopControl WHERE CoopID = @CoopID

	SELECT	@StartDate = StartDate FROM UCF.AccountPeriod 
	WHERE BudgetYear = @BudgetYear and PeriodID = 1

	SELECT	@EndDate = EndDate FROM UCF.AccountPeriod 
	WHERE BudgetYear = @BudgetYear and PeriodID = @Period2

	SELECT	@StartDate 
	SELECT	@EndDate
	--SELECT	@LoanTypeID

	--1. Prepare Loan
	--1.1 Read Loan
	DECLARE @tmp_MBLoan table( 
							Filestatus  nvarchar(1), 
							CoopID int, 
							LoanTypeID nvarchar(3), 
							LoanID nvarchar(15),
							BudgetYear nvarchar(4),
							Period1 int,
							Period2 int,
							LoanDate DateTime,
							LastContact DateTime,
							MemberID nvarchar(15),
							BFBal float,
							LoanAmt float,
							Credit float,
							LoanBal float,
							LoanDueInYear float,
							CreditBeforeDue float,
							LoanDueDefault float,
							LoanDueNextYear float,
							LoanDue float,
							BFInt float,
							BFIntCredit float,
							IntCalcInYear float,
							IntCalcInYearCredit float,
							UnpayInt float,
							BFCharge float,
							BFChargeCredit float,
							ChargeCalcInYear float,
							ChargeCalcInYearCredit float,
							UnpayCharge float,
							BFDiscInt float,
							BFDiscIntCredit float,
							DiscIntCalcInYear float,
							DiscIntCalcInYearCredit float,
							UnpayDiscInt float) 
    INSERT INTO @tmp_MBLoan (Filestatus, CoopID, LoanTypeID, LoanID, BudgetYear, Period1, Period2, LoanDate, LastContact, MemberID, BFBal, 
							LoanAmt, 
							LoanBal, LoanDueDefault, BFInt, IntCalcInYear, UnpayInt, BFCharge, ChargeCalcInYear, UnpayCharge, 
							LoanDueInYear, LoanDueNextYear, CreditBeforeDue, BFIntCredit, BFChargeCredit, ChargeCalcInYearCredit, 
							BFDiscInt, BFDiscIntCredit, DiscIntCalcInYear, DiscIntCalcInYearCredit, UnpayDiscInt)
				SELECT Filestatus, CoopID, LoanTypeID, LoanID, @BudgetYear, @Period1, @Period2, LoanDate, LastContact, MemberID, BFBal, 
							Case When (LoanDate >= @StartDate and LoanDate <= @EndDate) Then LoanAmt Else 0 End as LoanAmt, 
							LoanBal, Isnull(UnpayPrinciple, 0), Isnull(BFInt, 0), 0, Isnull(Unpayint, 0), Isnull(BFCharge, 0), 0, Isnull(UnpayCharge, 0), 
							0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
							FROM Master.Loan
							WHERE Filestatus <> 'D'
							--AND LoanTypeID = @LoanTypeID
							--AND LoanID = 'ป00012560'

	--1.2 update Loan ที่เปิดหลัง @EndDate
	update @tmp_MBLoan set Filestatus = 'D' where LoanDate > @EndDate

	--1.3 update Loan ที่เปิดหลัง @EndDate
	update @tmp_MBLoan set Filestatus = 'D' where LastContact < @StartDate and Filestatus = 'P'

	--select * from @tmp_MBLoan

	--2. Process Sum of TtlfLoan
	DECLARE @tmp_TtlfLoanSum table( 
							CoopID nvarchar(15),
							LoanID nvarchar(15),
							Credit float, 
							CreditBeforeDue float, 
							BFIntCredit float,
							IntCalcInYearCredit float,
							BFChargeCredit float,
							ChargeCalcInYearCredit float)

	INSERT INTO @tmp_TtlfLoanSum (CoopID, LoanID, Credit, CreditBeforeDue, BFIntCredit, IntCalcInYearCredit, BFChargeCredit, ChargeCalcInYearCredit)
    SELECT		CoopID, LoanID, Sum(PrincipleAmt), 0, Sum(BFInt), Sum(IntCalc), Sum(BFCharge), Sum(ChargeCalc)
	FROM		TXN.TtlfLoan
	WHERE		TxnDate between @StartDate and @EndDate
	GROUP BY CoopID, LoanID

 --   SELECT		CoopID, LoanID, TxnDate, PrincipleAmt, 0, BFInt, IntCalc, IntAmt, BFCharge, ChargeAmt
	--FROM		TXN.TtlfLoan
	--WHERE		TxnDate between @StartDate and @EndDate AND LoanID = 'ป00012560'
	--order by TxnDate

	--3. หา หนี้ครบกำหนด LoanDueInYear, LoanDueNextYear
	DECLARE @tmp_LoanDueInYear table( 
							CoopID nvarchar(15),
							LoanID nvarchar(15),
							LoanDueInYear float)

	INSERT INTO @tmp_LoanDueInYear (CoopID, LoanID, LoanDueInYear)
    SELECT		CoopID, LoanID, Sum(BFLoanDueAmt)
	FROM		Master.LoanDue
	WHERE		Year(DueDate) = Year(@StartDate) AND Filestatus <> 'D'
	GROUP BY CoopID, LoanID

	DECLARE @tmp_LoanDueNextYear table( 
							CoopID nvarchar(15),
							LoanID nvarchar(15),
							LoanDueNextYear float)
	INSERT INTO @tmp_LoanDueNextYear (CoopID, LoanID, LoanDueNextYear)
    SELECT		CoopID, LoanID, Sum(BFLoanDueAmt)
	FROM		Master.LoanDue
	WHERE		Year(DueDate) > Year(@StartDate)  AND Filestatus <> 'D'
	GROUP BY CoopID, LoanID

	--select * from @tmp_TtlfLoanSum
	--select 'Before UPDATE'
	--select * FROM @tmp_MBLoan where Filestatus <> 'D' order by LoanID

	select * from @tmp_TtlfLoanSum order by LoanID

	--4 update @tmp_TtlfLoanSum
	--4.1 update @tmp_TtlfLoanSum => @tmp_MBLoan
	UPDATE l set 
		l.Credit = t.Credit,
		l.CreditBeforeDue = t.CreditBeforeDue,
		l.BFIntCredit = t.BFIntCredit,
		l.IntCalcInYearCredit = t.IntCalcInYearCredit,
		l.BFChargeCredit = t.BFChargeCredit,
		l.ChargeCalcInYearCredit = t.ChargeCalcInYearCredit
	FROM @tmp_MBLoan l 
	INNER JOIN @tmp_TtlfLoanSum t on t.LoanID = l.LoanID

	--4.2 update @@tmp_LoanDueInYear => @tmp_MBLoan
	UPDATE l set 
		l.LoanDueInYear = t.LoanDueInYear 
	FROM @tmp_MBLoan l 
	INNER JOIN @tmp_LoanDueInYear t on t.LoanID = l.LoanID

	--4.3 update @@tmp_LoanDueNextYear => @tmp_MBLoan
	UPDATE l set 
		l.LoanDueNextYear = t.LoanDueNextYear
	FROM @tmp_MBLoan l 
	INNER JOIN @tmp_LoanDueNextYear t on t.LoanID = l.LoanID

	select 'Before Load @tmp_MBLoan to MBLoan'
	select * FROM @tmp_MBLoan where Filestatus <> 'D' order by LoanID

	--4. Transfer @tmp_MBLoan => YearBalanceLoan
	DELETE Master.YearBalanceLoan WHERE BudgetYear = @BudgetYear AND Period1 = @Period1 AND Period2 = @Period2
	--DELETE Master.YearBalanceLoan WHERE BudgetYear = @BudgetYear AND Period1 = @Period
	--select * from @tmp_MBLoan order by LoanID

	--Credit, CreditBeforeDue, BFIntCredit, IntCalcInYearCredit, BFChargeCredit, ChargeCalcInYearCredit
	--LoanDueInYear, LoanDueNextYear
	INSERT INTO Master.YearBalanceLoan
							(CreateBy, CreateDate, ModifiedBy, ModifiedDate, Filestatus, CoopID, LoanTypeID, LoanID, BudgetYear, Period1, Period2, MemberID, BFBal, LoanAmt, Credit, LoanBal, 
							LoanDueDefault, LoanDueNextYear, LoanDue, 
							BFInt, BFIntCredit, IntCalcInYear, IntCalcInYearCredit, UnpayInt, 
							BFCharge, BFChargeCredit, ChargeCalcInYear, ChargeCalcInYearCredit, UnpayCharge)
	SELECT @UserId, GETDATE(), @UserId, GETDATE(), 'A', @CoopID, LoanTypeID, LoanID, @BudgetYear, @Period1, @Period2, MemberID, BFBal, LoanAmt, Credit, LoanBal, 
							LoanDueDefault, LoanDueNextYear, 0, 
							BFInt, BFIntCredit, IntCalcInYear, IntCalcInYearCredit, UnpayInt, 
							BFCharge, BFChargeCredit, ChargeCalcInYear, ChargeCalcInYearCredit, UnpayCharge
	FROM @tmp_MBLoan
	Where Filestatus <> 'D'

	select * from Master.YearBalanceLoan 
	where BudgetYear = @BudgetYear and Period1 = @Period1 and Period2 = @Period2 and Filestatus = 'A'
	order by LoanID

	--8. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END