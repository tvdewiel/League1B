CREATE TABLE [dbo].[Speler]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Naam] VARCHAR(150) NOT NULL, 
    [Rugnummer] INT NULL, 
    [Lengte] INT NULL, 
    [Gewicht] INT NULL, 
    [TeamId] INT NULL,
    CONSTRAINT [FK_Speler_Team] FOREIGN KEY ([TeamId]) REFERENCES [Team]([Stamnummer])
)
