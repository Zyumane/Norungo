# Dawn Fall Rain
### *A game by Zyumane Zye — Norungo Project*

---

## 🇲🇽 Español

### Descripción
**Dawn Fall Rain** es un juego de horror psicológico, sigilo y supervivencia en tercera persona desarrollado en Unity como proyecto de graduación de la carrera de Ingeniería en Programación y Diseño de Videojuegos.

El jugador controla a **Rom Origo**, un hombre desempleado que queda atrapado en **Orevono** — una ciudad industrial abandonada rodeada por un domo de oscuridad permanente. Sin combate, sin armas: solo ingenio, sigilo y la capacidad de leer el entorno para sobrevivir.

### Objetivo
Recuperar las **5 refracciones** de la motocicleta distribuidas en cinco actos y escapar de Orevono antes de ser consumido por la entidad que habita la ciudad.

### Mecánicas principales
- **Sigilo puro** — no existe sistema de combate. Ser detectado activa un sistema de tres esferas de proximidad: la primera alerta al reanimado, la segunda daña la cordura del jugador, la tercera es game over inmediato.
- **Dos tipos de reanimados** — Auditivo (detecta sonido, distraíble con objetos arrojables) y Visual (cono de visión tipo linterna, ciego al sonido). Nunca coexisten en el mismo nodo.
- **Sistema de cordura** — la salud del jugador es mental, no física. A menor lucidez, más intensas las alucinaciones visuales y auditivas. Se recupera con hierbas consumibles distribuidas en el mapa.
- **Inventario en dos fases** — 2 slots en el Acto 1 (manos), 5 slots a partir del Acto 2 (manos + cinturón desbloqueado narrativamente).
- **Sistema de llaves híbrido** — llaves específicas para la mayoría de puertas, llaves genéricas como excepción. Emergen aleatoriamente en nodos ya visitados, identificadas por una chispa luminosa tenue.
- **IA con Dijkstra + NavMesh** — patrullaje calculado con algoritmo Dijkstra propio en C# (requisito académico), movimiento físico ejecutado por NavMesh. State machine: Idle/Patrullaje → Pursuit.
- **Guardado dual** — autoguardado en cinemáticas de refracción y desbloqueo de puentes; guardado manual en zonas seguras (pintura amarilla) con llave de guardado consumible.
- **Presencia de la entidad** — distorsión progresiva de audio y visión según zona explorada. Solo se manifiesta visualmente en la carrera final del Acto 5.

### Estructura de actos
| Acto | Locación | Refracción |
|---|---|---|
| 1 | Zona Antigua — Edificio Histórico | Refracción 1 |
| 2 | Zona Moderna — Hospital | Refracción 2 |
| 3 | Zona Moderna — Mansión | Refracción 3 |
| 4 | Zona Moderna — Carnaval | Refracción 4 |
| 5 | Laboratorio (epicentro del colapso) | Refracción 5 → Escape final |

### Estructura del proyecto
```
Assets/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Enemy/
│   ├── Systems/
│   │   ├── Detection/
│   │   ├── Inventory/
│   │   ├── SaveLoad/
│   │   └── NodeMap/
│   ├── Narrative/
│   └── Core/
├── Prefabs/
├── Audio/
├── ExternalSources/
└── Settings/
```

### Tecnología
| Elemento | Detalle |
|---|---|
| Motor | Unity 2021.3.45f2 |
| Pipeline | Universal Render Pipeline (URP) |
| Lenguaje | C# |
| Input | New Input System |
| Guardado | JsonUtility (nativo Unity) |
| Pathfinding | Dijkstra — implementación propia en C# |
| Animaciones UI | DoTween Pro (Demigiant) |
| Audio | Sistema nativo de Unity |
| Assets visuales | Synty POLYGON Series |
| Control de versiones | Git + GitHub |

### Referencias
- **Silent Hill** (Konami, 1999) — atmósfera y horror psicológico
- **Resident Evil 2 Remake** (Capcom, 2019) — cámara en tercera persona, sigilo e inventario gestionable
- **Styx: Shards of Darkness** (Cyanide, 2017) — gramática de movimiento y mecánicas de ocultamiento

---

## 🇺🇸 English

### Description
**Dawn Fall Rain** is a third-person psychological horror, stealth, and survival game developed in Unity as a graduation project for a Video Game Programming and Design Engineering degree.

The player controls **Rom Origo**, an unemployed man trapped in **Orevono** — an abandoned industrial city encapsulated by a permanent dome of darkness. No combat, no weapons: only wit, stealth, and the ability to read the environment to survive.

### Objective
Recover the **5 refractions** (motorcycle parts) distributed across five acts and escape Orevono before being consumed by the entity that inhabits the city.

### Core Mechanics
- **Pure stealth** — no combat system exists. Detection activates a three-sphere proximity system: the first alerts the reanimated, the second damages the player's sanity, the third is immediate game over.
- **Two enemy types** — Auditory (detects sound, distractable with thrown objects) and Visual (flashlight cone of vision, deaf to sound). They never coexist in the same node.
- **Sanity system** — player health is mental, not physical. Lower lucidity triggers visual and auditory hallucinations. Restored with consumable herbs distributed across the map.
- **Two-phase inventory** — 2 slots in Act 1 (hands), 5 slots from Act 2 onwards (hands + belt unlocked through narrative event).
- **Hybrid key system** — specific keys for most doors, generic keys as exception. Keys spawn randomly in previously visited nodes, identified by a faint luminous spark.
- **AI with Dijkstra + NavMesh** — patrol order calculated with a custom Dijkstra algorithm in C# (academic requirement), physical movement handled by NavMesh. State machine: Idle/Patrol → Pursuit.
- **Dual save system** — autosave on refraction cinematics and bridge unlocks; manual save at safe zones (yellow paint) using a consumable save key.
- **Entity presence** — progressive audio and visual distortion based on explored zone. Only manifests visually during the final chase sequence in Act 5.

### Act Structure
| Act | Location | Refraction |
|---|---|---|
| 1 | Old Zone — Historic Building | Refraction 1 |
| 2 | Modern Zone — Hospital | Refraction 2 |
| 3 | Modern Zone — Mansion | Refraction 3 |
| 4 | Modern Zone — Carnival | Refraction 4 |
| 5 | Laboratory (collapse epicenter) | Refraction 5 → Final escape |

### Project Structure
```
Assets/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Enemy/
│   ├── Systems/
│   │   ├── Detection/
│   │   ├── Inventory/
│   │   ├── SaveLoad/
│   │   └── NodeMap/
│   ├── Narrative/
│   └── Core/
├── Prefabs/
├── Audio/
├── ExternalSources/
└── Settings/
```

### Tech Stack
| Element | Detail |
|---|---|
| Engine | Unity 2021.3.45f2 |
| Pipeline | Universal Render Pipeline (URP) |
| Language | C# |
| Input | New Input System |
| Save System | JsonUtility (Unity native) |
| Pathfinding | Dijkstra — custom C# implementation |
| UI Animation | DoTween Pro (Demigiant) |
| Audio | Unity native audio system |
| Visual Assets | Synty POLYGON Series |
| Version Control | Git + GitHub |

### References
- **Silent Hill** (Konami, 1999) — atmosphere and psychological horror
- **Resident Evil 2 Remake** (Capcom, 2019) — third-person camera, stealth and inventory system
- **Styx: Shards of Darkness** (Cyanide, 2017) — movement grammar and concealment mechanics

---

*Dawn Fall Rain © 2025 Zyumane Zye. Graduation project — Video Game Programming and Design Engineering.*
