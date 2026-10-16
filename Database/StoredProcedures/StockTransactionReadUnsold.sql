CREATE OR ALTER PROCEDURE Investments.StockTransactionReadUnsold
AS
BEGIN
    SET NOCOUNT ON;

    -- A transaction remains unsold until a sale date is recorded.
    SELECT StockTransactionId, StockTransactionLotId, TickerId,
           BuyDate, BuyPrice, SellDate, SellPrice, Quantity,
           Profit, ProfitPercent
    FROM Investments.StockTransaction
    WHERE SellDate IS NULL
    ORDER BY StockTransactionId;
END;
GO
