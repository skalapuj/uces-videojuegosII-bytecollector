# Análisis de Patrones de Diseño y Diagnóstico con Debugger — 2do Parcial (`ByteCollector`)

## 1. Patrones aplicados al gameplay

En el desarrollo de *ByteCollector*, se implementaron de manera complementaria los patrones de diseño **Observer** y **Command** para desacoplar la lógica de supervivencia y estado del jugador respecto de la presentación visual y las acciones de navegación de la interfaz de usuario (UI):

### Patrón Observer (`PopupEventSO` e `IntEventSO`)
- **Dónde se aplicó:** En la comunicación entre `PlayerHealth.cs` (emisor) y los controladores de interfaz `PopupManager.cs` y `HUDController.cs` (suscriptores), utilizando canales desacoplados basados en `ScriptableObject` (`PopupEventSO.OnPopupRequested` e `IntEventSO.OnEventRaised`).
- **Qué problema resuelve:** Evita que los scripts de gameplay mantengan referencias directas (`[SerializeField]`) hacia paneles del Canvas o managers de escena. Ante la pérdida de vidas o la derrota del avatar (`TriggerGameOver()`), el emisor únicamente publica un mensaje en el canal (`popupChannel.Raise(deathPopupData)`) sin conocer qué objetos están escuchando ni cómo procesarán la información.
- **Mejora en la estructura del sistema:** Permite añadir, quitar o desactivar oyentes (como audio, partículas o HUD) sin modificar el código fuente del jugador, garantizando una arquitectura modular y libre de referencias colgadas mediante suscripción en `OnEnable()` y desuscripción explícita en `OnDisable()`.

### Patrón Command (`ICommand`, `RestartLevelCommand`, `ReturnToMenuCommand`)
- **Dónde se aplicó:** En la parametrización de las acciones ejecutables desde las ventanas modales reutilizables (`GenericPopupView.cs`), encapsuladas dentro del objeto de transferencia inmutable `PopupData`.
- **Qué problema resuelve:** Impide que una vista genérica de interfaz (`GenericPopupView`) contenga lógica condicional rígida (`if/else` o `switch`) para decidir qué hacer según el contexto en el que fue invocada (por ejemplo, reiniciar el nivel tras morir o volver al menú principal).
- **Mejora en la estructura del sistema:** Cada acción de usuario se encapsula como un objeto independiente que implementa la interfaz `ICommand` (`Execute()`). De este modo, `PlayerHealth` inyecta los comandos concretos al construir `PopupData`, mientras que `GenericPopupView` se limita a vincular los botones de confirmación y cancelación con la ejecución polimórfica de dichos comandos, logrando un componente modal 100% reutilizable.

---

## 2. Diagnóstico con herramientas de depuración

Para validar la integridad del flujo de eventos en tiempo de ejecución y descartar problemas de sincronía, duplicación de llamadas o referencias nulas en la transición hacia el estado de derrota, se realizó una sesión de depuración avanzada conectando el IDE (Visual Studio) al proceso activo de Unity (*Attach to Unity Process*).

### Herramientas aplicadas y evidencia obtenida
1. **Breakpoints y navegación paso a paso (*Step Into* - F11):**
   - **Configuración:** Se establecieron puntos de interrupción en el método disparador `PopupEventSO.Raise(PopupData)` y en el método receptor `PopupManager.ShowPopup(PopupData)`.
   - **Información aportada:** Al provocar una colisión fatal en *Play Mode*, el juego se detuvo exactamente en `Raise()`. Al utilizar *Step Into (F11)*, el puntero de ejecución saltó de forma directa y determinista al cuerpo de `ShowPopup()`. Esto confirmó que suscribirse al delegado mediante métodos nombrados (`popupChannel.OnPopupRequested += ShowPopup`) en lugar de expresiones lambda anónimas permite recorrer el flujo de eventos con total claridad, evitando saltos opacos.
2. **Inspección de variables en tiempo real (*Locals / Watch*):**
   - **Información aportada:** Durante la detención en `PopupManager.ShowPopup()`, la ventana *Automático / Watch* permitió inspeccionar en vivo la instancia `data` (`ByteCollector.Events.PopupData`), corroborando que el título (`"MEMORY OVERFLOW"`), el mensaje descriptivo y las referencias a los comandos `RestartLevelCommand` y `ReturnToMenuCommand` llegaban íntegros y sin valores nulos antes de configurar `GenericPopupView`.
3. **Análisis de la pila de llamadas (*Call Stack*):**
   - **Información aportada:** La ventana *Pila de llamadas* expuso la cadena completa y secuencial de ejecución desde el motor de física 2D hasta la interfaz gráfica:
     `PlayerHealth.OnTriggerStay2D()` ➔ `PlayerHealth.CheckHazardCollision()` ➔ `PlayerHealth.TakeDamage()` ➔ `PlayerHealth.TriggerGameOver()` ➔ `PopupEventSO.Raise()` ➔ `PopupManager.ShowPopup()`.
   - **Diagnóstico resultante:** La traza demostró un orden de ejecución lineal y predecible, sin invocaciones duplicadas por frame ni marcos sintéticos generados por el compilador (`<DisplayClass>`), validando tanto la eficiencia de la suscripción como la correcta congelación del tiempo (`Time.timeScale = 0f`) al desplegar la vista modal.