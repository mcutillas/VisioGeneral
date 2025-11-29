-- ============================================
-- VisioGeneral - Script de creacio de Base de Dades
-- GREC - Grup d'Educadors de Carrer
-- ============================================

-- Crear la base de dades
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'VisioGeneral')
BEGIN
    CREATE DATABASE VisioGeneral;
END
GO

USE VisioGeneral;
GO

-- ============================================
-- TAULES PRINCIPALS D'ESTRUCTURA ORGANITZATIVA
-- ============================================

CREATE TABLE Areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Codi NVARCHAR(20) NOT NULL UNIQUE,
    Color NVARCHAR(7) NOT NULL,
    Ordre INT NOT NULL DEFAULT 0,
    Activa BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL
);
GO

CREATE TABLE Serveis (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AreaId INT NOT NULL,
    Nom NVARCHAR(200) NOT NULL,
    Descripcio NVARCHAR(500) NULL,
    Ordre INT NOT NULL DEFAULT 0,
    Actiu BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,
    CONSTRAINT FK_Serveis_Areas FOREIGN KEY (AreaId) REFERENCES Areas(Id)
);
GO

CREATE TABLE Directors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Cognoms NVARCHAR(150) NULL,
    Email NVARCHAR(200) NULL,
    Telefon NVARCHAR(20) NULL,
    Tipus NVARCHAR(50) NOT NULL,
    UsernameAD NVARCHAR(100) NULL,
    Actiu BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL
);
GO

CREATE TABLE Programes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ServeiId INT NOT NULL,
    DirectorId INT NULL,
    Nom NVARCHAR(200) NOT NULL,
    NomCurt NVARCHAR(50) NULL,
    Descripcio NVARCHAR(1000) NULL,
    NumTreballadors INT NOT NULL DEFAULT 0,
    NumUsuaris INT NULL,
    Estat NVARCHAR(50) NOT NULL DEFAULT 'Actiu',
    EsLiniaCreixement BIT NOT NULL DEFAULT 0,
    EsNou BIT NOT NULL DEFAULT 0,
    DataInici DATE NULL,
    DataFi DATE NULL,
    Ordre INT NOT NULL DEFAULT 0,
    Actiu BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,
    CONSTRAINT FK_Programes_Serveis FOREIGN KEY (ServeiId) REFERENCES Serveis(Id),
    CONSTRAINT FK_Programes_Directors FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);
GO

-- ============================================
-- TAULES DE GESTIO DE QUESTIONS
-- ============================================

CREATE TABLE EstatsQuestio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Descripcio NVARCHAR(500) NULL,
    Color NVARCHAR(7) NOT NULL,
    Ordre INT NOT NULL DEFAULT 0,
    EsFinal BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE OrgansGovern (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Descripcio NVARCHAR(500) NULL,
    Ordre INT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Questions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titol NVARCHAR(300) NOT NULL,
    Descripcio NVARCHAR(MAX) NULL,
    EstatId INT NOT NULL,
    Prioritat NVARCHAR(20) NOT NULL DEFAULT 'Normal',
    ProgramaId INT NULL,
    AreaId INT NULL,
    DirectorResponsableId INT NULL,
    DirectorCreadorId INT NULL,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,
    DataLimitResposta DATE NULL,
    DataResolucio DATETIME2 NULL,
    OrigenExtern BIT NOT NULL DEFAULT 0,
    FontExterna NVARCHAR(200) NULL,
    CONSTRAINT FK_Questio_Estat FOREIGN KEY (EstatId) REFERENCES EstatsQuestio(Id),
    CONSTRAINT FK_Questio_Programa FOREIGN KEY (ProgramaId) REFERENCES Programes(Id),
    CONSTRAINT FK_Questio_Area FOREIGN KEY (AreaId) REFERENCES Areas(Id),
    CONSTRAINT FK_Questio_DirectorResp FOREIGN KEY (DirectorResponsableId) REFERENCES Directors(Id),
    CONSTRAINT FK_Questio_DirectorCreador FOREIGN KEY (DirectorCreadorId) REFERENCES Directors(Id)
);
GO

CREATE TABLE HistorialQuestio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestioId INT NOT NULL,
    EstatAnteriorId INT NULL,
    EstatNouId INT NOT NULL,
    Comentari NVARCHAR(1000) NULL,
    DirectorId INT NULL,
    DataCanvi DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Historial_Questio FOREIGN KEY (QuestioId) REFERENCES Questions(Id),
    CONSTRAINT FK_Historial_EstatAnt FOREIGN KEY (EstatAnteriorId) REFERENCES EstatsQuestio(Id),
    CONSTRAINT FK_Historial_EstatNou FOREIGN KEY (EstatNouId) REFERENCES EstatsQuestio(Id),
    CONSTRAINT FK_Historial_Director FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);
