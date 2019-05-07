using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool selected = false;
    public Space currentSpace;
    public List<Token> tokens;
    public int MovesLeft { get; set; } = 5;
    public float animationSpeed = 4f;
    public float moveAnimationSpeed = 0.0f;

    private void Awake()
    {
        tag = "Unit";
        foreach(Token token in tokens)
        {
            var tokenInstance = Instantiate(token, GenerateTileSpawnVector(), Quaternion.identity);
            tokenInstance.transform.parent = this.transform;
        }
    }

    public void OnSelect()
    {
        selected = true;
        foreach(Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = Color.red;
        }
    }

    public void OnDeselect()
    {
        selected = false;
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = Color.white;
        }
    }

    public void TeleportTo(Space destination)
    {
        Debug.Log("Currently teleporting to " + destination.position);
        currentSpace.containedUnit = null;
        currentSpace = destination;
        destination.containedUnit = this;
        transform.position = destination.transform.position;
    }

    public bool TryMoveTo(Space destination)
    {
        if(!MapHolder.Instance.TryGetDistance(currentSpace, destination, out int distanceToDestination))
        {
            return false;
        }
        if (distanceToDestination <= MovesLeft)
        {
            MovesLeft -= distanceToDestination;
            TeleportTo(destination);
            return true;
        }
        return false;
    }

    private Vector3 GenerateTileSpawnVector()
    {
        Vector2 seed = 5 * Random.insideUnitCircle;
        return new Vector3(seed.x, 0, seed.y);
    }

    public List<Unit> GetNeighbors()
    {
        List<Unit> result = new List<Unit>();
        foreach(Space neighbor in currentSpace.GetNeighbors())
        {
            result.Add(neighbor.containedUnit);
        }
        return result;
    }
}
