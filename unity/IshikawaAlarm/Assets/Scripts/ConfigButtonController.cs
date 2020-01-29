using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 設定ボタンコントローラ
/// </summary>
public class ConfigButtonController : MonoBehaviour {
    [SerializeField, Tooltip("変更時間")]
    private float changeTime_ = 0.3f;
    [SerializeField, Tooltip("設定オブジェクトコントローラ")]
    private ConfigObjectController configObjectController_ = null;
    /// <summary>設定オブジェクトトランスフォーム</summary>
    private Transform configObjectTransform_ = null;
    [SerializeField, Tooltip("設定オブジェクトの非表示位置")]
    private Vector3 unviewPositionOfConfigObject_ = new Vector3(-720.0f, 0.0f, 0.0f);
    [SerializeField, Tooltip("設定オブジェクトの表示位置")]
    private Vector3 viewPositionOfConfigObject_ = Vector3.zero;


    // Start is called before the first frame update
    public void Start() {
        if(this.configObjectController_ == null) {
            Debug.LogError("Config object controller is null.");
        }
        this.configObjectTransform_ = this.configObjectController_.gameObject.transform;

        // 設定状態で戻るボタンが押されたらキャンセル
        MainSceneManager.instance.SetBackAction(MainSceneManager.ViewState.Config, this.OnCancel);
    }

    /// <summary>設定オブジェクトを表示する</summary>
    public void OnViewConfigObject() {
        // 設定を読み込む
        this.configObjectController_.setting = ConfigDataManager.instance.saveData;

        StartCoroutine(this.ViewConfigObject());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Select).Play();
    }
    /// <summary>設定オブジェクトを表示する</summary>
    private IEnumerator ViewConfigObject() {
        // 設定状態にする
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.Config;
        // オブジェクトをアクティブにする
        this.configObjectTransform_.gameObject.SetActive(true);

        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.configObjectTransform_.localPosition = Vector3.Lerp(
                this.unviewPositionOfConfigObject_,
                this.viewPositionOfConfigObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.configObjectTransform_.localPosition = this.viewPositionOfConfigObject_;
    }
    
    /// <summary>設定セーブ</summary>
    public void OnSave() {
        // データ設定
        ConfigDataManager.instance.saveData = this.configObjectController_.setting;

        StartCoroutine(this.UnviewConfigObject());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Select).Play();
    }
    /// <summary>設定キャンセル</summary>
    public void OnCancel() {
        // 元データ設定
        ConfigDataManager.instance.saveData = ConfigDataManager.instance.saveData;

        StartCoroutine(this.UnviewConfigObject());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Cancel).Play();
    }
    /// <summary>設定オブジェクトを非表示にする</summary>
    private IEnumerator UnviewConfigObject() {
        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.configObjectTransform_.localPosition = Vector3.Lerp(
                this.viewPositionOfConfigObject_,
                this.unviewPositionOfConfigObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.configObjectTransform_.localPosition = this.unviewPositionOfConfigObject_;

        // オブジェクトを非アクティブにする
        this.configObjectTransform_.gameObject.SetActive(false);
        // 設定メニューの状態に戻す
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.OptionMenu;
    }
}
