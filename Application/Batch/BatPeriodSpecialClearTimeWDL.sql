USE [CoopWeb]
GO
/****** Object:  StoredProcedure [dbo].[BatPeriodSpecialClearTimeWDL]    Script Date: 28/10/2563 19:16:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- EXEC [dbo].[BatPeriodSpecialClearTimeWDL]
ALTER PROCEDURE [dbo].[BatPeriodSpecialClearTimeWDL] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
     UPDATE Master.Deposit 
	 SET MonthWithdrawAmt = 0 ,
	     MonthWithdrawTimes = 0
	 WHERE Filestatus = 'A' --AND DepositTypeID = 'SPC'
END