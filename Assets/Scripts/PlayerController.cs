using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject mainCam;
    private GameObject idObj;

    void Start()
    {
        idObj = transform.Find("Id").gameObject;
        mainCam = GameObject.Find("MainCamera");
    }

    void Update()
    {
        idObj.transform.LookAt(mainCam.transform);
    }
}
