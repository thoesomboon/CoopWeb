USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatYrNewYearDeposit]    Script Date: 28/10/2563 19:17:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
-- EXEC [dbo].[BatYrNewYearDeposit] '3', '2557', 4
ALTER PROCEDURE [dbo].[BatYrNewYearDeposit] 
	-- Add the parameters for the stored procedure here
	@CoopID varchar(4), -- = '0001';
	@NewBudgetYear varchar(10),
	@UserID varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @DateNow datetime = GETDATE();
	DECLARE @RowCount int = 0;
	DECLARE @RowCountShare int = 0;
	DECLARE @RowCountLoan int = 0;
	DECLARE @RowCountDeposit int = 0;
	DECLARE @RowCountPN int = 0;
	DECLARE @RowCountALL varchar(100);
	DECLARE @TranCount int = 0;
	DECLARE @TranErrorCode int = -2;
	DECLARE @TranErrorParam int = -3;
	DECLARE @TempString nvarchar(500) = 'PROC: ';
	
	/*
	FOR Error Tracing
	*/
	DECLARE @ErrorNumber int = -1;
	DECLARE @ErrorSeverity int = -1;
	DECLARE @ErrorState int = -1;
	DECLARE @ErrorProcedure varchar(200) = N'N/A';
	DECLARE @ErrorLine int = -1;
	DECLARE @ErrorMessage varchar(1000) = N'N/A';

	DECLARE @NextDay DATETIME = (SELECT NextSystemDate FROM Control.CoopControl)
	DECLARE @BudgetYear NVARCHAR(4) = (SELECT BudgetYear FROM Control.CoopControl)
	
	IF @CoopID > 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				--ProcessNewYearLoan()
				UPDATE Master.Loan
				SET BFBal = LoanBal,
					BFCharge = UnpayCharge,
					BFDiscInt= UnpayDiscInt,
					BFInt = UnpayInt,
					BFUnpayCharge = UnpayCharge,
					BFUnpayPrinciple = UnpayPrinciple
				WHERE CoopID = @CoopID AND Filestatus = 'A'
				SET @RowCountDeposit = @@ROWCOUNT;

				--ProcessNewYearDeposit()
				UPDATE Master.Deposit
				SET BfLedgerBal = LedgerBal
				WHERE CoopID = @CoopID AND Filestatus = 'A'
				SET @RowCountDeposit = @@ROWCOUNT;

				----ProcessNewYearCoopControl()
				UPDATE Control.CoopControl
				SET PrevBudgetYear = BudgetYear,
					BudgetYear = @NewBudgetYear,
					PrevStartBudgetDate = StartBudgetDate,
					PrevEndBudgetDate = EndBudgetDate,
					StartBudgetDate = DATEADD("YEAR", 1,StartBudgetDate),
					EndBudgetDate =DATEADD("YEAR", 1,EndBudgetDate)
				WHERE CoopID = @CoopID AND Filestatus = 'A'
			END TRY
			BEGIN CATCH
				SET @ErrorNumber = (SELECT ERROR_NUMBER());
				SET @ErrorSeverity = (SELECT ERROR_SEVERITY());
				SET @ErrorState = (SELECT ERROR_STATE());
				SET @ErrorProcedure = (SELECT ERROR_PROCEDURE());
				SET @ErrorLine = (SELECT ERROR_LINE());
				SET @ErrorMessage = (SELECT ERROR_MESSAGE());
				--SELECT
				--	ERROR_NUMBER() AS ErrorNumber
				--	,ERROR_SEVERITY() AS ErrorSeverity
				--	,ERROR_STATE() AS ErrorState
				--	,ERROR_PROCEDURE() AS ErrorProcedure
				--	,ERROR_LINE() AS ErrorLine
				--	,ERROR_MESSAGE() AS ErrorMessage;
				IF @@TRANCOUNT > 0
					BEGIN
						SET @TranCount = @TranErrorCode;
						ROLLBACK TRANSACTION;
					END
			END CATCH;
			
			IF @@TRANCOUNT > 0		
				BEGIN
					SET @TranCount = @@TRANCOUNT;
					SET @TempString += CONVERT(varchar(50),'SHR:' + CAST(@RowCountShare AS VARCHAR) + ',LON:' 
											+ CAST(@RowCountLoan AS VARCHAR) + ', DEP:' + CAST(@RowCountDeposit AS VARCHAR));
					COMMIT TRANSACTION;
				END
		END
	ELSE
		BEGIN
			SET @TranCount = @TranErrorCode;
			SET @ErrorMessage = 'Parameter Required.';
			IF @CoopID = '' OR @CoopID IS NULL OR LEN(@CoopID) <> 4
				BEGIN
					SET @ErrorMessage += '''CoopID'' was not found or inactive.';
				END
			SET @TempString = '-';
		END

	---- UPDATE Coop ----------------------------------------------------------------------------------------------
	---- New Budget--------------------------------------------------------------------------------
	--DECLARE @EndBudgetDate datetime = (SELECT end_budget_date FROM coop WHERE coop_id = @CoopID)

	--IF(@NextDay > @EndBudgetDate) BEGIN
	--	UPDATE C SET start_budget_date = DATEADD(YEAR,1,start_budget_date),
	--				 end_budget_date = DATEADD(YEAR,1,end_budget_date),
	--				 prev_budget_year = @BudgetYear, 
	--				 budget_year = CONVERT(INT,@BudgetYear) + 1
	--	FROM coop C
	--	WHERE coop_id = @CoopID
	--END

	SELECT	@RowCount As [RowCount],
			@TranCount As [TranCount],
			@ErrorNumber As [ErrorNumber],
			@ErrorSeverity As [ErrorSeverity],
			@ErrorState As [ErrorState],
			@ErrorProcedure As [ErrorProcedure],
			@ErrorLine As [ErrorLine],
			CASE WHEN @TranCount >= 0 THEN @TempString ELSE @ErrorMessage + '\n' + @TempString END As [Message];
END
