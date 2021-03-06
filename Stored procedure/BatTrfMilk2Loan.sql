USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatTrfMilk2Loan]    Script Date: 03/11/2563 9:11:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatTrfMilk2Loan] 3, '2020-7-5', 4, 'WRK029'

ALTER PROCEDURE [dbo].[BatTrfMilk2Loan]
	 @CoopID int, 
	 @CalcDate datetime,			-- วันที่คำนวณดอกเบี้ย
	 @UserId int, 
	 @WorkStationId nvarchar(30)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SystemDate datetime
	DECLARE @LastLonTxnSeq int = 0
	DECLARE @MaxRow int
	DECLARE @BudgetYear nvarchar(4)
	DECLARE @RcptBookNo int
	DECLARE @RcptRunNo int
	DECLARE @BranchId nvarchar(5)
	DECLARE @ProgramName nvarchar(50) = 'BatTrfMilk2Loan'

	SELECT	@SystemDate = SystemDate, @BudgetYear = BudgetYear, @RcptBookNo = LastReceiptBookNo, @RcptRunNo = LastReceiptRunNo FROM Control.CoopControl WHERE CoopID = @CoopID
	
	select @LastLonTxnSeq = count(*) from TXN.TtlfLoan where TxnDate = @SystemDate
	If @LastLonTxnSeq > 0 
		 SELECT @LastLonTxnSeq = MAX(TxnSeq) FROM TXN.TtlfLoan where TxnDate = @SystemDate;

	--select @LastLonTxnSeq, @RcptBookNo
	select * from TRF.MilkPayment 
	where LoanID is not null
	--where LoanID in ('ป00022559','ป00042563','ป00432560','ป00452563','ป00612560','ป00662562', 'ป00572558', 'ป00642562')
	order by LoanID

	--1. Prepare Loan
	--1.1 Read Loan accoring to @DepTypeID (Saving or Special Saving) rate
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
							Balance float,
							InstallAmt float,
							Credit float,
							CalcDate datetime,
							BackDate datetime)
	--2. Calcualte interest in Loan
	--2.1 Binding Loan and Interest table (Interest might be several records)
    INSERT INTO @tmp_loan (Filestatus, CoopID, LoanTypeID, LoanID, MemberID, BFBal, LoanBal, UnpayPrinciple, BFInt,
							UnpayInt, IntCalc, BFCharge, UnpayCharge, ChargeCalc, IntRate, ChargeRate, days, 
							LastCalcInt, LastCalcCharge, Balance, InstallAmt, CalcDate, BackDate)
	SELECT l.Filestatus, l.CoopID, l.LoanTypeID, l.LoanID, l.MemberID, ISNULL(l.BFBal, 0), l.LoanBal, ISNULL(l.UnpayPrinciple, 0), ISNULL(l.BFInt, 0), 
							ISNULL(l.UnpayInt, 0) + ISNULL(m.LoanInt, 0),0, ISNULL(l.BFCharge, 0), ISNULL(l.UnpayCharge, 0)+ ISNULL(m.LoanCharge, 0), 0, l.IntRate, lt.ChargeRate, 0, 
							l.LastCalcInt, l.LastCalcCharge, m.Balance, l.InstallAmt, @CalcDate, @SystemDate
			FROM Master.Loan l
			INNER JOIN Control.LoanType lt ON lt.CoopID =  l.CoopID 
										AND lt.LoanTypeID =  l.LoanTypeID
			INNER JOIN TRF.MilkPayment m ON m.LoanID = l.LoanID
										AND m.Balance > 0
										AND m.Filestatus = 'A'
			WHERE l.Filestatus = 'A' 
			--AND l.LoanID in ('ป00022559','ป00042563','ป00432560','ป00452563','ป00612560','ป00662562')

	--2.2 Calcualte Days between Loan.LastCalcInt to LastEffectDate or @CalcDate (SystemDate)
	UPDATE @tmp_loan SET days = Datediff(day, LastCalcInt, CalcDate)

	--2.3 Calcualte interest and charge by Rate1 only
	--UPDATE @tmp_loan SET IntCalc = dbo.RoundedC(LoanBal * days * IntRate / 36500), ChargeCalc = dbo.RoundedC(UnpayPrinciple * days * ChargeRate / 36500)
	--UPDATE @tmp_loan SET UnpayInt = UnpayInt + IntCalc, UnpayCharge = UnpayCharge + ChargeCalc

	--select * from @tmp_loan order by LoanID

	--3. Generate TtlfLoan
	--3.1 Prepare tmp_TtlfLoan from Loan
	DECLARE @tmp_TtlfLoan table(
				Filestatus nvarchar(1), 
				CoopID nvarchar(10), 
				TxnDate datetime, 
				TxnSeq int,
				LoanTypeID nvarchar(3),
				LoanID nvarchar(15), 
				MemberID nvarchar(15), 
				LoanBal float, 
				BFBal float, 
				Amt1 float, 
				CFBal float, 
				BFInt float, 
				IntCalc float, 
				IntAmt float, 
				UnpayInt float, 
				BFCharge float, 
				ChargeCalc float, 
				ChargeAmt float, 
				UnpayCharge float, 
				PrincipleAmt float, 
				UnpayPrinciple float, 
				Balance float, 
				InstallAmt float, 
				TTxnCode nvarchar(5), 
				CDCode nvarchar(1), 
				RcptBookNo nvarchar(15),
				RcptRunNo nvarchar(15))

	INSERT INTO @tmp_TtlfLoan(Filestatus, CoopID, TxnDate, TxnSeq, LoanTypeID, LoanID, MemberID, LoanBal, BFBal, Amt1, CFBal, 
				BFInt, IntCalc, IntAmt, UnpayInt, BFCharge, ChargeCalc, ChargeAmt, UnpayCharge, PrincipleAmt, UnpayPrinciple, Balance, InstallAmt, 
				TTxnCode, CDCode)
    SELECT		'A', CoopID, @CalcDate, IdRow, LoanTypeID, LoanID, MemberID, LoanBal, LoanBal, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,'131', 'C'
	FROM		@tmp_loan

	--select '@tmp_Loan'
	--select * from @tmp_Loan
	
	--3.2 Calcualte Charge in @tmp_TtlfLoan
			-- UPDATE @tmp_TtlfLoan
			UPDATE t  SET 
						t.BFCharge = 
							Case When l.BFCharge > 0 Then
								Case When l.Balance >= l.BFCharge Then l.BFCharge Else l.Balance End
							Else 0 End,
						t.ChargeAmt = 
							Case When l.UnpayCharge > 0 Then
								Case When l.Balance >= l.UnpayCharge Then l.UnpayCharge Else l.Balance End
							Else 0 End,
						t.ChargeCalc = 
							Case When l.UnpayCharge > 0 Then
								Case When l.Balance >= l.UnpayCharge Then 
									l.UnpayCharge - l.BFCharge 
								Else
									Case When l.Balance >= l.BFCharge Then l.Balance - l.BFCharge Else 0 End
								End
							Else 0 End,
						t.UnpayCharge = 
							Case When l.UnpayCharge > 0 Then
								Case When l.Balance >= l.UnpayCharge Then 0 Else l.UnpayCharge - l.Balance End
							Else 0 End
			from @tmp_TtlfLoan t INNER JOIN @tmp_loan l ON l.LoanID = t.LoanID
			WHERE  l.Balance > 0 and l.UnpayInt > 0
			-- UPDATE @tmp_loan
			UPDATE @tmp_loan  SET Balance = Case When UnpayCharge >= Balance Then 0 Else Balance - UnpayCharge End
			WHERE  Balance > 0 and UnpayCharge > 0 --and Balance >= UnpayCharge

	--select 'Charge'
	--select '@tmp_Loan'
	--select * from @tmp_loan order by LoanID
	--select '@tmp_TtlfLoan'
	--select * from @tmp_TtlfLoan order by LoanID
	--3.3 Calcualte Int in @tmp_TtlfLoan
			-- UPDATE @tmp_TtlfLoan
			UPDATE t SET 
					t.BFInt = 
						Case When l.BFInt > 0 Then
							Case When l.Balance >= l.BFInt Then l.BFInt Else l.Balance End
						Else 0 End,
					t.IntAmt = 
						Case When l.UnpayInt > 0 Then
							Case When l.Balance >= l.UnpayInt Then l.UnpayInt Else l.Balance End
						Else 0 End,
					t.IntCalc = 
						Case When l.UnpayInt > 0 Then
							Case When l.Balance >= l.UnpayInt Then 
								l.UnpayInt - l.BFInt 
							Else
								Case When l.Balance >= l.BFInt Then l.Balance - l.BFInt Else 0 End
							End
						Else 0 End,
					t.UnpayInt = 
						Case When l.UnpayInt > 0 Then
							Case When l.Balance >= l.UnpayInt Then 0 Else l.UnpayInt - l.Balance End
						Else 0 End,
					t.Balance = 
						Case When l.Balance > 0 and l.UnpayInt > 0 Then
							Case When l.UnpayInt >= l.Balance Then 0 Else l.Balance - l.UnpayInt End
						End
			from @tmp_TtlfLoan t INNER JOIN @tmp_loan l ON l.LoanID = t.LoanID
			WHERE  l.Balance > 0 and l.UnpayInt > 0
			-- UPDATE @tmp_loan
			UPDATE @tmp_loan  SET Balance = Case When UnpayInt >= Balance Then 0 Else Balance - UnpayInt End
			WHERE  Balance > 0 and UnpayInt > 0 --and Balance >= UnpayInt

	--select 'Int'
	--select * from @tmp_loan order by LoanID
	--select * from @tmp_TtlfLoan order by LoanID

	--3.4 Calcualte Principle and Balance in @tmp_TtlfLoan
		UPDATE t SET 
			t.UnpayPrinciple = 
			CASE WHEN l.UnpayPrinciple > 0 THEN
				CASE WHEN l.Balance >= l.InstallAmt THEN 
					CASE WHEN l.UnpayPrinciple > l.InstallAmt THEN l.UnpayPrinciple - l.InstallAmt ELSE l.InstallAmt END
				ELSE -- Balance < InstallAmt
					CASE WHEN l.Balance >= l.UnpayPrinciple THEN 0 ELSE l.UnpayPrinciple - l.Balance END
				END
			END,
			t.LoanBal = 
			CASE WHEN l.Balance >= l.InstallAmt THEN
				CASE WHEN l.InstallAmt <= l.LoanBal THEN l.LoanBal - l.InstallAmt ELSE 0 END
			ELSE -- Balance < InstallAmt
				CASE WHEN l.Balance <= l.LoanBal THEN l.LoanBal - l.Balance ELSE 0 END
			END,
			t.PrincipleAmt = 
			CASE WHEN l.Balance >= l.InstallAmt THEN
				CASE WHEN l.InstallAmt <= l.LoanBal THEN l.InstallAmt ELSE l.LoanBal END
			ELSE -- Balance < InstallAmt
				CASE WHEN l.Balance <= l.LoanBal THEN l.Balance ELSE l.LoanBal END
			END,
			t.Balance = 
			Case When t.Balance > 0 And t.InstallAmt > 0 Then
				Case When t.Balance >= t.InstallAmt Then 
					Case When t.InstallAmt >= t.LoanBal Then t.Balance - t.LoanBal Else t.Balance - t.InstallAmt End
				Else
					0
				End
			End
		FROM @tmp_TtlfLoan t INNER JOIN @tmp_Loan l ON l.LoanID = t.LoanID
		--WHERE Balance > 0
		UPDATE @tmp_TtlfLoan SET CFBal = LoanBal
		UPDATE @tmp_TtlfLoan SET Filestatus = 'P' WHERE LoanBal = 0
		-- UPDATE @tmp_loan
		UPDATE @tmp_loan  SET Balance = 
		Case When InstallAmt >= Balance Then
			Case When InstallAmt <= LoanBal Then  0  Else Balance - InstallAmt End
		Else
			Balance - InstallAmt
		End
		WHERE  Balance > 0 and InstallAmt > 0 --and Balance >= UnpayInt

	--3.5 Calcualte AMT1 and RcptBookNo / RcptRunNo
		--UPDATE @tmp_TtlfLoan SET Amt1 = PrincipleAmt + IntAmt + ChargeAmt
		UPDATE @tmp_TtlfLoan SET Amt1 = PrincipleAmt + IntAmt + ChargeAmt, 
								RcptBookNo = RIGHT(CONVERT(nvarchar(3), @RcptBookNo), 3), 
								RcptRunNo = RIGHT('0000' + CONVERT(nvarchar(4), @RcptRunNo + TxnSeq), 4)
								--RcptRunNo = RIGHT('0000' + CONVERT(nvarchar(4), @RcptRunNo + ROW_NUMBER() OVER(ORDER BY LoanID)), 4)
	--select * from @tmp_TtlfLoan
	UPDATE @tmp_TtlfLoan SET CFBal = BFBal - PrincipleAmt Where BFBal > PrincipleAmt
	UPDATE @tmp_TtlfLoan SET CFBal = 0 Where BFBal <= PrincipleAmt

	select 'Balance'
	select '@tmp_Loan'
	select * from @tmp_loan order by LoanID
	select '@tmp_TtlfLoan'
	select * from @tmp_TtlfLoan order by LoanID

	--4. Load TtlfLoan from tmp_TtlfLoan
	INSERT INTO TXN.TtlfLoan 
		(CoopID, TxnDate, TxnSeq, TxnTime, UserID, WorkstationID, 
		OriginalProcess, Filestatus, MemberID, LoanID, BackDate, 
		BFBal, Amt1, CFBal, PrincipleAmt, CFUnpayPrinciple, BFInt, IntCalc, IntAmt, UnpayInt, 
		ChargeCalc, ChargeAmt, BFCharge, UnpayCharge, TTxnCode, CDCode, RcptBookNo, RcptRunNo)
	SELECT @CoopID, @SystemDate, 
		TxnSeq = Case When @LastLonTxnSeq > 0 Then @LastLonTxnSeq + TxnSeq Else TxnSeq End,
		GETDATE(), @UserId, @WorkStationId, 
		@ProgramName, 'A', MemberID, LoanID, @CalcDate, 
		BFBal, Amt1, CFBal, PrincipleAmt, UnpayPrinciple, BFInt, IntCalc, IntAmt, UnpayInt, 
		ChargeCalc, ChargeAmt, BFCharge, UnpayCharge, TTxnCode, CDCode, RcptBookNo, RcptRunNo
	FROM @tmp_TtlfLoan
	--UPDATE TXN.TtlfLoan SET CFBal = BFBal - PrincipleAmt Where BFBal > PrincipleAmt
	--UPDATE TXN.TtlfLoan SET CFBal = 0 Where BFBal <= PrincipleAmt

	--5. Update Loan
	  UPDATE l SET 
	  l.ModifiedBy = @UserId,
	  l.ModifiedDate = getdate(),
	  l.LastCalcInt = @CalcDate, 
	  l.LastCalcCharge = @CalcDate, 
	  l.LoanBal = t.CFBal, 
	  l.BFInt = l.BFInt - t.BFInt,
	  l.UnpayInt = t.UnpayInt,
	  l.BFCharge = l.BFCharge - t.BFCharge,
	  l.UnpayCharge = t.UnpayCharge,
	  l.UnpayPrinciple = t.UnpayPrinciple
	  FROM Master.Loan l
	  INNER JOIN @tmp_TtlfLoan t ON l.LoanID = t.LoanID

	--6. Update MilkPayment
	  UPDATE t SET 
		t.Balance = t.Receive - l.Amt1, 
		t.Filestatus = 
			Case When t.Balance = 0 Then
				'C'
			Else
				'A'
			End
	  FROM TRF.MilkPayment t INNER JOIN @tmp_TtlfLoan l on l.LoanID = t.LoanID

	--select '@tmp_Loan 3'
	--select * from @tmp_Loan
	select 'TtlfLoan'
	select * from TXN.TtlfLoan order by LoanID
	select 'Loan'
	select * from Master.Loan order by LoanID

	select 'Loan -> MilkPayment'
	select l.LoanID, l.LoanBal, l.BFInt, l.UnpayInt, l.BFCharge, l.UnpayCharge, l.UnpayPrinciple, 
	t.LoanID, t.LoanInt, t.LoanCharge, t.LoanAmt, t.Receive, t.Balance
	from Master.Loan l inner join TRF.MilkPayment t on t.LoanID = l.LoanID

	select 'MilkPayment Final'
	select * from TRF.MilkPayment where LoanID is not null
	select * from @tmp_Loan
	select * from @tmp_TtlfLoan

	--7. สรุป
	SELECT @MaxRow As [RowCount], 
			0 As TranCount, 
			0 As ErrorNumber, 
			0 As ErrorSeverity, 
			0 As ErrorState, '' As ErrorProcedure, 
			0 As ErrorLine, 
			'' As [Message]
END