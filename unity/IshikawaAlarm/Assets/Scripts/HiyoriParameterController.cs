using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

/// <summary>
/// ひよりちゃんパラメータコントローラ
/// </summary>
public class HiyoriParameterController : MonoBehaviour {
    /// <summary>
    /// パラメータ名
    /// </summary>
    public enum ParameterName {
        /// <summary>顔角度X [-30.0, 0.0, 30.0]</summary>
        AngleX = 0,
        /// <summary>顔角度Y [-30.0, 0.0, 30.0]</summary>
        AngleY,
        /// <summary>顔角度Z [-30.0, 0.0, 30.0]</summary>
        AngleZ,
        /// <summary>頬染め [-1.0, 0.0, 0.0]</summary>
        Cheek,
        /// <summary>左目開き [0.0, 1.0, 1.2]</summary>
        EyeLOpen,
        /// <summary>左目笑み [0.0, 0.0, 1.0]</summary>
        EyeLSmile,
        /// <summary>右目開き [0.0, 1.0, 1.2]</summary>
        EyeROpen,
        /// <summary>右目笑み [0.0, 0.0, 1.0]</summary>
        EyeRSmile,
        /// <summary>視線向き(横) [-1.0, 0.0, 1.0]</summary>
        EyeBallX,
        /// <summary>視線向き(縦) [-1.0, 0.0, 1.0]</summary>
        EyeBallY,
        /// <summary>左眉形 [-1.0, 0.0, 1.0]</summary>
        EyeBrowLForm,
        /// <summary>右眉形 [-1.0, 0.0, 1.0]</summary>
        EyeBrowRForm,
        /// <summary>口形 [-2.0, 1.0, 1.0]</summary>
        MouthForm,
        /// <summary>口開き [-1.0, 0.0, 1.0]</summary>
        MouthOpenY,
        /// <summary>体角度X [-10.0, 0.0, 10.0]</summary>
        BodyAngleX,
        /// <summary>体角度Y [-10.0, 0.0, 10.0]</summary>
        BodyAngleY,
        /// <summary>体角度Z [-10.0, 0.0, 10.0]</summary>
        BodyAngleZ,
        /// <summary>呼吸 [0.0, 0.0, 1.0]</summary>
        Breath,
        /// <summary>左腕角度 [-10.0, 0.0, 10.0]</summary>
        ArmLA,
        /// <summary>右腕角度 [-10.0, 0.0, 10.0]</summary>
        ArmRA,
        /// <summary>胸上下 [-1.0, 0.0, 1.0]</summary>
        BustY,
        /// <summary>アホ毛 [-10.0, 0.0, 10.0]</summary>
        HairAhoge,
        /// <summary>前髪(変更不可,自動演算)</summary>
        HairFront,
        /// <summary>横髪(変更不可,自動演算)</summary>
        HairSide,
        /// <summary>後ろ髪(変更不可,自動演算)</summary>
        HairBack,
        /// <summary>サイドテール(変更不可,自動演算)</summary>
        HairSideUp,
        /// <summary>胸元のリボン(変更不可,自動演算)</summary>
        Ribbon,
        /// <summary>スカート(変更不可,自動演算)</summary>
        Skirt,
        /// <summary>髪にあるリボン(変更不可,自動演算)</summary>
        SideUpRibbon
    }


