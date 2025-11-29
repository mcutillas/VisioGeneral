-- ============================================
-- VisioGeneral - Script de creació de Base de Dades
-- GREC - Grup d'Educadors de Carrer
-- ============================================
-- Versió: 1.0
-- Data: 2025-11-29
-- ============================================

-- Crear la base de dades (executar com a admin)
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

-- Àrees de l'entitat (6 àrees segons document)
CREATE TABLE Areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Codi NVARCHAR(20) NOT NULL UNIQUE,  -- Per identificar fàcilment: COM, INF, INS, SAL, PEN, ADM
    Color NVARCHAR(7) NOT NULL,          -- Color hexadecimal per visualització (#5b9aa0)
    Ordre INT NOT NULL DEFAULT 0,        -- Ordre de visualització
    Activa BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL
);

-- Serveis dins de cada àrea
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

-- Directors/es de l'equip de direcció
CREATE TABLE Directors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Cognoms NVARCHAR(150) NULL,
    Email NVARCHAR(200) NULL,
    Telefon NVARCHAR(20) NULL,
    Tipus NVARCHAR(50) NOT NULL,         -- 'Tecnic' o 'Gerencia'
    UsernameAD NVARCHAR(100) NULL,       -- Per vincular amb Active Directory
    Actiu BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL
);

-- Programes (la unitat fonamental de treball)
CREATE TABLE Programes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ServeiId INT NOT NULL,
    DirectorId INT NULL,                  -- Director/a responsable
    Nom NVARCHAR(200) NOT NULL,
    NomCurt NVARCHAR(50) NULL,           -- Per mostrar en espais petits del Treemap
    Descripcio NVARCHAR(1000) NULL,

    -- Mètriques actuals (s'actualitzen manualment o via integració futura)
    NumTreballadors INT NOT NULL DEFAULT 0,
    NumUsuaris INT NULL,

    -- Estat del programa
    Estat NVARCHAR(50) NOT NULL DEFAULT 'Actiu',  -- Actiu, Creixement, Parat, Finalitzat
    EsLiniaCreixement BIT NOT NULL DEFAULT 0,
    EsNou BIT NOT NULL DEFAULT 0,                  -- Programa nou aquest any

    -- Metadades
    DataInici DATE NULL,
    DataFi DATE NULL,                              -- NULL si és indefinit
    Ordre INT NOT NULL DEFAULT 0,
    Actiu BIT NOT NULL DEFAULT 1,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,

    CONSTRAINT FK_Programes_Serveis FOREIGN KEY (ServeiId) REFERENCES Serveis(Id),
    CONSTRAINT FK_Programes_Directors FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);

-- ============================================
-- TAULES DE GESTIÓ DE QÜESTIONS
-- ============================================

-- Estats possibles de les qüestions
CREATE TABLE EstatsQuestio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Descripcio NVARCHAR(500) NULL,
    Color NVARCHAR(7) NOT NULL,          -- Color per badges
    Ordre INT NOT NULL DEFAULT 0,
    EsFinal BIT NOT NULL DEFAULT 0       -- Si és un estat final (Resolta, etc.)
);

-- Òrgans de govern on es discuteixen les qüestions
CREATE TABLE OrgansGovern (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,          -- Direcció, Junta, Assemblea, Comitè d'Empresa
    Descripcio NVARCHAR(500) NULL,
    Ordre INT NOT NULL DEFAULT 0
);

-- Qüestions (el cor del sistema)
CREATE TABLE Qüestions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titol NVARCHAR(300) NOT NULL,
    Descripcio NVARCHAR(MAX) NULL,

    -- Classificació
    EstatId INT NOT NULL,
    Prioritat NVARCHAR(20) NOT NULL DEFAULT 'Normal',  -- Urgent, Alta, Normal, Baixa

    -- Relacions opcionals (una qüestió pot afectar un programa, àrea, o ser general)
    ProgramaId INT NULL,
    AreaId INT NULL,

    -- Responsable i seguiment
    DirectorResponsableId INT NULL,       -- Qui porta el tema
    DirectorCreadorId INT NULL,           -- Qui l'ha creat

    -- Dates
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,
    DataLimitResposta DATE NULL,          -- Data límit si n'hi ha
    DataResolucio DATETIME2 NULL,

    -- Context
    OrigenExtern BIT NOT NULL DEFAULT 0,  -- Si ve d'una font externa
    FontExterna NVARCHAR(200) NULL,       -- Qui/què ha originat la qüestió

    CONSTRAINT FK_Questio_Estat FOREIGN KEY (EstatId) REFERENCES EstatsQuestio(Id),
    CONSTRAINT FK_Questio_Programa FOREIGN KEY (ProgramaId) REFERENCES Programes(Id),
    CONSTRAINT FK_Questio_Area FOREIGN KEY (AreaId) REFERENCES Areas(Id),
    CONSTRAINT FK_Questio_DirectorResp FOREIGN KEY (DirectorResponsableId) REFERENCES Directors(Id),
    CONSTRAINT FK_Questio_DirectorCreador FOREIGN KEY (DirectorCreadorId) REFERENCES Directors(Id)
);

