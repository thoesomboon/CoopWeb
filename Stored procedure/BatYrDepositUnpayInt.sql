USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatPeriodDepositIntDue]    Script Date: 04/11/2563 21:54:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatYrDepositUnpayInt] 3, '2020-12-31'

Alter PROCEDURE [dbo].[BatYrDepositUnpayInt]
	 @CoopID int, 
	 @CalcDate datetime			-- วันที่คำนวณดอกเบี้ย
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SystemDate datetime
	DECLARE @LastDepTxnSeq int
	DECLARE @MaxRow int
	DECLARE @BudgetYear nvarchar(4)
	--DECLARE @LedgerNo nvarchar(10) = ''
	--DECLARE @BranchId nvarchar(5) = ''
	--DECLARE @ProgramName nvarchar(50) = 'BatPeriodSavingIntDue' -- FrmBatPeriodSpecialIntDue
	--DECLARE @WorkStationId nvarchar(30)
	--DECLARE @TypeOfDep nvarchar(3)

	--select @TypeOfDep = TypeOfDeposit from Control.DepositType where DepositTypeID = @DepTypeID
	--If @TypeOfDep = 'SPC' 
	--	@ProgramName = 'BatPeriodSpecialIntDue';

	SELECT	@SystemDate = SystemDate, 
			@BudgetYear = BudgetYear FROM Control.CoopControl WHERE CoopID = @CoopID

	select @LastDepTxnSeq = count(*) from TXN.TtlfDeposit where TxnDate = @SystemDate
	If @LastDepTxnSeq > 0 
		 SELECT @LastDepTxnSeq = MAX(TxnSeq) FROM TXN.TtlfDeposit where TxnDate = @SystemDate;
	--select @LastDepTxnSeq

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
							order by i.Type, i.TInt, i.FirstEffectDate
	UPDATE @tmpInterest SET LastEffectDate = @CalcDate WHERE LastEffectDate IS NULL

	--select * from @tmpInterest
	
	select * FROM Master.Deposit where Filestatus = 'A' order by AccountNo

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
										AND d.DepositTypeID = t.Type 
										AND d.IntType = t.IntType
			WHERE d.Filestatus = 'A'
			--AND d.DepositTypeID = @DepTypeID
			AND d.LastCalcInt <=  ISNULL(t.LastEffectDate, @SystemDate)
			ORDER BY d.AccountNo

	select * FROM @tmp_deposit

	--3.2 Calcualte Days between Deposit.LastCalcInt to LastEffectDate or @CalcDate (SystemDate)
	UPDATE @tmp_deposit SET days = CASE WHEN LastCalcInt > FirstEffectDate THEN   
											CASE WHEN LastEffectDate = @CalcDate THEN Datediff(day, LastCalcInt, LastEffectDate )	
											ELSE Datediff(day, LastCalcInt, LastEffectDate ) + 1
											END
									ELSE Datediff(day, FirstEffectDate, LastEffectDate ) END

	--3.3 Calcualte interest by Rate1 only
	UPDATE @tmp_deposit SET IntCalc = dbo.RoundedC(AvailBal * days * Rate1 / 36500)

	select '@tmp_deposit'
	select * from @tmp_deposit

	DECLARE @tmp_deposit_sum table(IdRow int IDENTITY(1, 1), 
						Filestatus  nvarchar(1), 
						CoopID nvarchar(10), 
						DepositTypeID nvarchar(3), 
						AccountNo nvarchar(15), 
						AvailBal float, 
						LedgerBal float, 
						AccInt float, 
						IntCalc money, 
						MemberID nvarchar(15)) 
	INSERT INTO @tmp_deposit_sum(Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID, IntCalc)
    SELECT		Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID, SUM(IntCalc)
	FROM		@tmp_deposit
	GROUP BY Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID

	select '@tmp_deposit_sum'
	select * from @tmp_deposit_sum

	--6. Update Deposit
	UPDATE d SET 
	UnpayInt = d.AccInt + ds.IntCalc
	FROM Master.Deposit d
	INNER JOIN @tmp_deposit_sum ds ON ds.AccountNo = d.AccountNo

	select * from Master.Deposit where DepositTypeID = 'SAV' And Filestatus = 'A' order by AccountNo
		
	--8. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END
