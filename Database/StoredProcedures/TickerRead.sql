CREATE OR ALTER PROCEDURE Investments.TickerRead
    @TickerId INT = NULL,
    @TicketName NVARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Omit all filters to read every ticker.
    SELECT TickerId, TickerName
    FROM Investments.Ticker
    WHERE (@TickerId IS NULL OR TickerId = @TickerId)
      AND (@TicketName IS NULL OR TickerName = @TicketName)
    ORDER BY TickerId;
END;
GO
