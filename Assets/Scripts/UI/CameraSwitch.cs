using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] BallMove bm; // ボールの制御スクリプト
    private CinemachineCamera[] Cameras; // VirtualCameraのリスト
    private int index;

    private void Start() 
    {
        index = 0;
        Cameras = bm.virtualCamera;
        resetCamera();
    }

    // カメラのリセット
    private void resetCamera()
    {
        for(int i=0; i < Cameras.Length; i++) {
            Cameras[i].enabled = i==index;
        }
    }

    // カメラの切り替え
    public void OnClicked()
    {
        index++;
        if(index >= Cameras.Length) index = 1;

        resetCamera();
    }
}
