using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // タイトル画面でマウスカーソルを動かせるように
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("ボタンが押されたよ");
    }
}