-- Historial de canvis d'estat de les qüestions
CREATE TABLE HistorialQuestio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestioId INT NOT NULL,
    EstatAnteriorId INT NULL,
    EstatNouId INT NOT NULL,
    Comentari NVARCHAR(1000) NULL,
    DirectorId INT NULL,                  -- Qui ha fet el canvi
    DataCanvi DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Historial_Questio FOREIGN KEY (QuestioId) REFERENCES Qüestions(Id),
    CONSTRAINT FK_Historial_EstatAnt FOREIGN KEY (EstatAnteriorId) REFERENCES EstatsQuestio(Id),
    CONSTRAINT FK_Historial_EstatNou FOREIGN KEY (EstatNouId) REFERENCES EstatsQuestio(Id),
    CONSTRAINT FK_Historial_Director FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);

-- Comentaris a les qüestions
CREATE TABLE ComentarisQuestio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestioId INT NOT NULL,
    DirectorId INT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL,

    CONSTRAINT FK_Comentari_Questio FOREIGN KEY (QuestioId) REFERENCES Qüestions(Id),
    CONSTRAINT FK_Comentari_Director FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);

-- Vinculació de qüestions amb òrgans de govern (on s'ha parlat)
CREATE TABLE QuestioOrgans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestioId INT NOT NULL,
    OrganId INT NOT NULL,
    DataReunio DATE NOT NULL,
    Resultat NVARCHAR(500) NULL,          -- Resum del que es va decidir
    PendentDe NVARCHAR(200) NULL,         -- Si queda pendent d'alguna cosa

    CONSTRAINT FK_QO_Questio FOREIGN KEY (QuestioId) REFERENCES Qüestions(Id),
    CONSTRAINT FK_QO_Organ FOREIGN KEY (OrganId) REFERENCES OrgansGovern(Id)
);

-- ============================================
-- TAULES PER REUNIONS I PREPARACIÓ
-- ============================================

-- Reunions (bilaterals, equip direcció, etc.)
CREATE TABLE Reunions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titol NVARCHAR(200) NOT NULL,
    Tipus NVARCHAR(50) NOT NULL,          -- Bilateral, EquipDireccio, Junta, Assemblea
    Data DATETIME2 NOT NULL,
    Lloc NVARCHAR(200) NULL,
    Notes NVARCHAR(MAX) NULL,
    Realitzada BIT NOT NULL DEFAULT 0,
    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Participants de la reunió
CREATE TABLE ReunioParticipants (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReunioId INT NOT NULL,
    DirectorId INT NOT NULL,
    EsConvocant BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_RP_Reunio FOREIGN KEY (ReunioId) REFERENCES Reunions(Id),
    CONSTRAINT FK_RP_Director FOREIGN KEY (DirectorId) REFERENCES Directors(Id)
);

-- Qüestions a tractar en una reunió
CREATE TABLE ReunioQüestions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReunioId INT NOT NULL,
    QuestioId INT NOT NULL,
    Ordre INT NOT NULL DEFAULT 0,
    Tractada BIT NOT NULL DEFAULT 0,
    NotesReunio NVARCHAR(MAX) NULL,

    CONSTRAINT FK_RQ_Reunio FOREIGN KEY (ReunioId) REFERENCES Reunions(Id),
    CONSTRAINT FK_RQ_Questio FOREIGN KEY (QuestioId) REFERENCES Qüestions(Id)
);

-- ============================================
-- TAULES PER CONTEXT ANUAL
-- ============================================

-- Context anual de l'entitat (per l'informe)
CREATE TABLE ContextAnual (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    [Any] INT NOT NULL UNIQUE,

    -- Dades globals
    TotalTreballadors INT NULL,
    TotalUsuaris INT NULL,
    TotalProgrames INT NULL,

    -- Reflexions de direcció (es van omplint durant l'any)
    ReflexioGeneral NVARCHAR(MAX) NULL,
    ReptesPrincipals NVARCHAR(MAX) NULL,
    ExitsPrincipals NVARCHAR(MAX) NULL,

    DataCreacio DATETIME2 NOT NULL DEFAULT GETDATE(),
    DataModificacio DATETIME2 NULL
);

-- Històric de mètriques de programes (snapshot anual)
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

-- ============================================
-- ÍNDEXS PER RENDIMENT
-- ============================================

CREATE INDEX IX_Serveis_AreaId ON Serveis(AreaId);
CREATE INDEX IX_Programes_ServeiId ON Programes(ServeiId);
CREATE INDEX IX_Programes_DirectorId ON Programes(DirectorId);
CREATE INDEX IX_Programes_Estat ON Programes(Estat);
CREATE INDEX IX_Questio_EstatId ON Qüestions(EstatId);
CREATE INDEX IX_Questio_ProgramaId ON Qüestions(ProgramaId);
CREATE INDEX IX_Questio_AreaId ON Qüestions(AreaId);
CREATE INDEX IX_Questio_Prioritat ON Qüestions(Prioritat);
CREATE INDEX IX_Questio_DataCreacio ON Qüestions(DataCreacio DESC);
CREATE INDEX IX_HistorialQuestio_QuestioId ON HistorialQuestio(QuestioId);
CREATE INDEX IX_ComentarisQuestio_QuestioId ON ComentarisQuestio(QuestioId);
CREATE INDEX IX_Reunions_Data ON Reunions(Data);
CREATE INDEX IX_Reunions_Tipus ON Reunions(Tipus);

GO

PRINT 'Base de dades VisioGeneral creada correctament!';
GO
