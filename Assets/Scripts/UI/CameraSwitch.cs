using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] BallMove bm; // ボールの制御スクリプト
    [SerializeField] CinemachineBrain camBrain;
    private CinemachineCamera[] Cameras; // VirtualCameraのリスト
    private int index;
    private int defaultIndex;
    private float defaulBlendTime;

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
        if(index >= Cameras.Length-2) return;

        index++;
        if(index >= Cameras.Length - 2) index = 1;

        resetCamera();
    }

    public void change2GoalCamera(int lr)
    {
        defaulBlendTime = camBrain.DefaultBlend.Time;
        camBrain.DefaultBlend.Time = 0.1f;
        defaultIndex = index;
        index = Cameras.Length-lr-1;
        resetCamera();
    }

    public void change2defaultCamera()
    {
        index = defaultIndex;
        resetCamera();
    }

    public void changeBlendTime2default()
    {
        camBrain.DefaultBlend.Time = defaulBlendTime;
    }
}
