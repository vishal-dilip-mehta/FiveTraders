CREATE OR ALTER PROCEDURE Investments.StockTransactionReadMonthlyProfit
AS
BEGIN
    SET NOCOUNT ON;

    -- Realized profit is attributed to the sale month, including losses.
    ;WITH MonthlyProfit AS
    (
        SELECT
            DATEFROMPARTS(YEAR(SellDate), MONTH(SellDate), 1) AS MonthStart,
            COUNT_BIG(*) AS TransactionsClosed,
            SUM(Profit) AS TotalProfit
        FROM Investments.StockTransaction
        WHERE SellDate IS NOT NULL
          AND SellPrice IS NOT NULL
        GROUP BY DATEFROMPARTS(YEAR(SellDate), MONTH(SellDate), 1)
    ),
    MonthBounds AS
    (
        SELECT MIN(MonthStart) AS FirstMonth, MAX(MonthStart) AS LastMonth
        FROM MonthlyProfit
    ),
    Months AS
    (
        SELECT FirstMonth AS MonthStart
        FROM MonthBounds
        WHERE FirstMonth IS NOT NULL

        UNION ALL

        SELECT DATEADD(MONTH, 1, m.MonthStart)
        FROM Months AS m
        CROSS JOIN MonthBounds AS b
        WHERE m.MonthStart < b.LastMonth
    )
    SELECT
        m.MonthStart,
        COALESCE(p.TransactionsClosed, CONVERT(BIGINT, 0)) AS TransactionsClosed,
        COALESCE(p.TotalProfit, 0) AS TotalProfit
    FROM Months AS m
    LEFT JOIN MonthlyProfit AS p ON p.MonthStart = m.MonthStart
    ORDER BY m.MonthStart
    OPTION (MAXRECURSION 0);
END;
GO
