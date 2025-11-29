-- ============================================
-- VisioGeneral - Dades inicials (Seed Data)
-- GREC - Grup d'Educadors de Carrer
-- ============================================

USE VisioGeneral;
GO

-- ============================================
-- DIRECTORS/ES DE L'EQUIP DE DIRECCIO
-- ============================================

INSERT INTO Directors (Nom, Cognoms, Tipus, Actiu)
VALUES
    ('Sebas', NULL, 'Tecnic', 1),
    ('Lorena', NULL, 'Tecnic', 1),
    ('Carme', NULL, 'Tecnic', 1),
    ('Cati', NULL, 'Tecnic', 1),
    ('Elisenda', NULL, 'Tecnic', 1),
    ('Maria', NULL, 'Gerencia', 1);
GO

-- ============================================
-- AREES DE L'ENTITAT
-- ============================================

INSERT INTO Areas (Nom, Codi, Color, Ordre)
VALUES
    (N'Intervenció Comunitària', 'COM', '#5b9aa0', 1),
    (N'Infància, Joves i Família', 'INF', '#6b9b7a', 2),
    (N'Inserció Laboral', 'INS', '#7a9ec2', 3),
    (N'Salut Mental i Discapacitat', 'SAL', '#9b8bb4', 4),
    (N'Població Penitenciària', 'PEN', '#8a9a9e', 5),
    (N'Administració', 'ADM', '#a0927b', 6);
GO

-- ============================================
-- ORGANS DE GOVERN
-- ============================================

INSERT INTO OrgansGovern (Nom, Descripcio, Ordre)
VALUES
    (N'Direcció', N'Equip de direcció col·legiat - Reunions setmanals', 1),
    ('Junta', N'Junta directiva de l''associació', 2),
    ('Assemblea', 'Assemblea general de socis', 3),
    (N'Comitè d''Empresa', N'Representació dels treballadors', 4);
GO

-- ============================================
-- ESTATS DE LES QUESTIONS
-- ============================================

INSERT INTO EstatsQuestio (Nom, Descripcio, Color, Ordre, EsFinal)
VALUES
    ('Pendent', N'Qüestió nova pendent de tractar', '#6c757d', 1, 0),
    (N'En espera d''informació externa', N'Esperant resposta o dades d''un tercer', '#17a2b8', 2, 0),
    ('Ajornada', N'Posposada per decisió de l''equip', '#ffc107', 3, 0),
    ('En seguiment', N'S''està treballant activament', '#007bff', 4, 0),
    (N'Comentada en direcció, pendent de Junta', N'Tractada en direcció, cal portar a Junta', '#6f42c1', 5, 0),
    (N'Parlada en Junta, pendent d''Assemblea', N'Aprovada en Junta, cal ratificar en Assemblea', '#e83e8c', 6, 0),
    ('Parlada en Assemblea', 'Tractada en Assemblea general', '#20c997', 7, 0),
    ('Resolta', N'Qüestió tancada i resolta', '#28a745', 8, 1);
GO

-- ============================================
-- SERVEIS I PROGRAMES
-- ============================================

DECLARE @AreaCOM INT = (SELECT Id FROM Areas WHERE Codi = 'COM');
DECLARE @AreaINF INT = (SELECT Id FROM Areas WHERE Codi = 'INF');
DECLARE @AreaINS INT = (SELECT Id FROM Areas WHERE Codi = 'INS');
DECLARE @AreaSAL INT = (SELECT Id FROM Areas WHERE Codi = 'SAL');
DECLARE @AreaPEN INT = (SELECT Id FROM Areas WHERE Codi = 'PEN');
DECLARE @AreaADM INT = (SELECT Id FROM Areas WHERE Codi = 'ADM');

DECLARE @Sebas INT = (SELECT Id FROM Directors WHERE Nom = 'Sebas');
DECLARE @Lorena INT = (SELECT Id FROM Directors WHERE Nom = 'Lorena');
DECLARE @Carme INT = (SELECT Id FROM Directors WHERE Nom = 'Carme');
DECLARE @Cati INT = (SELECT Id FROM Directors WHERE Nom = 'Cati');
DECLARE @Elisenda INT = (SELECT Id FROM Directors WHERE Nom = 'Elisenda');
DECLARE @Maria INT = (SELECT Id FROM Directors WHERE Nom = 'Maria');

