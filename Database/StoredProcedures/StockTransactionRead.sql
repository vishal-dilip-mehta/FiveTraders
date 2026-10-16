CREATE OR ALTER PROCEDURE Investments.StockTransactionRead
    @StockTransactionId INT = NULL,
    @StockTransactionLotId INT = NULL,
    @TickerId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Omit all filters to read every transaction.
    SELECT StockTransactionId, StockTransactionLotId, TickerId,
           BuyDate, BuyPrice, SellDate, SellPrice, Quantity,
           Profit, ProfitPercent
    FROM Investments.StockTransaction
    WHERE (@StockTransactionId IS NULL OR StockTransactionId = @StockTransactionId)
      AND (@StockTransactionLotId IS NULL OR StockTransactionLotId = @StockTransactionLotId)
      AND (@TickerId IS NULL OR TickerId = @TickerId)
    ORDER BY StockTransactionId;
END;
GO
