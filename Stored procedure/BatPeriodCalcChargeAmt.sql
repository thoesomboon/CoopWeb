USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatPeriodCalcChargeAmt]    Script Date: 28/10/2563 19:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatPeriodCalcChargeAmt] 3, '2020-7-5', 4, 'WRK029'

ALTER PROCEDURE [dbo].[BatPeriodCalcChargeAmt]
	 @CoopID nvarchar(10), 
	 @CalcDate datetime,			-- วันที่คำนวณดอกเบี้ย
	 @UserId nvarchar(10), 
	 @WorkStationId nvarchar(30)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SystemDate datetime
	DECLARE @MaxRow int
	DECLARE @BudgetYear nvarchar(4)
	DECLARE @BranchId nvarchar(5)
	DECLARE @ProgramName nvarchar(50) = 'BatPeriodCalcChargeAmt'

	SELECT	@SystemDate = SystemDate, @BudgetYear = BudgetYear FROM Control.CoopControl WHERE CoopID = @CoopID
	
	--1. Prepare LoanDue
	--1.1 Read Loan accoring to @DepTypeID (Saving or Special Saving) rate
	DECLARE @tmp_LoanDue table( 
							CoopID nvarchar(15),
							LoanID nvarchar(15),
							LoanDueAmt float)
	INSERT INTO @tmp_LoanDue (CoopID, LoanID, LoanDueAmt)
    SELECT		CoopID, LoanID, Sum(LoanDueAmt)
	FROM		Master.LoanDue
	WHERE		Year(DueDate) > Year(@CalcDate)  AND Filestatus = 'A'
	GROUP BY CoopID, LoanID

	--2. Prepare Loan
	--1.2 Read Loan to calcualte interest & charge before 
	DECLARE @tmp_loan table(IdRow int IDENTITY(1, 1), 
							Filestatus  nvarchar(1), 
							CoopID nvarchar(10), 
							LoanID nvarchar(15), 
							MemberID nvarchar(15), 
							LoanTypeID nvarchar(3), 
      						BFBal float,
      						LoanBal float,
							UnpayPrinciple float, 
							BFInt float,
							UnpayInt float,
							IntCalc float,
							BFCharge float,
							UnpayCharge float,
							ChargeCalc float,
							IntRate float, 
							ChargeRate float, 
							days int,
							LastCalcInt datetime, 
							LastCalcCharge datetime,
							LoanDueAmt float,
							CalcDate datetime)
	--2. Calcualte interest in Loan
	--2.1 Binding Loan and Interest table (Interest might be several records)
    INSERT INTO @tmp_loan (Filestatus, CoopID, LoanTypeID, LoanID, LoanBal, UnpayPrinciple, 
							UnpayInt, IntCalc, UnpayCharge, ChargeCalc, IntRate, ChargeRate, days, 
							LastCalcInt, LastCalcCharge, LoanDueAmt, CalcDate)
	SELECT l.Filestatus, l.CoopID, l.LoanTypeID, l.LoanID, l.LoanBal, ISNULL(l.UnpayPrinciple, 0), 
							ISNULL(l.UnpayInt, 0), 0, ISNULL(l.UnpayCharge, 0), 0, l.IntRate, lt.ChargeRate, 0, 
							l.LastCalcInt, l.LastCalcCharge, m.LoanDueAmt, @CalcDate
			FROM Master.Loan l
			INNER JOIN Control.LoanType lt ON lt.CoopID = l.CoopID AND lt.LoanTypeID = l.LoanTypeID
			INNER JOIN @tmp_LoanDue m ON m.LoanID = l.LoanID AND m.LoanDueAmt > 0
			WHERE l.Filestatus = 'A' 
			--AND l.LoanID in ('ป00022559','ป00042563','ป00432560','ป00452563','ป00612560','ป00662562')

	--2.2 Calcualte Days between Loan.LastCalcInt to LastEffectDate or @CalcDate (SystemDate)
	UPDATE @tmp_loan SET days = Datediff(day, LastCalcInt, CalcDate)

	--2.3 Calcualte interest and charge by Rate1 only
	--UPDATE @tmp_loan SET IntCalc = dbo.RoundedI(LoanBal * days * IntRate / 36500), ChargeCalc = dbo.RoundedI(UnpayPrinciple * days * ChargeRate / 36500)
	UPDATE @tmp_loan SET IntCalc = dbo.RoundedI(LoanBal * days * IntRate / 36500),
						UnpayInt = UnpayInt + dbo.RoundedI(LoanBal * days * IntRate / 36500), 
						ChargeCalc = dbo.RoundedI(UnpayPrinciple * days * ChargeRate / 36500),
						UnpayCharge = UnpayCharge + dbo.RoundedI(UnpayPrinciple * days * ChargeRate / 36500),
						UnpayPrinciple = UnpayPrinciple + LoanDueAmt

	--select * from @tmp_loan order by LoanID

	--3. Update Loan
	  UPDATE l SET 
	  l.ModifiedBy = @UserId,
	  l.ModifiedDate = getdate(),
	  l.LastCalcInt = @CalcDate, 
	  l.LastCalcCharge = @CalcDate, 
	  l.UnpayInt = t.UnpayInt,
	  l.UnpayCharge = t.UnpayCharge,
	  l.UnpayPrinciple = t.UnpayPrinciple
	  FROM Master.Loan l
	  INNER JOIN @tmp_loan t ON l.LoanID = t.LoanID

	--7. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END