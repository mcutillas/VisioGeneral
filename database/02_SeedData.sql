-- ============================================
-- VisioGeneral - Dades inicials (Seed Data)
-- GREC - Grup d'Educadors de Carrer
-- ============================================
-- Versió: 1.0
-- Data: 2025-11-29
-- ============================================

USE VisioGeneral;
GO

-- ============================================
-- DIRECTORS/ES DE L'EQUIP DE DIRECCIÓ
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
-- ÀREES DE L'ENTITAT
-- ============================================

INSERT INTO Areas (Nom, Codi, Color, Ordre)
VALUES
    ('Intervenció Comunitària', 'COM', '#5b9aa0', 1),
    ('Infància, Joves i Família', 'INF', '#6b9b7a', 2),
    ('Inserció Laboral', 'INS', '#7a9ec2', 3),
    ('Salut Mental i Discapacitat', 'SAL', '#9b8bb4', 4),
    ('Població Penitenciària', 'PEN', '#8a9a9e', 5),
    ('Administració', 'ADM', '#a0927b', 6);

GO

-- ============================================
-- ÒRGANS DE GOVERN
-- ============================================

INSERT INTO OrgansGovern (Nom, Descripcio, Ordre)
VALUES
    ('Direcció', 'Equip de direcció col·legiat - Reunions setmanals', 1),
    ('Junta', 'Junta directiva de l''associació', 2),
    ('Assemblea', 'Assemblea general de socis', 3),
    ('Comitè d''Empresa', 'Representació dels treballadors', 4);

GO

-- ============================================
-- ESTATS DE LES QÜESTIONS
-- ============================================

INSERT INTO EstatsQuestio (Nom, Descripcio, Color, Ordre, EsFinal)
VALUES
    ('Pendent', 'Qüestió nova pendent de tractar', '#6c757d', 1, 0),
    ('En espera d''informació externa', 'Esperant resposta o dades d''un tercer', '#17a2b8', 2, 0),
    ('Ajornada', 'Posposada per decisió de l''equip', '#ffc107', 3, 0),
    ('En seguiment', 'S''està treballant activament', '#007bff', 4, 0),
    ('Comentada en direcció, pendent de Junta', 'Tractada en direcció, cal portar a Junta', '#6f42c1', 5, 0),
    ('Parlada en Junta, pendent d''Assemblea', 'Aprovada en Junta, cal ratificar en Assemblea', '#e83e8c', 6, 0),
    ('Parlada en Assemblea', 'Tractada en Assemblea general', '#20c997', 7, 0),
    ('Resolta', 'Qüestió tancada i resolta', '#28a745', 8, 1);

GO

-- ============================================
-- SERVEIS I PROGRAMES
-- ============================================

-- Variables per IDs
DECLARE @AreaCOM INT = (SELECT Id FROM Areas WHERE Codi = 'COM');
DECLARE @AreaINF INT = (SELECT Id FROM Areas WHERE Codi = 'INF');
DECLARE @AreaINS INT = (SELECT Id FROM Areas WHERE Codi = 'INS');
DECLARE @AreaSAL INT = (SELECT Id FROM Areas WHERE Codi = 'SAL');
DECLARE @AreaPEN INT = (SELECT Id FROM Areas WHERE Codi = 'PEN');
DECLARE @AreaADM INT = (SELECT Id FROM Areas WHERE Codi = 'ADM');

-- Directors
DECLARE @Sebas INT = (SELECT Id FROM Directors WHERE Nom = 'Sebas');
DECLARE @Lorena INT = (SELECT Id FROM Directors WHERE Nom = 'Lorena');
DECLARE @Carme INT = (SELECT Id FROM Directors WHERE Nom = 'Carme');
DECLARE @Cati INT = (SELECT Id FROM Directors WHERE Nom = 'Cati');
DECLARE @Elisenda INT = (SELECT Id FROM Directors WHERE Nom = 'Elisenda');
DECLARE @Maria INT = (SELECT Id FROM Directors WHERE Nom = 'Maria');

