# VisioGeneral - Backlog de Tasques Pendents

Darrera actualització: 2025-11-29

---

## Noves Funcionalitats Proposades (29/11/2025)

### 1. Qüestions Recurrents / Cícliques (Complexitat Alta)
> **Context**: Dins de l'àrea administrativa hi ha qüestions que es repeteixen cada any o en cicles regulars: controls de pressupost, pressupostos provisionals, revisions de qualitat, revisions de Fundació Lealtad, xarxes en què es participa, negociació de preus de serveis, etc.
>
> Aquestes no són tasques "noves" sinó processos habituals que generen "coses per pensar" i poden tenir dates d'inici/fi previsibles.
>
> **Proposta de solució**:
> - Afegir camp `EsRecurrent` (bool) a Qüestió
> - Afegir camps `Periodicitat` (Anual, Trimestral, Mensual, Puntual) i `MesProgramat` (per saber quan toca)
> - Crear vista "Calendari anual de qüestions recurrents"
> - Opció de "Reobrir" una qüestió recurrent quan arribi el seu cicle
> - O bé: crear automàticament una nova instància quan s'acosta la data
>
> **Alternativa més simple**:
> - Crear una secció separada "Processos habituals" que no es barregi amb les qüestions "grans"
> - Cada procés té un checklist anual que es reinicia cada any

### ~~2. Subtasques dins d'una Qüestió~~ ✅ COMPLETAT (29/11/2025)
> Implementat sistema complet de subtasques dins de cada qüestió:
> - Nova entitat `Subtasca` amb camps: Títol, Descripció, DirectorAssignat, Estat, DataLimit, Ordre
> - Sistema d'estats: Pendent, En Curs, Completada (amb indicadors visuals de color)
> - Nova entitat `ComentariSubtasca` per comentaris de seguiment
> - Vista de detall amb llista de subtasques amb indicadors d'estat
> - Indicador de progrés visual (barra + comptador "3/7")
> - Formulari desplegable per afegir noves subtasques
> - Secció "Accions i seguiment" per canviar estat i afegir comentaris
> - Assignació de responsable individual per subtasca
> - Data límit opcional amb indicador visual de vençuda
> - Historial de comentaris amb badge d'estat quan es fa canvi

---

## Pendent de Reflexió / Decisió

### Revisió d'Estats i Prioritats
> **IMPORTANT**: Cal repensar l'estructura d'estats i prioritats. Potser tenim massa opcions o massa dimensions.
>
> **Situació actual:**
> - Prioritats: Urgent, Alta, Normal, Baixa
> - Estats: Pendent, En espera d'informació externa, Ajornada, En seguiment, Comentada en direcció pendent de Junta, Parlada en Junta pendent d'Assemblea, Parlada en Assemblea, Resolta
>
> **Preguntes a respondre:**
> - Els estats reflecteixen realment el flux de treball?
> - Cal simplificar?
> - La prioritat i l'estat es solapen en algun cas?

---

## Millores Immediates (Feedback Maria - 29/11/2025)

### Fetes
- [x] **Cercador sense distinció d'accents** - Que "questio" trobi "qüestió" ✅
- [x] **Botó "Netejar filtres"** al llistat de qüestions ✅
- [x] **Motiu d'eliminació obligatori** - Quan s'elimina una qüestió, demana el motiu ✅
  - Soft delete implementat amb camps `Eliminada`, `MotiuEliminacio`, `DataEliminacio`, `DirectorEliminadorId`
  - Formulari d'eliminació amb motiu obligatori (mínim 10 caràcters)
- [x] **Històric de qüestions tancades** - Vista separada per qüestions resoltes/eliminades ✅
  - Nova vista `/Qüestions/Historic` amb filtres per tipus (resoltes/eliminades)
  - Detall de qüestió històrica amb informació de tancament
  - Menú desplegable "Qüestions" amb opcions "Actives" i "Històric"

### Pendents (ordenades per complexitat)

#### Complexitat Mitjana
- [x] **Múltiples directors responsables** - Poder assignar més d'un director a una qüestió ✅
  - Taula intermèdia `QuestioDirector` creada
  - Formularis amb checkboxes per selecció múltiple
  - Opció "Tot l'Equip de Direcció" per qüestions globals
  - Vistes actualitzades per mostrar múltiples responsables

#### Complexitat Mitjana-Alta
- [ ] **Comentaris niuats (estil fòrum)** - Poder respondre dins d'un comentari
  - Afegir camp `ComentariPareId` a `ComentariQuestio`
  - Modificar vista per mostrar comentaris en arbre
  - Afegir botó "Respondre" a cada comentari