    [SerializeField, Tooltip("モデル")]
    private CubismModel model_ = null;
    [SerializeField, Tooltip("感情値更新比率"), Range(0.0f, 1.0f)]
    private float emotionalUpdateRatio_ = 0.3f;
    /// <summary>感情値更新比率</summary>
    private float emotionalUpdateRatio {
        get { return this.emotionalUpdateRatio_; }
    }
    /// <summary>幸福度 [-1.0, 0.0, 1.0]</summary>
    private float happiness_ = 0.0f;
    /// <summary>幸福度 [-1.0, 0.0, 1.0]</summary>
    public float happiness {
        get { return this.happiness_; }
    }
    [SerializeField, Tooltip("幸福度目標値"), Range(-1.0f, 1.0f)]
    private float toHappiness_ = 0.0f;
    /// <summary>幸福度目標値 [-1.0, 0.0, 1.0]</summary>
    public float toHappiness {
        private get { return this.toHappiness_; }
        set {
            const float min = -1.0f;
            const float max = 1.0f;
            this.toHappiness_ = Mathf.Clamp(value, min, max);
        }
    }
    /// <summary>困惑度 [0.0, 0.0, 1.0]</summary>
    private float worry_ = 0.0f;
    /// <summary>困惑度 [0.0, 0.0, 1.0]</summary>
    public float worry {
        get { return this.worry_; }
    }
    [SerializeField, Tooltip("困惑度目標値"), Range(0.0f, 1.0f)]
    private float toWorry_ = 0.0f;
    /// <summary>困惑度目標値 [0.0, 0.0, 1.0]</summary>
    public float toWorry {
        private get { return this.toWorry_; }
        set {
            const float min = 0.0f;
            const float max = 1.0f;
            this.toWorry_ = Mathf.Clamp(value, min, max);
        }
    }
    /// <summary>眠気 [0.0, 0.0, 1.0]</summary>
    private float sleepiness_ = 0.0f;
    /// <summary>眠気 [0.0, 0.0, 1.0]</summary>
    public float sleepiness {
        get { return this.sleepiness_; }
    }
    [SerializeField, Tooltip("眠気目標値"), Range(0.0f, 1.0f)]
    private float toSleepiness_ = 0.0f;
    /// <summary>眠気目標値 [0.0, 0.0, 1.0]</summary>
    public float toSleepiness {
        private get { return this.toSleepiness_; }
        set {
            const float min = 0.0f;
            const float max = 1.0f;
            this.toSleepiness_ = Mathf.Clamp(value, min, max);
        }
    }


    // Start is called before the first frame update
    public void Start() {
        if(this.model_ == null) {
            Debug.LogError("Model is null.");
        }

        // 初期化
        this.happiness_ = this.toHappiness_ = 0.0f;
        this.worry_ = this.toWorry_ = 0.0f;
        float time = System.DateTime.Now.Hour + (System.DateTime.Now.Minute / 60.0f);
        float sleep = 0.0f;
        if(time < 1.0f) {
            sleep = (1.0f-time)*0.3f + time*0.5f;
        }
        if(time < 5.0f) {
            sleep = 0.5f;
        }
        else if(time < 7.0f) {
            float t = (time - 5.0f) / 2.0f;
            sleep = (1.0f-t)*0.5f;
        }
        else if(time < 23.0f) {
            sleep = 0.0f;
        }
        else{
            float t = time - 23.0f;
            sleep = t*0.3f;
        }
        this.sleepiness_ = this.toSleepiness_ = sleep;
    }
    // Update is called once per frame
    public void Update() {
        // 感情値更新
        this.happiness_ = this.emotionalUpdateRatio*this.toHappiness + (1.0f-this.emotionalUpdateRatio)*this.happiness;
        this.worry_ = this.emotionalUpdateRatio*this.toWorry + (1.0f-this.emotionalUpdateRatio)*this.worry;
        this.sleepiness_ = this.emotionalUpdateRatio*this.toSleepiness + (1.0f-this.emotionalUpdateRatio)*this.sleepiness;
    }

    /// <summary>
    /// パラメータ情報の取得
    /// </summary>
    /// <param name="_name">パラメータ名</param>
    /// <returns>パラメータ情報</returns>
    private CubismParameter GetParameter(ParameterName _name) {
        return this.model_.Parameters[(int)_name];
    }

    /// <summary>
    /// パラメータの設定
    /// </summary>
    /// <param name="_name">パラメータ名</param>
    /// <param name="_value">値</param>
    public void SetParameter(ParameterName _name, float _value) {
        CubismParameter paramater = this.GetParameter(_name);
        paramater.Value = Mathf.Clamp(_value, paramater.MinimumValue, paramater.MaximumValue);
    }
}
