using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ひよりちゃんの呼吸コントローラ
/// </summary>
public class HiyoriBreathController : MonoBehaviour {
    [SerializeField, Tooltip("ひよりちゃんパラメータコントローラ")]
    private HiyoriParameterController hiyoriParameterController_ = null;
    [SerializeField, Tooltip("呼吸間隔")]
    private float interval_ = 2.5f;
    /// <summary>呼吸間隔</summary>
    private float interval {
        get {
            const float min = 0.1f;
            return Mathf.Max(this.interval_, min);
        }
    }
    /// <summary>時間</summary>
    private float time_ = 0.0f;


    // Start is called before the first frame update
    public void Start() {
        if(this.hiyoriParameterController_ == null) {
            Debug.LogError("Hiyori parameter controller is null.");
        }
    }
    // Update is called once per frame
    public void Update() {
        this.time_ += Time.deltaTime;
        while(this.time_ > this.interval) {
            this.time_ -= this.interval;
        }
    }
    /// <summary>
    /// Live2Dパラメータ更新
    /// </summary>
    private void LateUpdate() {
        float rate = this.time_ / this.interval;
        float sinValue = Mathf.Sin(2.0f*Mathf.PI*rate);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.Breath, 0.5f*(sinValue+1.0f));
    }
}
