using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 設定アラーム表示移動コントローラ
/// </summary>
public class ViewAlarmsObjectMoverController : MonoBehaviour {
    [SerializeField, Tooltip("変更時間")]
    private float changeTime_ = 0.3f;
    [SerializeField, Tooltip("時計アンカーオブジェクトトランスフォーム")]
    private Transform clockAnchorObjectTransform_ = null;
    [SerializeField, Tooltip("設定アラーム表示オブジェクトトランスフォーム")]
    private Transform viewAlarmsObjectTransform_ = null;
    [SerializeField, Tooltip("設定アラーム表示オブジェクトの非表示位置")]
    private Vector3 unviewPositionOfViewAlarmsObject_ = new Vector3(0.0f, -1280.0f, 0.0f);
    [SerializeField, Tooltip("設定アラーム表示オブジェクトの表示位置")]
    private Vector3 viewPositionOfViewAlarmsObject_ = Vector3.zero;
    [SerializeField, Tooltip("アラーム設定オブジェクトの最小スケール")]
    private Vector3 minimumScaleOfViewAlarmsObject_ = new Vector3(0.3f, 0.0f, 1.0f);
    [SerializeField, Tooltip("アラーム設定ボタンマネージャ")]
    private AlarmSettingButtonsManager alarmSettingButtonsManager_ = null;
    /// <summary>ショートカットした</summary>
    private bool isShortcut_ = false;


    // Start is called before the first frame update
    void Start() {
        if(this.clockAnchorObjectTransform_ == null) {
            Debug.LogError("Clock anchor object transform is null.");
        }
        if(this.viewAlarmsObjectTransform_ == null) {
            Debug.LogError("View alarms object transform is null.");
        }
        if(this.alarmSettingButtonsManager_ == null) {
            Debug.LogError("Alarm setting buttons manager is null.");
        }

        // 設定アラーム表示状態で戻るボタンが押されたら設定アラーム表示画面を閉じる
        MainSceneManager.instance.SetBackAction(MainSceneManager.ViewState.AlarmsView, this.OnUnviewViewAlarmsObject);
    } 


    /// <summary>設定アラーム表示オブジェクトを表示する</summary>
    public void OnViewViewAlarmsObjectNormal() {
        StartCoroutine(this.ViewViewAlarmsObjectNormal());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Select).Play();
    }
    /// <summary>設定アラーム表示オブジェクトを表示する</summary>
    private IEnumerator ViewViewAlarmsObjectNormal() {
        // アラーム表示状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.AlarmsView;

        this.isShortcut_ = false;
        this.viewAlarmsObjectTransform_.localScale = Vector3.one;
        // オブジェクトをアクティブにする
        this.viewAlarmsObjectTransform_.gameObject.SetActive(true);
        // 表示更新
        this.alarmSettingButtonsManager_.OnUpdateAllButtonsView();

        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.viewAlarmsObjectTransform_.localPosition = Vector3.Lerp(
                this.unviewPositionOfViewAlarmsObject_,
                this.viewPositionOfViewAlarmsObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.viewAlarmsObjectTransform_.localPosition = this.viewPositionOfViewAlarmsObject_;
    }
    /// <summary>ショートカットから設定アラーム表示オブジェクトを表示する</summary>
    public void OnViewViewAlarmsObjectShortcut() {
        StartCoroutine(this.ViewViewAlarmsObjectShortcut());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Select).Play();
    }
    /// <summary>ショートカットから設定アラーム表示オブジェクトを表示する</summary>
    private IEnumerator ViewViewAlarmsObjectShortcut() {
        // アラーム表示状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.AlarmsView;

        this.isShortcut_ = true;
        // オブジェクトをアクティブにする
        this.viewAlarmsObjectTransform_.gameObject.SetActive(true);
        // 表示更新
        this.alarmSettingButtonsManager_.OnUpdateAllButtonsView();

        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.viewAlarmsObjectTransform_.localPosition = Vector3.Lerp(
                this.clockAnchorObjectTransform_.localPosition,
                this.viewPositionOfViewAlarmsObject_,
                ratio
            );
            this.viewAlarmsObjectTransform_.localScale = Vector3.Lerp(
                this.minimumScaleOfViewAlarmsObject_,
                Vector3.one,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.viewAlarmsObjectTransform_.localPosition = this.viewPositionOfViewAlarmsObject_;
        this.viewAlarmsObjectTransform_.localScale = Vector3.one;
    }

    /// <summary>設定アラーム表示オブジェクトを非表示にする</summary>
    public void OnUnviewViewAlarmsObject() {
        if(this.isShortcut_) {
            StartCoroutine(this.UnviewViewAlarmsObjectShortcut());
        }
        else {
            StartCoroutine(this.UnviewViewAlarmsObjectNormal());
        }
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Cancel).Play();
    }
    /// <summary>設定アラーム表示オブジェクトを非表示にする</summary>
    private IEnumerator UnviewViewAlarmsObjectNormal() {
        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.viewAlarmsObjectTransform_.localPosition = Vector3.Lerp(
                this.viewPositionOfViewAlarmsObject_,
                this.unviewPositionOfViewAlarmsObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.viewAlarmsObjectTransform_.localPosition = this.unviewPositionOfViewAlarmsObject_;

        // オブジェクトを非アクティブにする
        this.viewAlarmsObjectTransform_.gameObject.SetActive(false);
        // 設定メニュー表示状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.OptionMenu;
    }
    /// <summary>ショートカットから設定アラーム表示オブジェクトを表示する</summary>
    private IEnumerator UnviewViewAlarmsObjectShortcut() {
        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.viewAlarmsObjectTransform_.localPosition = Vector3.Lerp(
                this.viewPositionOfViewAlarmsObject_,
                this.clockAnchorObjectTransform_.localPosition,
                ratio
            );
            this.viewAlarmsObjectTransform_.localScale = Vector3.Lerp(
                Vector3.one,
                this.minimumScaleOfViewAlarmsObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.viewAlarmsObjectTransform_.localPosition = this.unviewPositionOfViewAlarmsObject_;
        this.viewAlarmsObjectTransform_.localScale = Vector3.one;

        // オブジェクトを非アクティブにする
        this.viewAlarmsObjectTransform_.gameObject.SetActive(false);
        // メイン状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.Main;
    }
}