---

## Funcionalitats Pendents

### Qüestions
- [x] Llistat de qüestions amb filtres (àrea, estat, prioritat, dates)
- [x] Cercador de qüestions per títol/descripció (sense distinció d'accents)
- [x] Crear nova qüestió
- [x] Detall de qüestió amb historial complet
- [x] Editar qüestió
- [x] Canviar estat amb comentari
- [x] Afegir comentaris a una qüestió
- [ ] Registrar pas per òrgans de govern (Direcció → Junta → Assemblea)
- [ ] Adjuntar documents a qüestions
- [ ] Exportar qüestions a PDF/Excel

### Dashboard i Visualització
- [x] Treemap interactiu amb programes per àrea
- [x] Una fila per àrea, programes amb mida proporcional als treballadors
- [ ] Panel lateral al clicar programa (millorar contingut)
- [ ] Mostrar qüestions pendents per programa al panel
- [ ] Gràfics d'evolució de qüestions
- [ ] Vista de calendari amb dates límit

### Programes i Àrees
- [ ] CRUD de Programes
- [ ] CRUD d'Àrees
- [ ] CRUD de Serveis
- [ ] Gestió de Directors

### Autenticació i Seguretat
- [ ] Integració amb Windows AD (Active Directory)
- [ ] Rols i permisos per director/àrea
- [ ] Auditoria de canvis (qui va fer què i quan)

### Plans i Seguiment
- [ ] Fitxes de programa (plans anuals)
- [ ] Seguiment d'objectius per programa
- [ ] Indicadors i KPIs
- [ ] Alertes i notificacions

### Context Anual
- [ ] Gestió del context anual (pressupost, prioritats)
- [ ] Comparativa any a any
- [ ] Històric d'anys anteriors

### Informes i Exportació
- [ ] Informe resum per direcció
- [ ] Informe per àrea
- [ ] Exportació a Word/PDF per presentar a Junta
- [ ] Generació d'actes de reunió

### Millores Tècniques
- [ ] Paginació al llistat de qüestions
- [ ] Optimització de queries EF Core (QuerySplittingBehavior)
- [ ] Cache de dades freqüents
- [ ] Validació de formularis (client i servidor)
- [ ] Missatges de confirmació/error més amigables
- [ ] Tests unitaris i d'integració

---

## Idees per al Futur

- [ ] App mòbil o PWA per consultar des del mòbil
- [ ] Integració amb calendari (Outlook/Google)
- [ ] Notificacions per email
- [ ] Dashboard personalitzat per director
- [ ] Mode offline

---

## Notes i Decisions Tècniques

- **Paleta de colors**: Tons suaus (blaus, verds, turqueses, liles) - evitar vermells agressius
- **Idioma**: Català a la interfície
- **Base de dades**: SQL Server Express local (LAPTOP-90BJQNEB\SQLEXPRESS)
- **Treemap**: Una fila per àrea, programes amb mida proporcional als treballadors
- **Cercador**: Utilitza COLLATE SQL_Latin1_General_CP1_CI_AI per ignorar accents

---

## Completat (29/11/2025)

- [x] Esquema SQL de la base de dades
- [x] Projecte ASP.NET Core MVC
- [x] Configuració Entity Framework amb SQL Server
- [x] Models (entitats) C#
- [x] Seed data amb dades reals del GREC (6 àrees, 22 programes, 6 directors, 4 qüestions exemple)
- [x] Dashboard amb Treemap interactiu
- [x] Estils CSS amb paleta suau
- [x] CRUD complet de Qüestions
- [x] Filtres i cercador al llistat (sense distinció d'accents)
- [x] Canvi d'estat amb historial
- [x] Comentaris a qüestions
- [x] Botó "Netejar filtres"
- [x] Soft delete amb motiu obligatori
- [x] Històric de qüestions (resoltes/eliminades)
- [x] Múltiples directors responsables amb checkboxes
- [x] Opció "Tot l'Equip de Direcció" per qüestions globals
- [x] Repositori Git i pujada a GitHub (https://github.com/mcutillas/VisioGeneral)
- [x] Subtasques dins de qüestions amb indicador de progrés
- [x] Sistema d'estats per subtasques (Pendent, En Curs, Completada)
- [x] Comentaris de seguiment per subtasques
- [x] Reorganització de la vista de detalls (descripció a columna lateral)
- [x] Simplificació del menú de navegació
