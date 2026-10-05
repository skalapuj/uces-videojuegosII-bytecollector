# 👾 Byte Collector

**Universidad de Ciencias Empresariales y Sociales (UCES)**

**Carrera:** Tecnicatura en Programación

**Asignatura:** Diseño y Desarrollo de Videojuegos II

---

## 📖 Descripción general
Byte Collector es un videojuego arcade 2D desarrollado en Unity. El jugador controla un avatar en una arena visible y debe recolectar bits dentro del campo de juego mientras evita colisiones con elementos peligrosos identificados con los tags `Hazard` y `Glitch`.

El proyecto incluye la lógica principal de gameplay, el sistema de HUD, la navegación entre escenas y la estructura documental del proyecto.

---

## 🧩 Sistema de gameplay actual
La implementación actual responde a una estructura simple pero funcional:

- El jugador se mueve en 2D con `Rigidbody2D` y se limita dinámicamente según el tamaño de la cámara ortográfica.
- Los bits se generan en posiciones aleatorias dentro del área visible y se destruyen al ser recolectados.
- El gestor de partida lleva el control del progreso por sectores, el puntaje y el aumento de dificultad.
- El HUD refleja vidas, progreso del buffer de bits y puntuación.
- El sistema de daño aplica invulnerabilidad temporal y dispara la pantalla de Game Over cuando el jugador queda sin vidas.

---

## 🗺️ Diagrama de arquitectura
El flujo de navegación entre escenas y paneles modales está modelado mediante **PlantUML** en el archivo [`docs/flujo_pantallas.puml`](docs/flujo_pantallas.puml).
![Flujo de Pantallas y Navegación](docs/flujo_pantallas.png)

El diagrama de clases que detalla la relación entre controladores, gestores de ciclo de vida y entidades de colisión se encuentra modelado en [`docs/architecture.puml`](docs/architecture.puml).

![Arquitectura](docs/architecture.png)
---

## 📐 Diagrama de clases actual
El diagrama de clases del proyecto se centra en este conjunto de entidades:

- `PlayerController`
- `PlayerHealth`
- `DataBit`
- `BitSpawner`
- `GameLoopManager`
- `HUDController`
- `GameOverUI`
- `UIManager`


---

## ⚙️ Arquitectura implementada

### Gameplay

- `PlayerController`: movimiento, entrada, confinamiento y ajuste de velocidad.
- `PlayerHealth`: manejo de vidas, daño, invulnerabilidad y fin de partida.
- `DataBit`: elemento recolectable con detección de trigger por contacto con el jugador.
- `BitSpawner`: creación procedural de bits en posiciones seguras dentro de la cámara.
- `GameLoopManager`: control del sector activo, acumulación de bits, score y transición entre etapas.

### UI

- `HUDController`: actualización de salud, barra de progreso y puntaje.
- `GameOverUI`: reinicio de partida y retorno al menú principal.
- `UIManager`: navegación entre pantallas principales y submenús.
- `GameplayNavigation`: navegación desde la escena de juego.
- `SplashScreenLoader`: carga inicial con transición de pantalla.
- `SettingsManager`: configuración del menú.

---

## 🎮 Mecánicas reales presentes en el código

### 1. Movimiento y confinamiento
`PlayerController` usa entrada por eje horizontal y vertical, aplica velocidad al `Rigidbody2D` y limita la posición según los límites calculados a partir de la cámara principal.

### 2. Recolección de bits
`BitSpawner` genera un bit a la vez y `DataBit` lo destruye al detectar el tag `Player`. Cuando se recolecta, se informa a `GameLoopManager` para actualizar progreso y score.

### 3. HUD y progreso
`HUDController` actualiza:
- vidas como íconos activos/inactivos
- texto `BITS: X/Y`
- barra `Slider`
- puntaje con formato `SCORE: 000000`

### 4. Sistema de daño
`PlayerHealth` detecta colisiones con objetos marcados como `Hazard` o `Glitch`, reduce vidas y activa una corrutina de invulnerabilidad con parpadeo visual.