-- ============================================
-- AREA: INTERVENCIO COMUNITARIA
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaCOM, N'Servei socioeducatiu en medi obert', N'Treball amb infants i joves en risc en el seu entorn comunitari', 1);

DECLARE @ServeiMediObert INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, EsLiniaCreixement, Ordre)
VALUES
    (@ServeiMediObert, @Sebas, 'Ajuntament de Palma', 'Palma', 12, 'Actiu', 0, 1),
    (@ServeiMediObert, @Sebas, 'Ajuntament de Llucmajor', 'Llucmajor', 8, 'Actiu', 1, 2),
    (@ServeiMediObert, @Sebas, N'Ajuntament de Sóller', N'Sóller', 4, 'Actiu', 0, 3),
    (@ServeiMediObert, @Sebas, N'Ajuntament d''Alcúdia', N'Alcúdia', 6, 'Actiu', 1, 4),
    (@ServeiMediObert, @Sebas, N'Altres municipis (Deià, Fornalutx, Esporles...)', 'Altres', 8, 'Actiu', 0, 5);

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaCOM, 'Altres serveis comunitaris', N'Programes complementaris d''àmbit comunitari', 2);

DECLARE @ServeiAltresCom INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiAltresCom, @Sebas, N'CaixaProInfància', 'CaixaPro', 5, 'Actiu', 1),
    (@ServeiAltresCom, @Sebas, 'Sempre Acompanyats', 'Sempre Ac.', 2, 'Actiu', 2);

-- ============================================
-- AREA: INFANCIA, JOVES I FAMILIA
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaINF, N'Intervenció familiar en el domicili', N'Suport a famílies en el seu entorn', 1);

DECLARE @ServeiFamilia INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiFamilia, @Carme, 'Educadors Familiars', 'Ed. Fam.', 18, 'Actiu', 1),
    (@ServeiFamilia, @Carme, 'Suport al Menor', 'Suport', 12, 'Actiu', 2);

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaINF, 'Acolliment residencial', 'Recursos residencials per a menors', 2);

DECLARE @ServeiResidencial INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiResidencial, @Carme, 'Llar de menors', 'Llar', 8, 'Actiu', 1);

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaINF, N'Servei d''emancipació per a joves 18-25', N'Acompanyament a la vida adulta', 3);

DECLARE @ServeiEmancipacio INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, EsLiniaCreixement, Ordre)
VALUES
    (@ServeiEmancipacio, @Carme, N'Acompanyament a l''emancipació', 'Eman. Ac.', 10, 'Actiu', 1, 1),
    (@ServeiEmancipacio, @Carme, N'Habitatge per a l''emancipació', 'Eman. Hab.', 14, 'Actiu', 1, 2);

-- ============================================
-- AREA: INSERCIO LABORAL
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaINS, N'Serveis d''ocupació', N'Orientació i inserció laboral', 1);

DECLARE @ServeiOcupacio INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiOcupacio, @Lorena, N'Servei d''Orientació Professional (SOIB)', 'SOIB', 15, 'Actiu', 1),
    (@ServeiOcupacio, @Lorena, N'Incorpora (Clàssic, Autoocupació, Punts Formatius)', 'Incorpora', 6, 'Actiu', 2),
    (@ServeiOcupacio, @Lorena, 'Reincorpora', 'Reinc.', 4, 'Actiu', 3);

-- ============================================
-- AREA: SALUT MENTAL I DISCAPACITAT
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaSAL, N'Serveis per a persones amb diagnòstic de salut mental', 'Acompanyament i recursos residencials', 1);

DECLARE @ServeiSalutMental INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, EsLiniaCreixement, EsNou, Ordre)
VALUES
    (@ServeiSalutMental, @Elisenda, N'Servei d''acompanyament', 'SM Acomp.', 10, 'Actiu', 0, 0, 1),
    (@ServeiSalutMental, @Elisenda, 'Habitatge supervisat', 'SM Hab.', 8, 'Actiu', 1, 0, 2),
    (@ServeiSalutMental, @Elisenda, 'Domicilis compartits', 'SM Dom.', 4, 'Actiu', 1, 1, 3);

