using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ひよりちゃんの目のコントローラ
/// </summary>
public class HiyoriEyeController : MonoBehaviour {
    /// <summary>
    /// 設定値
    /// </summary>
    public struct SettingValues{
        /// <summary>左開き [0.0, 1.0, 1.2]</summary>
        public float openLeft_;
        /// <summary>左笑み [0.0, 0.0, 1.0]</summary>
        public float smileLeft_;
        /// <summary>右開き [0.0, 1.0, 1.2]</summary>
        public float openRight_;
        /// <summary>右笑み [0.0, 0.0, 1.0]</summary>
        public float smileRight_;
        /// <summary>視線向き(横) [-1.0, 0.0, 1.0]</summary>
        public float ballX_;
        /// <summary>視線向き(縦) [-1.0, 0.0, 1.0]</summary>
        public float ballY_;
    }


    [SerializeField, Tooltip("ひよりちゃんパラメータコントローラ")]
    private HiyoriParameterController hiyoriParameterController_ = null;
    [SerializeField, Tooltip("幸福度での変化値(開き)")]
    private float happinessOpenSlide_ = 0.1f;
    [SerializeField, Tooltip("困惑度での変化値(開き)")]
    private float worryOpenSlide_ = -0.1f;
    [SerializeField, Tooltip("ひよりちゃんまばたきコントローラ")]
    private HiyoriBlinkController hiyoriBlinkController_ = null;
    [SerializeField, Tooltip("視線更新比率")]
    private float lookUpdateRatio_ = 0.3f;
    /// <summary>視線更新比率</summary>
    private float lookUpdateRatio {
        get { return this.lookUpdateRatio_; }
    }
    [SerializeField, Tooltip("視線(横)目標値"), Range(-1.0f, 1.0f)]
    private float toLookX_ = 0.0f;
    /// <summary>視線(横)目標値 [-1.0, 0.0, 1.0]</summary>
    public float toLookX {
        private get { return this.toLookX_; }
        set {
            const float min = -1.0f;
            const float max = 1.0f;
            this.toLookX_ = Mathf.Clamp(value, min, max);
        }
    }
    [SerializeField, Tooltip("視線(縦)目標値"), Range(-1.0f, 1.0f)]
    private float toLookY_ = 0.0f;
    /// <summary>視線(縦)目標値 [-1.0, 0.0, 1.0]</summary>
    public float toLookY {
        private get { return this.toLookY_; }
        set {
            const float min = -1.0f;
            const float max = 1.0f;
            this.toLookY_ = Mathf.Clamp(value, min, max);
        }
    }
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
        if(this.hiyoriBlinkController_ == null) {
            Debug.LogError("Hiyori blink controller is null.");
        }
    }
    // Update is called once per frame
    public void Update() {
        // 左目
        this.settingValues_.openLeft_ =
            (
                1.0f +
                this.happinessOpenSlide_*this.hiyoriParameterController_.happiness +
                this.worryOpenSlide_*this.hiyoriParameterController_.worry
            ) *
            (
                this.hiyoriBlinkController_.value *
                (1.0f - this.hiyoriParameterController_.sleepiness*this.hiyoriParameterController_.sleepiness)
            );
        this.settingValues_.smileLeft_ = Mathf.Max(this.hiyoriParameterController_.happiness, 0.0f);
        // 右目
        this.settingValues_.openRight_ =
            (
                1.0f +
                this.happinessOpenSlide_*this.hiyoriParameterController_.happiness +
                this.worryOpenSlide_*this.hiyoriParameterController_.worry
            ) *
            (
                this.hiyoriBlinkController_.value *
                (1.0f - this.hiyoriParameterController_.sleepiness*this.hiyoriParameterController_.sleepiness)
            );
        this.settingValues_.smileRight_ = Mathf.Max(this.hiyoriParameterController_.happiness, 0.0f);
        // 視線
        this.settingValues_.ballX_ = this.lookUpdateRatio*this.toLookX + (1.0f-this.lookUpdateRatio)*this.settingValues_.ballX_;
        this.settingValues_.ballY_ = this.lookUpdateRatio*this.toLookY + (1.0f-this.lookUpdateRatio)*this.settingValues_.ballY_;
    }
    /// <summary>
    /// Live2Dパラメータ更新
    /// </summary>
    private void LateUpdate() {
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeLOpen, this.settingValues.openLeft_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeLSmile, this.settingValues.smileLeft_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeROpen, this.settingValues.openRight_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeRSmile, this.settingValues.smileRight_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeBallX, this.settingValues.ballX_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.EyeBallY, this.settingValues.ballY_);
    }
}
