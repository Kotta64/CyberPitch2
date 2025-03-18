using UnityEngine;

public class BallAnimation : MonoBehaviour
{
    private Vector3 before_position;
    private Vector3 delta;
    private GameObject parent;

    void Start()
    {
        parent = transform.parent.gameObject;
    }

    void Update()
    {
        delta = parent.transform.position - before_position;

        transform.Rotate(delta.z/Mathf.PI * 180, 0, delta.x/Mathf.PI * -180, Space.World);

        before_position = parent.transform.position;
    }
}