### 5. Fin de partida
Cuando `currentLives <= 0`, `PlayerHealth` desactiva el movimiento del jugador, frena el `Rigidbody2D`, activa el panel de Game Over y congela el tiempo con `Time.timeScale = 0f`.

### 6. Progresión por sectores
`GameLoopManager` inicia en `Sector 01`, cuenta los bits recolectados y cuando alcanza la meta de ese sector pasa a `Sector 02`, aumenta la velocidad del jugador aproximadamente un 30% y cambia el fondo de la cámara.

---

## 📋 Build Settings y Orden de Escenas

Las escenas deben registrarse en el siguiente orden secuencial en Unity (`File > Build Settings`):

| Index | Escena | Descripción |
| :---: | :--- | :--- |
| **0** | `00_Bootstrap_Splash` | Pantalla de inicio con logo del juego |
| **1** | `01_MainMenu` | Menú principal y modal de configuración |
| **2** | `02_Credits` | Créditos, integrantes y atribuciones |
| **3** | `03_Gameplay` | Escena mínima jugable y HUD de pausa |

---

## 🛠️ Herramientas y Metodología
* **Motor Gráfico:** Unity 2D.
* **Control de Versiones:** Git y GitHub.
* **Gestión de Tareas:** GitHub Projects (Tablero Kanban).
* **Comunicación:** [Discord (Canales temáticos y reuniones de voz)](https://discord.gg/JWbFbYqRR).

---

## 🚀 Instrucciones de Instalación y Uso
Para clonar y probar este proyecto en un entorno local:

1. Clonar el repositorio usando Git:
   ```bash
   git clone https://github.com/skalapuj/uces-videojuegosII-bytecollector.git
   ```
2. Abrir Unity Hub.
3. Hacer clic en Add project from disk (Agregar proyecto desde el disco).
4. Seleccionar la carpeta clonada uces-videojuegosII-bytecollecto.
5. Abrir la escena principal navegando a: Assets/Scenes/00_Bootstrap_Splash.unity.

---

## 📝 Convenciones de Commits 
Para mantener el historial limpio, colaborativo y estructurado, utilizamos el estándar de **Conventional Commits** con mensajes descriptivos en presente. Opcionalmente sumamos el ámbito `(scope)` para identificar el área afectada:

* **feat**: Para nuevas funcionalidades, mecánicas o pantallas (ej. `feat(ui): implementar layout responsivo en menu principal`).
* **fix**: Para corrección de errores o bugs visuales/lógicos (ej. `fix(gameplay): corregir anclaje del boton salir`).
* **style**: Para ajustes visuales, estéticos o de formato que no alteran la lógica de programación (ej. `style(splash): estandarizar paleta de color de fondo`).
* **chore**: Para tareas de mantenimiento, configuración del motor, estructuración de carpetas o dependencias (ej. `chore: actualizar configuracion de gitignore`).
* **docs**: Para creación o actualización de documentación y diagramas (ej. `docs: actualizar diagrama de flujo en README`).


## 👥 Equipo de Desarrollo y Roles (Scrum/Kanban)
Somos un equipo de 3 integrantes, trabajando bajo un modelo de responsabilidad compartida con roles rotativos:

* **Fiorella Mosca** - *Product Owner / Developer*
  * Encargada de priorizar el Backlog, definir las mecánicas principales y programar los scripts de movimiento del jugador.
* **Sol Ailen Kalapuj** - *Scrum Master / Developer*
  * Facilita el flujo de trabajo en el tablero Kanban, destraba conflictos de integración en Unity y programa la lógica de la UI.
* **Armando Pasilis** - *Game Designer / Developer*
  * Responsable de la creación de prefabs, diseño de niveles, balanceo de dificultad y recolección de assets (sonido y visuales).

---

## 📌 Nota importante sobre la documentación
La documentación del proyecto describe correctamente la base conceptual del juego, pero debe leerse como una especificación general del diseño y no como un detalle exacto de cada asset o prefab visual presente en la escena actual.

La implementación real se ajusta a la estructura del código: gameplay, HUD, score, daño, sectorización y navegación principal.