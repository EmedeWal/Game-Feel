using UnityEngine;
using System;

public class ColliderEvent : MonoBehaviour
{
    public event Action<Collision2D> CollisionEnter;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter?.Invoke(collision);
    }
}
