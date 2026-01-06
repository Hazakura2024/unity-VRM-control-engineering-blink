using UnityEngine;
using Cinemachine; // "Unity." を取って、単に Cinemachine にする

public class CameraSwitcher : MonoBehaviour
{

    // カメラの配列
    public CinemachineVirtualCamera[] cameras;
    private int currentCameraIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //最初のカメラ(indexが初期値の0)以外は優先度を下げる
        UpdateCameraPriority();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // 次のカメラへ
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            UpdateCameraPriority();
        }
    }

    void UpdateCameraPriority()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            // 現在のカメラの優先度を高く
            cameras[i].Priority = (i == currentCameraIndex) ? 10 : 0;
        }
    }
}