-- ============================================
-- AREA: POBLACIO PENITENCIARIA
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaPEN, 'Programes penitenciaris', N'Intervenció en context penitenciari i alternatives', 1);

DECLARE @ServeiPenitenciari INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiPenitenciari, @Cati, N'Mòdul de joves del centre penitenciari', N'Presó Joves', 5, 'Actiu', 1),
    (@ServeiPenitenciari, @Cati, 'Unitat Dependent del CIS', 'CIS', 4, 'Actiu', 2),
    (@ServeiPenitenciari, @Cati, N'Mesures Alternatives / Justícia Restaurativa', 'Mes. Alt.', 3, 'Parat', 3);

-- ============================================
-- AREA: ADMINISTRACIO
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaADM, N'Processos de gestió', 'Serveis interns de suport', 1);

DECLARE @ServeiAdmin INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiAdmin, @Maria, N'Gestió econòmica', 'Economia', 4, 'Actiu', 1),
    (@ServeiAdmin, @Maria, N'Gestió de recursos humans', 'RRHH', 3, 'Actiu', 2),
    (@ServeiAdmin, @Maria, N'Gestió de qualitat', 'Qualitat', 2, 'Actiu', 3),
    (@ServeiAdmin, @Maria, N'Planificació', 'Planif.', 3, 'Actiu', 4);

GO

-- ============================================
-- CONTEXT ANUAL 2025
-- ============================================

INSERT INTO ContextAnual ([Any], TotalTreballadors, TotalProgrames)
VALUES (2025, 179, 22);
GO

-- ============================================
-- ALGUNES QUESTIONS D'EXEMPLE
-- ============================================

DECLARE @EstatPendent INT = (SELECT Id FROM EstatsQuestio WHERE Nom = 'Pendent');
DECLARE @EstatSeguiment INT = (SELECT Id FROM EstatsQuestio WHERE Nom = 'En seguiment');
DECLARE @EstatJunta INT = (SELECT Id FROM EstatsQuestio WHERE Nom = N'Comentada en direcció, pendent de Junta');
DECLARE @EstatEspera INT = (SELECT Id FROM EstatsQuestio WHERE Nom = N'En espera d''informació externa');

DECLARE @Sebas2 INT = (SELECT Id FROM Directors WHERE Nom = 'Sebas');
DECLARE @Elisenda2 INT = (SELECT Id FROM Directors WHERE Nom = 'Elisenda');
DECLARE @Carme2 INT = (SELECT Id FROM Directors WHERE Nom = 'Carme');

DECLARE @AreaSAL2 INT = (SELECT Id FROM Areas WHERE Codi = 'SAL');
DECLARE @AreaINF2 INT = (SELECT Id FROM Areas WHERE Codi = 'INF');

INSERT INTO Questions (Titol, Descripcio, EstatId, Prioritat, AreaId, DirectorResponsableId, DirectorCreadorId)
VALUES
    ('Taules salarials del conveni de discapacitat',
     N'Cal revisar les taules salarials del conveni de discapacitat per als nous contractes del servei d''habitatge supervisat.',
     @EstatSeguiment, 'Urgent', @AreaSAL2, @Elisenda2, @Elisenda2),

    ('Ratios nous habitatges supervisats',
     'Definir els ratios de personal per als nous habitatges que obrirem el 2025.',
     @EstatJunta, 'Alta', @AreaSAL2, @Elisenda2, @Elisenda2),

    (N'Ampliació servei Alcúdia',
     N'L''Ajuntament d''Alcúdia vol ampliar el servei. Pendent de reunió per definir condicions.',
     @EstatEspera, 'Normal', NULL, @Sebas2, @Sebas2),

    (N'Renovació conveni Educadors Familiars',
     N'El conveni amb l''IMAS acaba el juny. Cal preparar proposta de renovació.',
     @EstatPendent, 'Alta', @AreaINF2, @Carme2, @Carme2);

GO

PRINT 'Dades inicials carregades correctament!';
GO
