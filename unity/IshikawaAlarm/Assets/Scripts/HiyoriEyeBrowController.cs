using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ひよりちゃんの眉のコントローラ
/// </summary>
public class HiyoriEyeBrowController : MonoBehaviour {
    /// <summary>
    /// 設定値
    /// </summary>
    public struct SettingValues{
        /// <summary>左形 [-1.0, 0.0, 1.0]</summary>
        public float formLeft_;
        /// <summary>右形 [-1.0, 0.0, 1.0]</summary>
        public float formRight_;
    }


    [SerializeField, Tooltip("ひよりちゃんパラメータコントローラ")]
    private HiyoriParameterController hiyoriParameterController_ = null;
    [SerializeField, Tooltip("ひよりちゃんの目のコントローラ")]
    private HiyoriEyeController hiyoriEyeController_ = null;
    [SerializeField, Tooltip("幸福度での変化値")]
    private float happinessSlide_ = 0.5f;
    [SerializeField, Tooltip("目の開きでの変化値")]
    private float eyeOpenSlide_ = 0.3f;
    [SerializeField, Tooltip("困惑最大時の最大値"), Range(-0.999f, 1.0f)]
    private float worryMax_ = -0.5f;
    /// <summary>設定値</summary>
    private SettingValues settingValues_ = new SettingValues();
    /// <summary>設定値</summary>
    public SettingValues settingValues {
        get { return this.settingValues_; }
    }


    // Start is called before the first frame update
    public void Start() {
        if(this.hiyoriParameterController_ == null) {
            Debug.LogError("Hiyori parameter controller is null.");
        }
        if(this.hiyoriEyeController_ == null) {
            Debug.LogError("Hiyori eye controller is null.");
        }
    }
    // Update is called once per frame
    public void Update() {
        float range =
            (1.0f-this.hiyoriParameterController_.worry)*2.0f +
            this.hiyoriParameterController_.worry*(1.0f+this.worryMax_);
        float rate = 0.5f * range;
        // 左
        this.settingValues_.formLeft_ =
            (
                this.happinessSlide_ * this.hiyoriParameterController_.happiness +
                this.eyeOpenSlide_ * (this.hiyoriEyeController_.settingValues.openLeft_ - 1.0f)
            )*rate + (rate - 1.0f);
        // 右
        this.settingValues_.formRight_ =
            (
                this.happinessSlide_ * this.hiyoriParameterController_.happiness +
                this.eyeOpenSlide_ * (this.hiyoriEyeController_.settingValues.openRight_ - 1.0f)
            )*rate + (rate - 1.0f);
    }
    /// <summary>
    /// Live2Dパラメータ更新
    /// </summary>
    private void LateUpdate() {
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeBrowLForm, this.settingValues.formLeft_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeBrowRForm, this.settingValues.formRight_);
    }
}
