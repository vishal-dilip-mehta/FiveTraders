CREATE TABLE Investments.Ticker
(
    TickerId INT IDENTITY(1,1) NOT NULL,
    TickerName NVARCHAR(10) NOT NULL,
    CONSTRAINT PK_Ticker PRIMARY KEY (TickerId)
);
GO
