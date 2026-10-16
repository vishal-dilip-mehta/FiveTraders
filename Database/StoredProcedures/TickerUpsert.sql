CREATE OR ALTER PROCEDURE Investments.TickerUpsert
    @TicketName NVARCHAR(10),
    @TickerId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Omit the ID to insert; supply an existing ID to update.
    IF @TickerId IS NULL
    BEGIN
        INSERT INTO Investments.Ticker (TickerName)
        OUTPUT inserted.TickerId, inserted.TickerName
        VALUES (@TicketName);
    END
    ELSE
    BEGIN
        UPDATE Investments.Ticker
        SET TickerName = @TicketName
        OUTPUT inserted.TickerId, inserted.TickerName
        WHERE TickerId = @TickerId;

        IF @@ROWCOUNT = 0
            THROW 50010, 'The specified TickerId does not exist.', 1;
    END;
END;
GO
