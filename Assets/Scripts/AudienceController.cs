using UnityEngine;

public class AudienceController : MonoBehaviour
{
    private Vector3 defaultPosition;
    private float ran;
    
    void Start() 
    {
        defaultPosition = transform.position;
        ran = Random.Range(1.5f, 3.0f);
    }
    void Update()
    {
        transform.position = defaultPosition + new Vector3(0f, Mathf.Sin(Time.time*ran)/5f, 0f);
    }
}