-- ============================================
-- ÀREA: INTERVENCIÓ COMUNITÀRIA
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaCOM, 'Servei socioeducatiu en medi obert', 'Treball amb infants i joves en risc en el seu entorn comunitari', 1);

DECLARE @ServeiMediObert INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, EsLiniaCreixement, Ordre)
VALUES
    (@ServeiMediObert, @Sebas, 'Ajuntament de Palma', 'Palma', 12, 'Actiu', 0, 1),
    (@ServeiMediObert, @Sebas, 'Ajuntament de Llucmajor', 'Llucmajor', 8, 'Actiu', 1, 2),
    (@ServeiMediObert, @Sebas, 'Ajuntament de Sóller', 'Sóller', 4, 'Actiu', 0, 3),
    (@ServeiMediObert, @Sebas, 'Ajuntament d''Alcúdia', 'Alcúdia', 6, 'Actiu', 1, 4),
    (@ServeiMediObert, @Sebas, 'Altres municipis (Deià, Fornalutx, Esporles...)', 'Altres', 8, 'Actiu', 0, 5);

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaCOM, 'Altres serveis comunitaris', 'Programes complementaris d''àmbit comunitari', 2);

DECLARE @ServeiAltresCom INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiAltresCom, @Sebas, 'CaixaProInfància', 'CaixaPro', 5, 'Actiu', 1),
    (@ServeiAltresCom, @Sebas, 'Sempre Acompanyats', 'Sempre Ac.', 2, 'Actiu', 2);

-- ============================================
-- ÀREA: INFÀNCIA, JOVES I FAMÍLIA
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaINF, 'Intervenció familiar en el domicili', 'Suport a famílies en el seu entorn', 1);

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
VALUES (@AreaINF, 'Servei d''emancipació per a joves 18-25', 'Acompanyament a la vida adulta', 3);

DECLARE @ServeiEmancipacio INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, EsLiniaCreixement, Ordre)
VALUES
    (@ServeiEmancipacio, @Carme, 'Acompanyament a l''emancipació', 'Eman. Ac.', 10, 'Actiu', 1, 1),
    (@ServeiEmancipacio, @Carme, 'Habitatge per a l''emancipació', 'Eman. Hab.', 14, 'Actiu', 1, 2);

-- ============================================
-- ÀREA: INSERCIÓ LABORAL
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaINS, 'Serveis d''ocupació', 'Orientació i inserció laboral', 1);

DECLARE @ServeiOcupacio INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiOcupacio, @Lorena, 'Servei d''Orientació Professional (SOIB)', 'SOIB', 15, 'Actiu', 1),
    (@ServeiOcupacio, @Lorena, 'Incorpora (Clàssic, Autoocupació, Punts Formatius)', 'Incorpora', 6, 'Actiu', 2),
    (@ServeiOcupacio, @Lorena, 'Reincorpora', 'Reinc.', 4, 'Actiu', 3);

-- ============================================
-- ÀREA: SALUT MENTAL I DISCAPACITAT
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaSAL, 'Serveis per a persones amb diagnòstic de salut mental', 'Acompanyament i recursos residencials', 1);

DECLARE @ServeiSalutMental INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, EsLiniaCreixement, EsNou, Ordre)
VALUES
    (@ServeiSalutMental, @Elisenda, 'Servei d''acompanyament', 'SM Acomp.', 10, 'Actiu', 0, 0, 1),
    (@ServeiSalutMental, @Elisenda, 'Habitatge supervisat', 'SM Hab.', 8, 'Actiu', 1, 0, 2),
    (@ServeiSalutMental, @Elisenda, 'Domicilis compartits', 'SM Dom.', 4, 'Actiu', 1, 1, 3);

-- ============================================
-- ÀREA: POBLACIÓ PENITENCIÀRIA
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaPEN, 'Programes penitenciaris', 'Intervenció en context penitenciari i alternatives', 1);

