CREATE OR ALTER PROCEDURE Investments.StockTransactionReadYearlyProfit
AS
BEGIN
    SET NOCOUNT ON;

    -- Realized profit is attributed to the sale year, including losses.
    SELECT
        YEAR(SellDate) AS SaleYear,
        COUNT_BIG(*) AS TransactionsClosed,
        SUM(Profit) AS TotalProfit
    FROM Investments.StockTransaction
    WHERE SellDate IS NOT NULL
      AND SellPrice IS NOT NULL
    GROUP BY YEAR(SellDate)
    ORDER BY SaleYear;
END;
GO
