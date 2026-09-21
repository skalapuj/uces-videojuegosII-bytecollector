using UnityEngine;

/// <summary>
/// Contrato para entidades que se mueven por la grilla (Player, Leak).
/// Permite que GridManager sepa qué casillas están ocupadas SIN depender de una clase concreta.
/// </summary>
public interface IGridOccupant
{
    bool Occupies(Vector2Int cell);
}
