USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatPeriodSavingIntDue]    Script Date: 06/07/2563 12:44:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatPeriodSavingIntDue] '3', 'SAV', '2013-6-30', 'R1', 4, 'BatPeriodSavingIntDue', 'IN360-ITDEV-029'

ALTER PROCEDURE [dbo].[BatPeriodSavingIntDue]
	 @CoopID nvarchar(10), 
	 @DepTypeID nvarchar(3),
	 @CalcDate datetime,			-- วันที่คำนวณดอกเบี้ย
	 @BranchId nvarchar(5), 
	 @UserId nvarchar(10), 
	 @ProgramName nvarchar(50), 
	 @WorkStationId nvarchar(30)
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
							AND i.Type = @DepTypeID
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
													AND d.DepositTypeID = t.Type 
													AND d.IntType = t.IntType
													AND d.LastCalcInt <=  ISNULL(t.LastEffectDate, @SystemDate)
						WHERE d.Filestatus = 'A'
						And d.DepositTypeID = @DepTypeID
						ORDER BY d.AccountNo

	--3.2 Calcualte Days between Deposit.LastCalcInt to LastEffectDate or @CalcDate (SystemDate)
	UPDATE @tmp_deposit SET days = CASE WHEN LastCalcInt > FirstEffectDate THEN   
											CASE WHEN LastEffectDate = @CalcDate THEN Datediff(day, LastCalcInt, LastEffectDate )	
											ELSE Datediff(day, LastCalcInt, LastEffectDate ) + 1
											END
									ELSE Datediff(day, FirstEffectDate, LastEffectDate ) END

	--3.3 Calcualte interest by Rate1 only
	UPDATE @tmp_deposit SET IntCalc = dbo.RoundedC(AvailBal * days * Rate1 / 36500)
	select * from @tmp_deposit

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
						MemberID nvarchar(15)) 
	INSERT INTO @tmp_deposit_sum(Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID, IntCalc)
    SELECT		Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID, SUM(IntCalc)
	FROM		@tmp_deposit
	GROUP BY Filestatus, CoopID, DepositTypeID, AccountNo, AvailBal, LedgerBal, AccInt, MemberID
	select * from @tmp_deposit_sum

	--4. Generate TtlfDeposit
	--4.1 Prepare tmp_TtlfDeposit from Deposit
	DECLARE @tmp_TtlfDeposit table(
				Filestatus  nvarchar(1), 
				CoopID nvarchar(10), 
				TxnDate datetime, 
				TxnSeq int, 					
				DepositTypeID nvarchar(3), 
				AccountNo nvarchar(15), 
				AvailBal float, 
				LedgerBal float, 
				AccInt float, 
				IntType nvarchar(2), 
				BFLedgerBal money, 
				Credit money, 
				CFLedgerBal money, 
				MemberID nvarchar(15))

	INSERT INTO @tmp_TtlfDeposit(TxnSeq, AccountNo, LedgerBal, AccInt, Credit, MemberID, DepositTypeID)
    SELECT		@LastDepTxnSeq + IdRow, AccountNo, LedgerBal, 0, ISNULL(AccInt, 0) + ISNULL(IntCalc, 0), MemberID, DepositTypeID
	FROM		@tmp_deposit_sum
	
	select * from @tmp_TtlfDeposit

	--4.2 Load TtlfDeposit from tmp_TtlfDeposit
	INSERT INTO TXN.TtlfDeposit 
		(Filestatus, CoopID, TxnDate, TxnSeq, TxnTime, UserID, WorkstationID, OriginalProcess, MemberID, DepositTypeID, AccountNo, 
		BackDate, BFLedgerBal, Credit, CFLedgerBal, AccInt, IntDueAmt, 
		BookFlag, BudgetYear, Type, TTxnCode, CDCode, InstrumentType)
	SELECT 'A', @CoopID, @SystemDate, TxnSeq, GETDATE(), @UserId, @WorkStationId, @ProgramName, MemberID, DepositTypeID, AccountNo, 
		@CalcDate, LedgerBal, Credit, LedgerBal + Credit, 0, Credit, 
		'N', @BudgetYear, DepositTypeID, '180', 'C', '80' 
	FROM @tmp_TtlfDeposit

	select * from TXN.TtlfDeposit where TxnDate = '2013-6-30' order by AccountNo

	--5. Generate NoBook
	--5.1 Prepare NoBook from Deposit
    DECLARE @tmp_Nobook table(
				IdRow int IDENTITY(1, 1), 
				AccountNo  nvarchar(15), 
				seq int)
    INSERT INTO @tmp_Nobook(AccountNo, seq)
	SELECT AccountNo, MAX(seq) FROM Master.NoBook   
	GROUP BY AccountNo

	--5.2 Load NoBook from @tmp_Nobook
	INSERT INTO Master.NoBook (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Filestatus, CoopID, AccountNo, Seq, TxnDate, BackDate, TTxnCode, AbbCode, CDCode, 
						TxnAmt, CFLedgerBal)
	SELECT @UserId, getdate(), @UserId, getdate(), 'A', @CoopID, tDep.AccountNo, ISNULL(n.seq, 0)+1, @SystemDate, @CalcDate, '180', 'INT', 'C', 
		tDep.Credit, tDep.LedgerBal + tDep.Credit
	FROM @tmp_TtlfDeposit tDep
	LEFT JOIN @tmp_Nobook n ON tDep.AccountNo = n.AccountNo

	select * from Master.NoBook where TxnDate = '2013-6-30' order by AccountNo

	--6. Update Deposit
	  UPDATE dep SET 
	  ModifiedBy = @UserId,
	  ModifiedDate = getdate(),
	  LastCalcInt = @CalcDate, 
	  AvailBal = dep.AvailBal + tDep.Credit, 
	  LedgerBal = dep.LedgerBal + tDep.Credit, 
	  AccInt = 0, 
	  IntDueAmt = tDep.Credit
	  FROM Master.Deposit dep
	  INNER JOIN @tmp_TtlfDeposit tDep ON dep.AccountNo = tDep.AccountNo

	select * from Master.Deposit where DepositTypeID = 'SAV' And Filestatus = 'A' order by AccountNo

	--7. Update CoopControl ยกเลิก last_dep_TxnSeq แล้ว
	--SELECT @MaxRow = MAX(TxnSeq) FROM @tmp_TtlfDeposit
	--IF @MaxRow  > 0 
	--BEGIN
	--UPDATE coop SET  last_dep_TxnSeq =  @MaxRow  WHERE CoopID = @CoopID
	--END
		
	--8. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END