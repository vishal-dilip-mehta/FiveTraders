IF SCHEMA_ID(N'Investments') IS NULL
    EXEC sys.sp_executesql N'CREATE SCHEMA Investments AUTHORIZATION dbo;';
GO
