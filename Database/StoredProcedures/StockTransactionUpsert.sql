CREATE OR ALTER PROCEDURE Investments.StockTransactionUpsert
    @StockTransactionLotId INT,
    @TickerId INT,
    @BuyDate DATETIME2,
    @BuyPrice DECIMAL(19,4),
    @Quantity DECIMAL(19,4),
    @SellDate DATETIME2 = NULL,
    @SellPrice DECIMAL(19,4) = NULL,
    @StockTransactionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Omit the ID to insert; supply an existing ID to replace its editable values.
    -- Profit and ProfitPercent are calculated by the table.
    IF @StockTransactionId IS NULL
    BEGIN
        INSERT INTO Investments.StockTransaction
            (StockTransactionLotId, TickerId, BuyDate, BuyPrice,
             SellDate, SellPrice, Quantity)
        OUTPUT inserted.StockTransactionId, inserted.StockTransactionLotId,
               inserted.TickerId, inserted.BuyDate, inserted.BuyPrice,
               inserted.SellDate, inserted.SellPrice, inserted.Quantity,
               inserted.Profit, inserted.ProfitPercent
        VALUES
            (@StockTransactionLotId, @TickerId, @BuyDate, @BuyPrice,
             @SellDate, @SellPrice, @Quantity);
    END
    ELSE
    BEGIN
        UPDATE Investments.StockTransaction
        SET StockTransactionLotId = @StockTransactionLotId,
            TickerId = @TickerId,
            BuyDate = @BuyDate,
            BuyPrice = @BuyPrice,
            SellDate = @SellDate,
            SellPrice = @SellPrice,
            Quantity = @Quantity
        OUTPUT inserted.StockTransactionId, inserted.StockTransactionLotId,
               inserted.TickerId, inserted.BuyDate, inserted.BuyPrice,
               inserted.SellDate, inserted.SellPrice, inserted.Quantity,
               inserted.Profit, inserted.ProfitPercent
        WHERE StockTransactionId = @StockTransactionId;

        IF @@ROWCOUNT = 0
            THROW 50011, 'The specified StockTransactionId does not exist.', 1;
    END;
END;
GO
