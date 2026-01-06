using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
// VRM1.0を使用
using UniVRM10;

public class AutoBlink : MonoBehaviour
{
    // 制御パラメータ
    // 固有角周波数ω
    public float omega_n = 30.0f;
    // 減衰係数
    public float zeta = 1.0f;

    // 状態変数
    // まぶたの位置
    private float x = 0.0f;
    // まぶたの速度
    private float v = 0.0f;
    // 目標位置r(t)
    private float target = 0.0f;


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
            // StartCoroutine(BlinkAction());
            StartCoroutine(BlinkInpulse());
            ResetTimer();
        }

        // 制御工学計算
        // 運動方程式: x'' + 2*zeta*omega*x' + omega^2*x = omega^2*r
        // 加速度 a = x'' を求める
            float a = (omega_n * omega_n * target) 
                      - (2 * zeta * omega_n * v) 
                      - (omega_n * omega_n * x);

        // オイラー法で積分
        float dt = Time.deltaTime;
        v += a * dt;
        x += v * dt;

        // 値を0~1にclamp
        float appliedWeight = Mathf.Clamp01(x);

        //vrmに適用
        vrmInstance.Runtime.Expression.SetWeight(ExpressionKey.Blink, appliedWeight);
    }

    // 指数分布に従う乱数生成（平均間隔 meanTime 秒）
    float GenerateExponentialRandom(float meanTime)
    {
        return -meanTime * Mathf.Log(UnityEngine.Random.value);
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
    IEnumerator BlinkInpulse()
    {
        // ステップ入力(閉じる)
        target = 1.0f;
        // 0.1秒待つ(閉じてる時間)
        yield return new WaitForSeconds(0.1f);
        // ステップ入力(開く)
        target = 0.0f;
    }



    void ResetTimer()
    {
        timer = 0f;

        // 平均3.5秒間のランダムに次の瞬きを決める
        nextBlinkTime = GenerateExponentialRandom(3.5f);
    }
}
