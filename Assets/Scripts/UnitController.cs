using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Unit selectedUnit;
    private Dictionary<KeyCode, Action> keyCommands = new Dictionary<KeyCode, Action>();
    private float currentKeyHold = 0f;
    private float keyHoldDelay = .4f;
    private bool unitInMotion = false;

    private void Start()
    {
        keyCommands.Add(KeyCode.W, () => MoveTo(Direction.North));
        keyCommands.Add(KeyCode.A, () => MoveTo(Direction.West));
        keyCommands.Add(KeyCode.S, () => MoveTo(Direction.South));
        keyCommands.Add(KeyCode.D, () => MoveTo(Direction.East));
        keyCommands.Add(KeyCode.Space, () => selectedUnit.currentSpace.ChangeType());
    }

    void Update()
    {
        HandleLeftClick();
        StartCoroutine(HandleRightClick());

        foreach(KeyCode key in keyCommands.Keys)
        {
            if (Input.GetKeyDown(key)) keyCommands[key].Invoke();
            if (Input.GetKey(key))
            {
                if(currentKeyHold > keyHoldDelay)
                {
                    keyCommands[key].Invoke();
                }
                currentKeyHold += Time.deltaTime;
            }
            else if(!Input.anyKey)
            {
                currentKeyHold = 0f;
            }
        }
    }

    void HandleLeftClick()
    {
        if (Input.GetMouseButtonDown(0) && !unitInMotion)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Unit")
                {
                    selectedUnit?.OnDeselect();
                    selectedUnit = hit.collider.GetComponent<Unit>();
                }
                if (hit.collider.tag == "Space")
                {
                    selectedUnit?.OnDeselect();
                    selectedUnit = hit.collider.GetComponent<Space>().SelectContainedUnit();
                }
            }
            selectedUnit?.OnSelect();
        }
    }

    IEnumerator HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(!selectedUnit) { yield return null; }
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Space")
                {
                    Queue<Space> path;
                    if(!MapHolder.Instance.TryGetRoute(selectedUnit.currentSpace, hit.collider.GetComponent<Space>(), out path))
                    {
                        yield return null;
                    }
                    Space next;
                    while (path.Count != 0)
                    {
                        unitInMotion = true;
                        next = path.Dequeue();
                        selectedUnit.TeleportTo(next);
                        yield return WaitForNFrames(10);
                    }
                    unitInMotion = false;
                }
            }
        }
    }

    void MoveTo(Direction direction)
    {
        var neighboringSpace = selectedUnit?.currentSpace?.GetNeighbor(direction);
        if (neighboringSpace == null)
        {
            return;
        }
        selectedUnit?.TeleportTo(neighboringSpace);
    }

    IEnumerator WaitForNFrames(int n)
    {
        if(n<0 || n > 60)
        {
            yield return null;
        }
        while (n > 0)
        {
            yield return new WaitForEndOfFrame();
            n--;
        }
    }
}
