USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatMthDepositBal]    Script Date: 28/10/2563 19:14:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================

-- EXEC [dbo].[BatMthDepositBal] '3', 'SAV', 4, '2561', 1

ALTER PROCEDURE [dbo].[BatMthDepositBal]
	 @CoopID int, 
	 @DepTypeID nvarchar(5),
	 @UserId int,
	 @BudgetYear nvarchar(4),
	 @Period int
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

	SELECT	@StartDate = StartDate, @EndDate = EndDate FROM UCF.AccountPeriod 
	WHERE BudgetYear = @BudgetYear and PeriodID = @Period

	SELECT	@StartDate 
	SELECT	@EndDate
	SELECT	@DepTypeID

	--1. Prepare Deposit
	--1.1 Read Deposit
	DECLARE @tmp_MBDeposit table( 
							Filestatus  nvarchar(1), 
							CoopID int, 
							DepositTypeID nvarchar(3), 
							AccountNo nvarchar(15),
							OpenDate DateTime,
							LastContact DateTime,
							BudgetYear nvarchar(4),
							Period int,
							BFLedgerBal float,
							Deposit float,
							Withdraw float,
							CFLedgerBal float,
							AccInt float) 
    INSERT INTO @tmp_MBDeposit (Filestatus, CoopID, DepositTypeID, AccountNo, BFLedgerBal, Deposit, Withdraw, CFLedgerBal, AccInt, 
								OpenDate, LastContact)
								SELECT Filestatus, CoopID, DepositTypeID, AccountNo, LedgerBal, 0, 0, LedgerBal, AccInt, OpenDate, LastContact
								FROM Master.Deposit
								WHERE Filestatus <> 'D'
								AND DepositTypeID = @DepTypeID

	--1.2 update Deposit ที่เปิดหลัง @EndDate
	update @tmp_MBDeposit set Filestatus = 'D' where OpenDate > @EndDate

	--1.3 update Deposit ที่เปิดหลัง @EndDate
	update @tmp_MBDeposit set Filestatus = 'D' where LastContact < @StartDate and Filestatus = 'C'

	select * from @tmp_MBDeposit

	--2. Process Sum of Deposit, Withdraw
	--2.1 Calcualte Sum of Deposit, Withdraw from TtlfDeposit
	DECLARE @tmp_TtlfDepositSum table( 
							AccountNo nvarchar(15),
							Deposit float,
							Withdraw float)

	INSERT INTO @tmp_TtlfDepositSum (AccountNo, Deposit, Withdraw)
    SELECT		AccountNo, Sum(Credit) as Deposit, Sum(Debit) as Withdraw
	FROM		TXN.TtlfDeposit
	WHERE		DepositTypeID = @DepTypeID 
				AND TxnDate between @StartDate and @EndDate
				--DepositTypeID = 'SAV' AND TxnDate between '2013-6-1' and '2013-6-30'
	GROUP BY AccountNo

	select * from @tmp_TtlfDepositSum

	--2.2 Update Sum of Deposit, Withdraw from TtlfDeposit to @tmp_MBDeposit
	UPDATE d set 
	d.Deposit = t.Deposit,
	d.Withdraw = t.Withdraw
	FROM @tmp_MBDeposit d 
	INNER JOIN @tmp_TtlfDepositSum t on t.AccountNo = d.AccountNo
	
	--select * from @tmp_MBDeposit
	--3. หา BFLedgerBal
	DECLARE @tmp_BFLedger table( 
						AccountNo nvarchar(15),
						BFLedgerBal float)

	--3.1 หา BFLedgerBal จาก TtlfDeposit
	INSERT INTO @tmp_BFLedger (AccountNo, BFLedgerBal)
	SELECT B.AccountNo, B.BFLedgerBal FROM [TXN].[TtlfDeposit] B
	JOIN 
	(SELECT t.AccountNo, 
           t.BFLedgerBal, 
		   t.TxnDate, 
		   t.TxnSeq,
           ROW_NUMBER() OVER(PARTITION BY t.AccountNo 
                                 ORDER BY t.AccountNo, t.TxnDate, t.TxnSeq ASC) AS rk
    FROM [TXN].[TtlfDeposit] t
	WHERE t.TxnDate between @StartDate and @EndDate AND DepositTypeID = @DepTypeID) x
	--WHERE t.TxnDate between @StartDate and @EndDate) x
	on x.AccountNo = B.AccountNo AND x.TxnDate = B.TxnDate AND x.TxnSeq = B.TxnSeq
	WHERE x.rk = 1
	order by B.AccountNo

	select * FROM @tmp_BFLedger order by AccountNo

	--3.1 update @tmp_BFLedger.BFLedgerBal => @tmp_MBDeposit.BFLedgerBal และคำนวณ CFLedgerBal
	UPDATE d set 
	d.BFLedgerBal = b.BFLedgerBal,
	d.CFLedgerBal = ISNULL(b.BFLedgerBal, 0) + ISNULL(d.Deposit, 0) - ISNULL(d.Withdraw, 0),
	d.BudgetYear = @BudgetYear,
	d.Period = @Period
	FROM @tmp_MBDeposit d 
	INNER JOIN @tmp_BFLedger b on b.AccountNo = d.AccountNo

	select * FROM @tmp_MBDeposit where Filestatus <> 'D' order by AccountNo

	--4. Transfer @tmp_MBDeposit => MonthBalanceDeposit
	DELETE Master.MonthBalanceDeposit WHERE BudgetYear = @BudgetYear AND Period = @Period AND DepositTypeID = @DepTypeID
	--DELETE Master.MonthBalanceDeposit WHERE BudgetYear = @BudgetYear AND Period1 = @Period
	select * from @tmp_MBDeposit order by AccountNo

	INSERT INTO Master.MonthBalanceDeposit
	(CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Filestatus, CoopID, AccountNo, DepositTypeID, BudgetYear, Period, 
	BFLedgerBal, Deposit, Withdraw, CFLedgerBal, AccInt)
	--SELECT @UserId, GETDATE(), @UserId, GETDATE(), 'A', @CoopID, AccountNo, @DepTypeID, @BudgetYear, @Period, BFLedgerBal, Deposit, Withdraw, CFLedgerBal, AccInt
	SELECT @UserId, GETDATE(), @UserId, GETDATE(), 'A', @CoopID, AccountNo, DepositTypeID, @BudgetYear, @Period, 
	BFLedgerBal, Deposit, Withdraw, CFLedgerBal, AccInt
	FROM @tmp_MBDeposit
	Where Filestatus <> 'D'

	select * from Master.MonthBalanceDeposit 
	where BudgetYear = @BudgetYear and @Period = @Period and Filestatus = 'A'
	order by AccountNo

	--select * from Master.MonthBalanceDeposit where BudgetYear = @BudgetYear AND Period1 = @Period AND DepositTypeID = @DepTypeID
	--select * from Master.MonthBalanceDeposit where BudgetYear = @BudgetYear AND Period1 = @Period
	--8. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END
