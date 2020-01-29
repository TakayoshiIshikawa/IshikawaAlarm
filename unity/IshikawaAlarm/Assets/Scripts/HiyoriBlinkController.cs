using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ひよりちゃんまばたきコントローラ
/// </summary>
public class HiyoriBlinkController : MonoBehaviour {
    [SerializeField, Tooltip("最小間隔")]
    private float minInterval_ = 1.0f;
    /// <summary>最小間隔</summary>
    private float minInterval {
        get {
            const float min = 0.1f;
            return Mathf.Max(this.minInterval_, min);
        }
    }
    [SerializeField, Tooltip("最大間隔")]
    private float maxInterval_ = 5.0f;
    /// <summary>最大瞬き間隔</summary>
    private float maxInterval {
        get { return Mathf.Max(this.maxInterval_, this.minInterval + 0.1f); }
    }
    [SerializeField, Tooltip("動作時間")]
    private float blinkingTime_ = 0.15f;
    /// <summary>動作時間</summary>
    private float blinkingTime {
        get {
            const float min = 0.1f;
            return Mathf.Max(this.blinkingTime_, min);
        }
    }
    /// <summary>瞬き時間</summary>
    private float time_ = 0.0f;
    /// <summary>瞬き間隔</summary>
    private float interval {
        get {
            float rate = (Random.Range(0.0f,1.0f)+Random.Range(0.0f,1.0f)+Random.Range(0.0f,1.0f)) / 3.0f;
            return ((1.0f-rate)*this.minInterval + rate*this.maxInterval);
        }
    }
    /// <summary>瞬き</summary>
    private IEnumerator blink_ = null;
    /// <summary>設定値</summary>
    private float value_ = 1.0f;
    /// <summary>設定値</summary>
    public float value {
        get { return this.value_; }
    }


    // Start is called before the first frame update
    public void Start() {
        this.time_ = this.interval;
    }
    // Update is called once per frame
    public void Update() {
        if(this.blink_ == null) {
            this.time_ -= Time.deltaTime;
            if(this.time_ < 0.0f) {
                this.blink_ = this.Blink();
            }
        }
        else{
            if(!(this.blink_.MoveNext())) {
                this.time_ = this.interval;
                this.blink_ = null;
            }
        }
    }

    /// <summary>
    /// まばたき
    /// </summary>
    /// <returns>コルーチン用データ</returns>
    private IEnumerator Blink() {
        float time = 0.0f;
        while(time < this.blinkingTime) {
            float x = 2.0f*time/this.blinkingTime - 1.0f;
            this.value_ = 2.0f*x*x - x*x*x*x;
            yield return null;
            time += Time.deltaTime;
        }
        this.value_ = 1.0f;
    }
}