GO

CREATE TABLE ComentarisQuestio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestioId INT NOT NULL,
    DirectorId INT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,
    CONSTRAINT FK_Comentari_Questio FOREIGN KEY (QuestioId) REFERENCES Questions(Id),
    CONSTRAINT FK_Comentari_Director FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);
GO

CREATE TABLE QuestioOrgans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestioId INT NOT NULL,
    OrganId INT NOT NULL,
    DataReunio DATE NOT NULL,
    Resultat NVARCHAR(500) NULL,
    PendentDe NVARCHAR(200) NULL,
    CONSTRAINT FK_QO_Questio FOREIGN KEY (QuestioId) REFERENCES Questions(Id),
    CONSTRAINT FK_QO_Organ FOREIGN KEY (OrganId) REFERENCES OrgansGovern(Id)
);
GO

-- ============================================
-- TAULES PER REUNIONS
-- ============================================

CREATE TABLE Reunions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titol NVARCHAR(200) NOT NULL,
    Tipus NVARCHAR(50) NOT NULL,
    Data DATETIME2 NOT NULL,
    Lloc NVARCHAR(200) NULL,
    Notes NVARCHAR(MAX) NULL,
    Realitzada BIT NOT NULL DEFAULT 0,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE ReunioParticipants (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReunioId INT NOT NULL,
    DirectorId INT NOT NULL,
    EsConvocant BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_RP_Reunio FOREIGN KEY (ReunioId) REFERENCES Reunions(Id),
    CONSTRAINT FK_RP_Director FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);
GO

CREATE TABLE ReunioQuestions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReunioId INT NOT NULL,
    QuestioId INT NOT NULL,
    Ordre INT NOT NULL DEFAULT 0,
    Tractada BIT NOT NULL DEFAULT 0,
    NotesReunio NVARCHAR(MAX) NULL,
    CONSTRAINT FK_RQ_Reunio FOREIGN KEY (ReunioId) REFERENCES Reunions(Id),
    CONSTRAINT FK_RQ_Questio FOREIGN KEY (QuestioId) REFERENCES Questions(Id)
);
GO

-- ============================================
-- TAULES PER CONTEXT ANUAL
-- ============================================

CREATE TABLE ContextAnual (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    [Any] INT NOT NULL UNIQUE,
    TotalTreballadors INT NULL,
    TotalUsuaris INT NULL,
    TotalProgrames INT NULL,
    ReflexioGeneral NVARCHAR(MAX) NULL,
    ReptesPrincipals NVARCHAR(MAX) NULL,
    ExitsPrincipals NVARCHAR(MAX) NULL,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL
);
GO

CREATE TABLE HistoricProgrames (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramaId INT NOT NULL,
    [Any] INT NOT NULL,
    NumTreballadors INT NULL,
    NumUsuaris INT NULL,
    Estat NVARCHAR(50) NULL,
    Notes NVARCHAR(1000) NULL,
    DataRegistre DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_HP_Programa FOREIGN KEY (ProgramaId) REFERENCES Programes(Id),
    CONSTRAINT UQ_HP_ProgramaAny UNIQUE (ProgramaId, [Any])
);
GO

-- ============================================
-- INDEXS PER RENDIMENT
-- ============================================

CREATE INDEX IX_Serveis_AreaId ON Serveis(AreaId);
CREATE INDEX IX_Programes_ServeiId ON Programes(ServeiId);
CREATE INDEX IX_Programes_DirectorId ON Programes(DirectorId);
CREATE INDEX IX_Programes_Estat ON Programes(Estat);
CREATE INDEX IX_Questio_EstatId ON Questions(EstatId);
CREATE INDEX IX_Questio_ProgramaId ON Questions(ProgramaId);
CREATE INDEX IX_Questio_AreaId ON Questions(AreaId);
CREATE INDEX IX_Questio_Prioritat ON Questions(Prioritat);
CREATE INDEX IX_Questio_DataCreacio ON Questions(DataCreacio DESC);
CREATE INDEX IX_HistorialQuestio_QuestioId ON HistorialQuestio(QuestioId);
CREATE INDEX IX_ComentarisQuestio_QuestioId ON ComentarisQuestio(QuestioId);
CREATE INDEX IX_Reunions_Data ON Reunions(Data);
CREATE INDEX IX_Reunions_Tipus ON Reunions(Tipus);
GO

PRINT 'Base de dades VisioGeneral creada correctament!';
GO
