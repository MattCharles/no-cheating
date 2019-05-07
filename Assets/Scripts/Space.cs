using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    public Space North;
    public Space East;
    public Space South;
    public Space West;
    public Unit containedUnit;
    public CoordinatePosition position;
    public bool impassable = false;
    /// <summary>
    /// Not directly settable. Set cost with <see cref="ChangeType(TerrainType)"/>
    /// </summary>
    public int BaseMovementCost = 1;
    public int CostModifier { get; set; } = 0;
    public int MovementCost
    {
        get
        {
            return BaseMovementCost + CostModifier;
        }
    }
    /// <summary>
    /// Not directly settable. Set type with <see cref="ChangeType(TerrainType)"/>
    /// </summary>
    public TerrainType Terrain { get; private set; } = TerrainType.Grassland;

    internal List<Space> GetNeighbors()
    {
        List<Space> result = new List<Space>();
        foreach (Space space in new[] { North, South, East, West })
        {
            if (space != null)
            {
                result.Add(space);
            }
        }
        return result;
    }

    internal List<CoordinatePosition> GetNeighborCoordinates()
    {
        List<CoordinatePosition> result = new List<CoordinatePosition>();
        foreach (Space space in new[] { North, South, East, West })
        {
            if (space != null)
            {
                result.Add(space.position);
            }
        }
        return result;
    }
    private static Dictionary<TerrainType, Color> terrainPainter = new Dictionary<TerrainType, Color>();
    private static Dictionary<TerrainType, int> terrainCosts = new Dictionary<TerrainType, int>();

    public void Awake()
    {
        if (terrainPainter.Count == 0)
        {
            terrainPainter.Add(TerrainType.Grassland, Color.green);
            terrainPainter.Add(TerrainType.Mountain, Color.red);
            terrainCosts.Add(TerrainType.Grassland, 1);
            terrainCosts.Add(TerrainType.Mountain, 99);
        }
    }

    public Unit SelectContainedUnit()
    {
        containedUnit?.OnSelect();
        return containedUnit;
    }

    internal Space GetNeighbor(Direction direction)
    {
        return direction == Direction.North ? North :
            direction == Direction.East ? East :
            direction == Direction.West ? West :
            direction == Direction.South ? South :
            throw new ArgumentException("Direction " + direction + " is invalid.");
    }

    public void ChangeType(TerrainType toType)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = terrainPainter[toType];
        BaseMovementCost = terrainCosts[toType];
        Terrain = toType;
    }

    public void ChangeType()
    {
        Terrain = Terrain.CircularNext();
        gameObject.GetComponent<MeshRenderer>().material.color = terrainPainter[Terrain];
        BaseMovementCost = terrainCosts[Terrain];
    }
}
