# Dawn Fall Rain
### *A game by Zyumane Zye — Norungo Project*

---

## 🇲🇽 Español

### Descripción
**Dawn Fall Rain** es un juego de horror psicológico, sigilo y supervivencia en tercera persona desarrollado en Unity como proyecto de graduación de la carrera de Ingeniería en Programación y Diseño de Videojuegos.

El jugador controla a **Rom Origo**, un hombre desempleado que queda atrapado en **Orevono**, una ciudad industrial abandonada rodeada por un domo de oscuridad permanente. Sin combate, sin armas — solo ingenio, sigilo y la capacidad de leer el entorno para sobrevivir.

### Objetivo
Recuperar las **5 refracciones** de la motocicleta distribuidas en cinco actos y escapar de Orevono antes de ser consumido por la entidad que habita la ciudad.

### Mecánicas principales
- **Sigilo puro** — ser detectado activa un QTE; fallar significa game over inmediato.
- **Dos tipos de enemigos** — Auditivo (detecta sonido) y Visual (detecta movimiento en su campo de visión).
- **Inventario limitado** — 2 slots para llaves y objetos de distracción.
- **Pathfinding propio** — sistema de navegación de enemigos implementado con el algoritmo de Dijkstra en C# sin NavMesh.
- **Guardado con JsonUtility** — guardado manual en refugios marcados y autoguardado en puntos de progresión.
- **Presencia de la entidad** — distorsión progresiva de audio y visión según la zona explorada.

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
| Guardado | JsonUtility (nativo Unity) |
| Pathfinding | Dijkstra — implementación propia |
| Audio | Sistema nativo de Unity |
| Assets visuales | Synty Polygon Series |
| Control de versiones | Git + GitHub |

### Referencias
- **Silent Hill** (Konami, 1999) — atmósfera y horror psicológico
- **Resident Evil 2 Remake** (Capcom, 2019) — sigilo e inventario
- **Styx: Shards of Darkness** (Cyanide, 2017) — gramática de movimiento y ocultamiento

---

## 🇺🇸 English

### Description
**Dawn Fall Rain** is a third-person psychological horror, stealth and survival game developed in Unity as a graduation project for a Video Game Programming and Design Engineering degree.

The player controls **Rom Origo**, an unemployed man trapped in **Orevono**, an abandoned industrial city surrounded by a permanent dome of darkness. No combat, no weapons — only wit, stealth, and the ability to read the environment to survive.

### Objective
Recover the **5 refractions** of the motorcycle distributed across five acts and escape Orevono before being consumed by the entity inhabiting the city.

### Core Mechanics
- **Pure stealth** — being detected triggers a QTE; failing means immediate game over.
- **Two enemy types** — Auditory (detects sound) and Visual (detects movement within its field of view).
- **Limited inventory** — 2 slots for keys and distraction objects.
- **Custom pathfinding** — enemy navigation system implemented using Dijkstra's algorithm in C# without NavMesh.
- **JsonUtility saving** — manual saving at marked shelters and autosave at progression checkpoints.
- **Entity presence** — progressive audio and visual distortion depending on the explored zone.

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
| Save System | JsonUtility (Unity native) |
| Pathfinding | Dijkstra — custom implementation |
| Audio | Unity native audio system |
| Visual Assets | Synty Polygon Series |
| Version Control | Git + GitHub |

### References
- **Silent Hill** (Konami, 1999) — atmosphere and psychological horror
- **Resident Evil 2 Remake** (Capcom, 2019) — stealth and inventory system
- **Styx: Shards of Darkness** (Cyanide, 2017) — movement grammar and concealment mechanics

---

*Dawn Fall Rain © 2025 Zyumane Zye. Graduation project — Video Game Programming and Design Engineering.*
