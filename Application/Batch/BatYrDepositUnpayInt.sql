USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatYrDepositUnpayInt]    Script Date: 28/10/2563 19:17:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatYrDepositUnpayInt] '3', '2020-12-31'
ALTER PROCEDURE [dbo].[BatYrDepositUnpayInt]
	 @CoopID nvarchar(10), 
	 @CalcDate datetime
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SystemDate datetime
	DECLARE @LastDepTxnSeq int
	DECLARE @MaxRow int
	DECLARE @BudgetYear nvarchar(4)
	--DECLARE @LedgerNo nvarchar(10) = ''

	SELECT	@SystemDate = SystemDate, 
			@BudgetYear = BudgetYear FROM Control.CoopControl WHERE CoopID = @CoopID

	--1. Read Interest table accoring to @DepTypeID (Saving or Special Saving) rate
	DECLARE @tmpInterest table(IdRow int IDENTITY(1, 1), 
								Filestatus  nvarchar(1), 
								CoopID nvarchar(10), 
								Type nvarchar(3), 
								IntType nvarchar(2), 
								FirstEffectDate datetime, 
								LastEffectDate datetime, 
								Balance1 money, 
								Rate1 money, 
								Balance2 money, 
								Rate2 money, 
								Balance3 money, 
								Rate3 money, 
								Balance4 money, 
								Rate4 money, 
								Balance5 money, 
								Rate5 money) 

	INSERT INTO @tmpInterest(Filestatus, CoopID, Type, IntType, FirstEffectDate, LastEffectDate, Balance1, Rate1, 
							Balance2, Rate2, Balance3, Rate3, Balance4, Rate4, Balance5, Rate5)
							SELECT i.Filestatus, i.CoopID, i.Type, i.TInt, i.FirstEffectDate, i.LastEffectDate, i.Balance1, i.Rate1, 
									Balance2, Rate2, Balance3, Rate3, Balance4, Rate4, Balance5, Rate5 
							FROM Control.Interest i 
							WHERE i.Filestatus = 'A'
							--AND i.Type = @DepTypeID
							order by i.Type, i.TInt, i.FirstEffectDate
	UPDATE @tmpInterest SET LastEffectDate = @CalcDate WHERE  LastEffectDate IS NULL

	--select * from @tmpInterest

	--2. Prepare Deposit
	--2.1 Read Deposit accoring to @DepTypeID (Saving or Special Saving) rate
	DECLARE @tmp_deposit table(IdRow int IDENTITY(1, 1), 
							Filestatus  nvarchar(1), 
							CoopID nvarchar(10), 
							DepositTypeID nvarchar(3), 
							AccountNo nvarchar(15), 
							AvailBal float, 
							LedgerBal float, 
							AccInt float, 
							IntType nvarchar(2), 
							LastCalcInt datetime, 
							FirstEffectDate datetime, 
							LastEffectDate datetime, 
							Rate1 money, 
							days int, 
							IntCalc money, 
							MemberID nvarchar(15))  

	--3. Calcualte interest in Deposit
	--3.1 Binding Deposit and Interest table (Interest might be several records)
    INSERT INTO @tmp_deposit (Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, IntType, LastCalcInt, FirstEffectDate,  
							LastEffectDate, Rate1, days, IntCalc, MemberID)
	SELECT d.Filestatus, d.CoopID, d.DepositTypeID, d.AccountNo, d.AvailBal, d.LedgerBal, d.AccInt, d.IntType, d.LastCalcInt, t.FirstEffectDate, 
			t.LastEffectDate, t.Rate1, 0, 0, d.MemberID
			FROM Master.Deposit d
			INNER JOIN @tmpInterest t ON d.CoopID =  t.CoopID 
										AND t.Type =  d.DepositTypeID
										AND t.IntType = d.IntType
										AND ISNULL(t.LastEffectDate, @SystemDate) >= d.LastCalcInt 
			WHERE d.Filestatus = 'A'

	--3.2 Calcualte Days between Deposit.LastCalcInt to LastEffectDate or @CalcDate (SystemDate)
	UPDATE @tmp_deposit SET days = CASE WHEN LastCalcInt > FirstEffectDate THEN   
											CASE WHEN LastEffectDate = @CalcDate THEN Datediff(day, LastCalcInt, LastEffectDate )	
											ELSE Datediff(day, LastCalcInt, LastEffectDate ) + 1
											END
									ELSE Datediff(day, FirstEffectDate, LastEffectDate ) END

	--3.3 Calcualte interest by Rate1 only
	UPDATE @tmp_deposit SET IntCalc = dbo.RoundedC(AvailBal * days * Rate1 / 36500)

	select * from @tmp_deposit order by AccountNo

	DECLARE @tmp_deposit_sum table(IdRow int IDENTITY(1, 1), 
						Filestatus  nvarchar(1), 
						CoopID nvarchar(10), 
						DepositTypeID nvarchar(3), 
						AccountNo nvarchar(15), 
						AvailBal float, 
						LedgerBal float, 
						AccInt float, 
						--IntType nvarchar(2), 
						--LastCalcInt datetime, 
						--FirstEffectDate datetime, 
						--LastEffectDate datetime, 
						--Rate1 money, 
						--days int, 
						IntCalc money, 
						MemberID nvarchar(15),
						Balance money) 
	INSERT INTO @tmp_deposit_sum(Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID, IntCalc)
    SELECT		Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID, SUM(IntCalc)
	FROM		@tmp_deposit
	GROUP BY Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID

	select * from @tmp_deposit_sum

	--4. Update Deposit
	UPDATE Master.Deposit SET UnpayInt = 0

	UPDATE dep SET 
	UnpayInt = ISNULL(DepS.AccInt, 0) + ISNULL(DepS.IntCalc, 0)
	FROM Master.Deposit dep
	INNER JOIN @tmp_deposit_sum DepS ON dep.AccountNo = DepS.AccountNo

	select * from Master.Deposit

END