using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
// VRM1.0を使用
using UniVRM10;

public class AutoBlink : MonoBehaviour
{
    // VRMを制御するコンポーネント
    private Vrm10Instance vrmInstance;

    // 次の瞬きまでの時間
    private float timer;
    private float nextBlinkTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //自分についているVrm10Instanceを取得
        vrmInstance = GetComponent<Vrm10Instance>();

        // 最初の瞬き時間をランダムに決める
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (vrmInstance == null) return;

        timer += Time.deltaTime;

        // 時間が来たらまばたき実行
        if (timer >= nextBlinkTime)
        {
            StartCoroutine(BlinkAction());
            ResetTimer();
        }
    }

    // 指数分布に従う乱数生成（平均間隔 meanTime 秒）
    float GenerateExponentialRandom(float meanTime)
    {
        return -meanTime * Mathf.Log(Random.value);
    }

    // まばたきの動作
    IEnumerator BlinkAction()
    {
        // 目を閉じる(Blinkの値を1.0に)
        vrmInstance.Runtime.Expression.SetWeight(ExpressionKey.Blink, 1.0f);

        // 0.1秒待つ(閉じてる時間)
        yield return new WaitForSeconds(0.1f);

        // 目を開ける(Blinkの値を0.1f)
        vrmInstance.Runtime.Expression.SetWeight(ExpressionKey.Blink, 0.0f);
    }

    void ResetTimer()
    {
        timer = 0f;

        // 2秒~5秒の間のランダムに次の瞬きを決める
        nextBlinkTime = Random.Range(2.0f, 5.0f);
    }
}
