using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
[System.Serializable]
public class DoorData_
{
    public Collider collider;
    public bool occupied = false;
}
public class DoorsController_ : MonoBehaviour
{
    public List<DoorData_> doorsInfo = new();

    void OnDrawGizmos()
    {
        if(doorsInfo == null) return;
        foreach(var door in doorsInfo)
        {
            if(door.collider != null)
            {
                Color color = door.occupied ? Color.rebeccaPurple : Color.mediumSeaGreen;

                Handles.color = color;
                Handles.DrawWireCube(door.collider.bounds.center, door.collider.bounds.size);

                Handles.color = Color.blue;
                Handles.DrawLine(door.collider.transform.position, door.collider.transform.position + door.collider.transform.forward * 1.5f);
            }
        }
    }
}
