using UnityEngine;

public class ChickenFollower : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 3f;

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }
}
