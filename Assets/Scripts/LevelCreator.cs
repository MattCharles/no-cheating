using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public Space spacePrefab;
    public Unit unitPrefab;
    public Dictionary<CoordinatePosition, Space> spaces = new Dictionary<CoordinatePosition, Space>();
    public int distanceModifier = 10;
    public const int max_x = 10;
    public const int min_x = 0;
    public const int max_z = 10;
    public const int min_z = 0;

    void Start()
    {
        Space current = null;
        Space last;
        Queue<Space> previousRow = new Queue<Space>();
        for (int x = min_x; x < max_x; x++)
        {
            for (int z = min_z; z < max_z; z++)
            {
                last = current;
                current = Instantiate(spacePrefab, new Vector3(x, 0, z)*distanceModifier, Quaternion.identity);
                CoordinatePosition position = new CoordinatePosition(x, z);
                current.position = position;
                spaces.Add(position, current);
                if (last != null)
                {
                    last.North = current;
                    current.South = last;
                }
                if (x != max_x - 1)
                {
                    previousRow.Enqueue(current);
                }
                if (x != min_x)
                {
                    current.West = previousRow.Dequeue();
                    current.West.East = current;
                }
            }
            current = null;
        }

        Space originSpace = spaces[CoordinatePosition.Origin];
        new MapHolder(spaces);
        originSpace.containedUnit = Instantiate(unitPrefab, originSpace.transform.position, Quaternion.identity);
        originSpace.containedUnit.currentSpace = originSpace;
    }
}
