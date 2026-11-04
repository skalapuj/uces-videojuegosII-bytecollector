# Informe de Depuración Avanzada y Análisis de Call Stack

**Evento Analizado:** `PopupEventSO.OnPopupRequested` (`System.Action<PopupData>`) 

![Llamado inicial](../Imagenes/Debug/debugger_callstack_call.png) 

**Componentes Intervinientes:** `PlayerHealth.cs`, `PopupEventSO.cs`, `PopupManager.cs`, `GenericPopupView.cs`

---

### Evidencia Gráfica de la Pila de Llamadas 

![Trazabilidad del Call Stack en Visual Studio](../Imagenes/Debug/debugger_stack_view.png)
*Figura 1: Captura de la ventana Call Stack en el IDE mostrando la traza de ejecución desde la física 2D hasta la activación de la interfaz.*

---

### Análisis Trazable del Flujo Interno (Frame por Frame)

De acuerdo con la inspección realizada mediante el depurador (*Attach to Unity Process*) y la traza de la **Figura 1**, el recorrido secuencial de la señal se desglosa en los siguientes 6 niveles de la pila de llamadas:

1. **Frame 6 — `PlayerHealth:OnTriggerStay2D(Collider2D)` [Línea 151]:**  
   El ciclo de física 2D de Unity detecta que el avatar permanece en contacto con el *collider* de una entidad dañina (`Glitch_Leak` / `CorruptedTile`). Se inicia la cadena de ejecución.
2. **Frame 5 — `PlayerHealth:CheckHazardCollision(GameObject)` [Línea 65]:**  
   Se valida si el objeto colisionado posee la etiqueta (*tag*) de amenaza y si el jugador no se encuentra en estado de invulnerabilidad temporal. Al confirmarse el impacto, se solicita el descuento de vidas.
3. **Frame 4 — `PlayerHealth:TakeDamage(Int32)` [Línea 82]:**  
   Se resta la cantidad de daño a la salud actual y se evalúa la condición de derrota (`currentLives <= 0`). Al agotarse las vidas, se deriva la responsabilidad al método de finalización.
4. **Frame 3 — `PlayerHealth:TriggerGameOver()` [Línea 141]:**  
   Se desactiva el componente `PlayerController` y se detiene el `Rigidbody2D` (`velocity = Vector2.zero`). Se instancia el objeto inmutable `PopupData` inyectando las acciones concretas (`RestartLevelCommand` y `ReturnToMenuCommand`) y se emite la señal invocando `popupChannel.Raise(deathPopupData)`.
5. **Frame 2 — `PopupEventSO:Raise(PopupData)` [Línea 19]:**  
   La ejecución entra al ScriptableObject que actúa como canal de comunicación desacoplado (Observer). El método `Raise` valida que los datos no sean nulos e invoca el delegado `OnPopupRequested?.Invoke(data)`, notificando a todos los escuchadores suscritos sin conocer sus identidades ni sus tipos.
6. **Frame 1 — `PopupManager:ShowPopup(PopupData)` [Línea 41]:**  
   Mediante la instrucción *Step Into (F11)*, el depurador salta determinísticamente a la primera instrucción de `ShowPopup`, receptor suscrito explícitamente en `OnEnable`. `PopupManager` inyecta la configuración en `GenericPopupView`, eleva el panel en el Canvas (`SetAsLastSibling()`) y congela el bucle del juego (`Time.timeScale = 0f`).

---

### 📝 Diagnóstico y Conclusiones Técnicas
- **Evaluación de Ausencia de Marcos Sintéticos:** La pila expone una secuencia limpia de métodos con nombre. No se observaron marcos sintéticos (`<DisplayClass>`), lo que confirma que evitar lambdas anónimas previene saltos a código no legible y garantiza la trazabilidad limpia con *Step Into*.
- **Integridad del Flujo:** Se constató que no existen invocaciones duplicadas del delegado por frame ni referencias nulas en el transporte de `PopupData`.
- **Mapeo de Suscripciones:** La arquitectura actual garantiza que la UI responda de forma autónoma al evento sin acoplamiento directo con `PlayerHealth`, manteniendo la desuscripción explícita en `OnDisable` para evitar referencias colgadas o fugas de memoria (*memory leaks*).

---

## 📋 Resumen Sintético de la Actividad 

1. **Nombre del Evento Analizado:** `PopupEventSO.OnPopupRequested` (`System.Action<PopupData>`).
2. **Clases y Scripts Involucrados:** `PlayerHealth.cs` (emisor), `PopupEventSO.cs` (canal Observer), `PopupManager.cs` (receptor/suscriptor), `GenericPopupView.cs` (vista modal) e `ICommand` / `RestartLevelCommand` / `ReturnToMenuCommand` (comandos inyectados).
3. **Orden de Ejecución Observado:** `OnTriggerStay2D()` ➔ `CheckHazardCollision()` ➔ `TakeDamage()` ➔ `TriggerGameOver()` ➔ `PopupEventSO.Raise()` ➔ `PopupManager.ShowPopup()` ➔ `GenericPopupView.Configure()`.
4. **Problemas y Anomalías Detectadas:** No se detectaron ejecuciones duplicadas de escuchadores, referencias nulas al destruirse objetos ni listeners perdidos. La pila de llamadas se presentó completamente limpia de clases generadas (`<DisplayClass>`), validando que suscribirse con métodos nombrados evita comportamientos raros o código opaco en el depurador.
5. **Posibles Mejoras y Ajustes Propuestos:** Asegurar que se desuscriban siempre (`-=`) los eventos dentro de `OnDisable` en todos los componentes para no dejar referencias en memoria al recargar la escena de gameplay, y evaluar agregar un logger de métricas para registrar cuándo y por qué se interrumpe la partida.