DECLARE @ServeiPenitenciari INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiPenitenciari, @Cati, 'Mòdul de joves del centre penitenciari', 'Presó Joves', 5, 'Actiu', 1),
    (@ServeiPenitenciari, @Cati, 'Unitat Dependent del CIS', 'CIS', 4, 'Actiu', 2),
    (@ServeiPenitenciari, @Cati, 'Mesures Alternatives / Justícia Restaurativa', 'Mes. Alt.', 3, 'Parat', 3);

-- ============================================
-- ÀREA: ADMINISTRACIÓ
-- ============================================

INSERT INTO Serveis (AreaId, Nom, Descripcio, Ordre)
VALUES (@AreaADM, 'Processos de gestió', 'Serveis interns de suport', 1);

DECLARE @ServeiAdmin INT = SCOPE_IDENTITY();

INSERT INTO Programes (ServeiId, DirectorId, Nom, NomCurt, NumTreballadors, Estat, Ordre)
VALUES
    (@ServeiAdmin, @Maria, 'Gestió econòmica', 'Economia', 4, 'Actiu', 1),
    (@ServeiAdmin, @Maria, 'Gestió de recursos humans', 'RRHH', 3, 'Actiu', 2),
    (@ServeiAdmin, @Maria, 'Gestió de qualitat', 'Qualitat', 2, 'Actiu', 3),
    (@ServeiAdmin, @Maria, 'Planificació', 'Planif.', 3, 'Actiu', 4);

GO

-- ============================================
-- CONTEXT ANUAL 2025
-- ============================================

INSERT INTO ContextAnual ([Any], TotalTreballadors, TotalProgrames)
VALUES (2025, 179, 22);

GO

-- ============================================
-- ALGUNES QÜESTIONS D'EXEMPLE
-- ============================================

DECLARE @EstatPendent INT = (SELECT Id FROM EstatsQuestio WHERE Nom = 'Pendent');
DECLARE @EstatSeguiment INT = (SELECT Id FROM EstatsQuestio WHERE Nom = 'En seguiment');
DECLARE @EstatJunta INT = (SELECT Id FROM EstatsQuestio WHERE Nom = 'Comentada en direcció, pendent de Junta');
DECLARE @EstatEspera INT = (SELECT Id FROM EstatsQuestio WHERE Nom = 'En espera d''informació externa');

DECLARE @Sebas2 INT = (SELECT Id FROM Directors WHERE Nom = 'Sebas');
DECLARE @Elisenda2 INT = (SELECT Id FROM Directors WHERE Nom = 'Elisenda');
DECLARE @Carme2 INT = (SELECT Id FROM Directors WHERE Nom = 'Carme');

DECLARE @AreaSAL2 INT = (SELECT Id FROM Areas WHERE Codi = 'SAL');
DECLARE @AreaINF2 INT = (SELECT Id FROM Areas WHERE Codi = 'INF');

INSERT INTO Qüestions (Titol, Descripcio, EstatId, Prioritat, AreaId, DirectorResponsableId, DirectorCreadorId)
VALUES
    ('Taules salarials del conveni de discapacitat',
     'Cal revisar les taules salarials del conveni de discapacitat per als nous contractes del servei d''habitatge supervisat.',
     @EstatSeguiment, 'Urgent', @AreaSAL2, @Elisenda2, @Elisenda2),

    ('Ratios nous habitatges supervisats',
     'Definir els ratios de personal per als nous habitatges que obrirem el 2025.',
     @EstatJunta, 'Alta', @AreaSAL2, @Elisenda2, @Elisenda2),

    ('Ampliació servei Alcúdia',
     'L''Ajuntament d''Alcúdia vol ampliar el servei. Pendent de reunió per definir condicions.',
     @EstatEspera, 'Normal', NULL, @Sebas2, @Sebas2),

    ('Renovació conveni Educadors Familiars',
     'El conveni amb l''IMAS acaba el juny. Cal preparar proposta de renovació.',
     @EstatPendent, 'Alta', @AreaINF2, @Carme2, @Carme2);

GO

PRINT 'Dades inicials carregades correctament!';
GO
