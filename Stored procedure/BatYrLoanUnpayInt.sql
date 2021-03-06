USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatYrLoanUnpayInt]    Script Date: 04/11/2563 18:56:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatYrLoanUnpayInt] 3, '2020-7-5'

ALTER PROCEDURE [dbo].[BatYrLoanUnpayInt]
	 @CoopID nvarchar(10), 
	 @CalcDate datetime			-- วันที่คำนวณดอกเบี้ย
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SystemDate datetime
	DECLARE @MaxRow int
	--DECLARE @BudgetYear nvarchar(4)
	--DECLARE @BranchId nvarchar(5)
	--DECLARE @ProgramName nvarchar(50) = 'BatPeriodCalcChargeAmt'

	SELECT	@SystemDate = SystemDate FROM Control.CoopControl WHERE CoopID = @CoopID
	
	--1. Prepare Loan
	--1.1 Read Loan to calcualte interest & charge before 
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
							CalcDate datetime,
							tempUnpayInt float,
							tempUnpayCharge float)
	--2. Calcualte interest in Loan
	--2.1 Binding Loan and Interest table (Interest might be several records)
    INSERT INTO @tmp_loan (Filestatus, CoopID, LoanTypeID, LoanID, LoanBal, UnpayPrinciple, 
							UnpayInt, IntCalc, UnpayCharge, ChargeCalc, IntRate, ChargeRate, days, 
							LastCalcInt, LastCalcCharge, CalcDate, tempUnpayInt, tempUnpayCharge)
	SELECT l.Filestatus, l.CoopID, l.LoanTypeID, l.LoanID, l.LoanBal, UnpayPrinciple,
							ISNULL(l.UnpayInt, 0), 0, ISNULL(l.UnpayCharge, 0), 0, l.IntRate, lt.ChargeRate, 0, 
							l.LastCalcInt, l.LastCalcCharge, @CalcDate, 0, 0
			FROM Master.Loan l
			INNER JOIN Control.LoanType lt ON lt.CoopID = l.CoopID AND lt.LoanTypeID = l.LoanTypeID
			WHERE l.Filestatus = 'A' 

	--2.2 Calcualte Days between Loan.LastCalcInt to LastEffectDate or @CalcDate (SystemDate)
	UPDATE @tmp_loan SET days = Datediff(day, LastCalcInt, CalcDate)

	--2.3 Calcualte interest and charge by Rate1 only
	--UPDATE @tmp_loan SET IntCalc = dbo.RoundedI(LoanBal * days * IntRate / 36500), ChargeCalc = dbo.RoundedI(UnpayPrinciple * days * ChargeRate / 36500)
	UPDATE @tmp_loan SET IntCalc = dbo.RoundedI(LoanBal * days * IntRate / 36500),
						tempUnpayInt = UnpayInt + dbo.RoundedI(LoanBal * days * IntRate / 36500), 
						tempUnpayCharge = UnpayCharge + dbo.RoundedI(UnpayPrinciple * days * ChargeRate / 36500)

	select * from @tmp_loan order by LoanID

	--3. Update Loan
	  UPDATE l SET 
	  l.tempUnpayInt = t.tempUnpayInt,
	  l.tempUnpayCharge = t.tempUnpayCharge
	  FROM Master.Loan l
	  INNER JOIN @tmp_loan t ON l.LoanID = t.LoanID

	--4. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END