# VisioGeneral - Backlog de Tasques Pendents

Darrera actualització: 2025-11-29

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
- [ ] **Múltiples directors responsables** - Poder assignar més d'un director a una qüestió
  - Crear taula intermèdia `QuestioDirector`
  - Modificar formularis per permetre selecció múltiple
  - Actualitzar vistes per mostrar múltiples responsables

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
