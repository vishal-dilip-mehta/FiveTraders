SET XACT_ABORT ON;

DECLARE @Inserted TABLE (StockTransactionId INT PRIMARY KEY);

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (
        SELECT 1
        FROM Investments.StockTransactionImport AS i
        WHERE NOT EXISTS (
            SELECT 1
            FROM Investments.Ticker AS t
            WHERE t.TickerName = i.TickerName
        )
    )
        THROW 50001, 'An imported ticker is missing from Investments.Ticker.', 1;

    TRUNCATE TABLE Investments.StockTransaction    
        
    INSERT INTO Investments.StockTransaction
    (
        StockTransactionLotId, TickerId,
        BuyDate, BuyPrice, Quantity, SellDate, SellPrice
    )
    OUTPUT inserted.StockTransactionId INTO @Inserted
    SELECT
        0, t.TickerId,
        i.BuyDate, i.BuyPrice, i.Quantity, i.SellDate, i.SellPrice
    FROM Investments.StockTransactionImport AS i
    JOIN (
        SELECT TickerName, MIN(TickerId) AS TickerId
        FROM Investments.Ticker
        GROUP BY TickerName
    ) AS t ON t.TickerName = i.TickerName;

    UPDATE s
    SET StockTransactionLotId = s.StockTransactionId
    FROM Investments.StockTransaction AS s
    JOIN @Inserted AS i
        ON i.StockTransactionId = s.StockTransactionId;

    COMMIT TRANSACTION;

    SELECT COUNT(*) AS TransactionsImported FROM @Inserted;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
