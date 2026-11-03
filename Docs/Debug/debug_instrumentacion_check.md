# Auditoría e Instrumentación de Depuración de Eventos – Unidad 4 Clase 1

## Contexto y Objetivo
El objetivo de este documento es registrar la auditoría de instrumentación técnica del canal de eventos de gameplay `PopupEventSO` (`System.Action<PopupData>`) y validar el entorno de desarrollo para el análisis con el depurador de IDE (Rider / Visual Studio) acoplado a Unity (*Attach to Unity Process*).

Se busca garantizar la trazabilidad del recorrido de la señal desde su emisión en el dominio de gameplay hasta su recepción en el dominio de UI, previniendo comportamientos anómalos o saltos sintéticos en el depurador.

---

## Arquitectura del Evento Auditado

- **Evento Auditado:** `PopupEventSO.OnPopupRequested`
- **Tipo de Delegado:** `System.Action<PopupData>` (DTO inmutable de transporte de datos)
- **Componente Emisor:** `PlayerHealth.cs` mediante la invocación `popupChannel.Raise(deathPopupData)`
- **Componente Receptor:** `PopupManager.cs` en la escena `03_Gameplay`

---

## Fundamentación Técnica de Instrumentación

### 1. Ausencia de Expresiones Anónimas y Lambdas
Para realizar una depuración efectiva con la técnica **Step Into (F11)**, es requisito arquitectónico que las suscripciones a delegados no utilicen lambdas (`() => { ... }`) ni delegados anónimos. 

* **Fundamento:** Las expresiones anónimas generan en tiempo de compilación clases y métodos sintéticos gestionados por el compilador de C# (`<DisplayClass>`). Al presionar *Step Into* sobre una llamada a un delegado basado en lambdas, el depurador de Unity suele saltar el bloque, entrar en código CIL/generado no legible o perder la referencia del puntero de ejecución.
* **Estado en la Base de Código:** Se verificó que `PopupManager` efectúa la suscripción en el ciclo de vida `OnEnable` utilizando un método con nombre de forma explícita:
  ```csharp
  private void OnEnable()
  {
      if (popupChannel != null)
      {
          popupChannel.OnPopupRequested += ShowPopup; // Método nombrado explícito
      }
  }