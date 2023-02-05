using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnLocation {
    public Vector3 Position;
    public Quaternion Direction;
    public SpawnLocation(Vector3 pos, Quaternion rotation) {
        Position = pos;
        Direction = rotation;
    }
    public SpawnLocation(Transform transform) {
        Position = transform.position;
        Direction = transform.rotation;
    }
}

public class ScenarioCard : MonoBehaviour {
    [SerializeField] private int _questId;
}



