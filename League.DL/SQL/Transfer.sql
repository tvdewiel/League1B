CREATE TABLE [dbo].[Transfer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SpelerId] INT NOT NULL, 
    [Prijs] INT NOT NULL, 
    [OudTeamId] INT NULL, 
    [NieuwTeamId] INT NULL,
    CONSTRAINT [FK_Transfer_Speler] FOREIGN KEY ([SpelerId]) REFERENCES [Speler]([Id]),
    CONSTRAINT [FK_Transfer_OudTeam] FOREIGN KEY ([OudTeamId]) REFERENCES [Team]([Stamnummer]),
    CONSTRAINT [FK_Transfer_NieuwTeam] FOREIGN KEY ([NieuwTeamId]) REFERENCES [Team]([Stamnummer])
)
