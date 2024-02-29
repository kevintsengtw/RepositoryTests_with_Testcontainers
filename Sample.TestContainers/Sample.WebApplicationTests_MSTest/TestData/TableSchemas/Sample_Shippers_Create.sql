CREATE TABLE Sample.dbo.Shippers
(
    ShipperID   int IDENTITY,
    CompanyName nvarchar(40) NOT NULL,
    Phone       nvarchar(24) NULL,
    CONSTRAINT PK_Shippers PRIMARY KEY CLUSTERED (ShipperID)
)
    ON [PRIMARY];