using System;
using UnityEngine;

public class Reap : MonoBehaviour
{
    public static Action<int, Vector2Int> destroyed;
    public int id;
    public Vector2Int pos;
    private void OnDestroy()
    {
        destroyed?.Invoke(id, pos);
    }
}
