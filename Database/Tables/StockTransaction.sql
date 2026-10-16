CREATE TABLE Investments.StockTransaction
(
    StockTransactionId INT IDENTITY(1,1) NOT NULL,
    StockTransactionLotId INT NOT NULL,
    TickerId INT NOT NULL,
    BuyDate DATETIME2 NOT NULL,
    BuyPrice DECIMAL(19,4) NOT NULL,
    SellDate DATETIME2 NULL,
    SellPrice DECIMAL(19,4) NULL,
    Quantity DECIMAL(19,4) NOT NULL,
    Profit AS CAST((SellPrice - BuyPrice) * Quantity AS DECIMAL(19,4)),
    ProfitPercent AS CAST(
        ((SellPrice - BuyPrice) / NULLIF(BuyPrice, 0)) * 100
        AS DECIMAL(19,4)
    ),
    CONSTRAINT PK_StockTransaction PRIMARY KEY (StockTransactionId),
    CONSTRAINT FK_StockTransaction_Ticker
        FOREIGN KEY (TickerId)
        REFERENCES Investments.Ticker (TickerId)
);
GO
