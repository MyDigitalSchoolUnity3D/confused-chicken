using System.Collections.Generic;
using UnityEngine;

public class ChickenFollower : MonoBehaviour
{
    GameObject lastChicken = ChickenController.chickens[ChickenController.chickens.Count - 1];

    public Transform target;
    private Vector2 lastPosition;
    private Queue<Vector2> positionHistory = new Queue<Vector2>();

    public float followDelay = 0.2f;

    void Start()
    {
        if (target != null)
        {
            lastPosition = target.position;
        }
    }

    void Update()
    {
        if (target != null)
        {
            positionHistory.Enqueue(target.position);

            if (positionHistory.Count > Mathf.RoundToInt(followDelay / Time.deltaTime))
            {
                lastPosition = positionHistory.Dequeue();
            }

            transform.position = lastPosition;
        }
    }
}
