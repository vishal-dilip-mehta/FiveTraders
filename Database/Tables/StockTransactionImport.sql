CREATE TABLE Investments.StockTransactionImport
(
    TickerName NVARCHAR(10) NOT NULL,
    BuyDate DATETIME2 NOT NULL,
    BuyPrice DECIMAL(19,4) NOT NULL,
    Quantity DECIMAL(19,4) NOT NULL,
    SellDate DATETIME2 NULL,
    SellPrice DECIMAL(19,4) NULL
);
GO
