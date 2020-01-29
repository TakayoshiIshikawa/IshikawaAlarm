using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ひよりちゃんの口のコントローラ
/// </summary>
public class HiyoriMouthController : MonoBehaviour {
    /// <summary>
    /// 設定値
    /// </summary>
    public struct SettingValues{
        /// <summary>形 [-2.0, 1.0, 1.0]</summary>
        public float form_;
        /// <summary>開き [-1.0, 0.0, 1.0]</summary>
        public float openY_;
    }


    [SerializeField, Tooltip("ひよりちゃんパラメータコントローラ")]
    private HiyoriParameterController hiyoriParameterController_ = null;
    [SerializeField, Tooltip("幸福度での変化値(形)")]
    private float happinessFormSlide_ = 0.3f;
    [SerializeField, Tooltip("困惑度での変化値(形)")]
    private float worryFormSlide_ = -0.1f;
    [SerializeField, Tooltip("眠気での変化率(開き)")]
    private float sleepinessOpenRatio_ = 0.8f;
    [SerializeField, Tooltip("ひよりちゃん会話モーションコントローラ")]
    private HiyoriSpeakMotionController hiyoriSpeakMotionController_ = null;
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
        if(this.hiyoriSpeakMotionController_ == null) {
            Debug.LogError("Hiyori speak motion controller is null.");
        }
    }
    // Update is called once per frame
    public void Update() {
        // 形
        this.settingValues_.form_ =
            this.hiyoriSpeakMotionController_.settingValues.form_ +
            this.happinessFormSlide_*this.hiyoriParameterController_.happiness +
            this.worryFormSlide_*this.hiyoriParameterController_.worry;
        // 開き
        this.settingValues_.openY_ =
            this.hiyoriSpeakMotionController_.settingValues.openY_ *
            (
                (1.0f - this.hiyoriParameterController_.sleepiness) +
                this.hiyoriParameterController_.sleepiness * this.sleepinessOpenRatio_
            );
    }
    /// <summary>
    /// Live2Dパラメータ更新
    /// </summary>
    private void LateUpdate() {
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.MouthForm, this.settingValues.form_);
        this.hiyoriParameterController_.SetParameter(HiyoriParameterController.ParameterName.MouthOpenY, this.settingValues.openY_);
    }
}
