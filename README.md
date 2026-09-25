# 👾 Byte Collector

**Universidad de Ciencias Empresariales y Sociales (UCES)**

**Carrera:** Tecnicatura en Programación

**Asignatura:** Diseño y Desarrollo de Videojuegos II

---

## 📖 Descripción General
**Byte Collector** es un videojuego arcade 2D minimalista. El jugador controla un "byte" en una cuadrícula con el objetivo de absorber paquetes de datos válidos mientras sobrevive y esquiva glitches del sistema que lo persiguen. 

Este repositorio contiene el código fuente, los assets y la documentación del proyecto, desarrollado bajo un enfoque de trabajo colaborativo y metodologías ágiles.

---

## 🗺️ Flujo de Pantallas y Navegación

![Flujo de Pantallas y Navegación](docs/flujo_pantallas.png)

El flujo de navegación entre escenas y paneles modales está modelado mediante **PlantUML** en el archivo [`docs/flujo_pantallas.puml`](docs/flujo_pantallas.puml).


### Diagrama de Estados

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

---

## 🛠️ Arquitectura Técnica de UI

* **Canvas Scaler:** Configurado en `Scale With Screen Size`, resolución base de **1920x1080** y factor de escala balanceado `Match Width Or Height = 0.5` para garantizar coherencia en resoluciones 16:9, 4:3 y 21:9.
* **Sistema de Prefabs UI (`Assets/Prefabs/UI/`):**
  * `btn_Base`: Prefab base con anclajes centrados, dimensiones estandarizadas y componente TextMeshPro responsivo.
  * `btn_Primary` (Variante): Estilo cian neón para acciones primarias (`Jugar`).
  * `btn_Secondary` (Variante): Estilo magenta neón para submenús (`Opciones`, `Créditos`).
  * `btn_Back` (Variante): Botón compacto de retorno.
  * `panel_Container_Modal`: Contenedor semitransparente con borde neón para desplegar vistas modales sin recargar escenas.
* **Controladores de Navegación:**
  * `UIManager.cs`: Centraliza la apertura/cierre de modales (`Panel_Options`) y transiciones de escena desde el menú principal.
  * `GameplayNavigation.cs`: Gestiona el retorno seguro desde el HUD del juego hacia `01_MainMenu`.
  * `SplashScreenLoader.cs`: Corrutina temporizada para transicionar desde el arranque.

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

## 👥 Equipo de Desarrollo y Roles (Scrum/Kanban)
Somos un equipo de 3 integrantes, trabajando bajo un modelo de responsabilidad compartida con roles rotativos:

* **Fiorella Mosca** - *Product Owner / Developer*
  * Encargada de priorizar el Backlog, definir las mecánicas principales y programar los scripts de movimiento del jugador.
* **Sol Ailen Kalapuj** - *Scrum Master / Developer*
  * Facilita el flujo de trabajo en el tablero Kanban, destraba conflictos de integración en Unity y programa la lógica de la UI.
* **Armando Pasilis** - *Game Designer / Developer*
  * Responsable de la creación de prefabs, diseño de niveles, balanceo de dificultad y recolección de assets (sonido y visuales).
