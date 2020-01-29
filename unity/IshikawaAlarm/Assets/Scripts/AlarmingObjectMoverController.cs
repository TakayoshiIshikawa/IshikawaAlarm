using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アラーム中移動コントローラ
/// </summary>
public class AlarmingObjectMoverController : MonoBehaviour {
    [SerializeField, Tooltip("アンドロイド接続用")]
    private AndroidConnector androidConnector_ = null;
    [SerializeField, Tooltip("変更時間")]
    private float changeTime_ = 0.3f;
    [SerializeField, Tooltip("アラーム中オブジェクトコントローラ")]
    private AlarmingObjectController alarmingObjectController_ = null;
    /// <summary>アラーム中オブジェクトトランスフォーム</summary>
    private Transform alarmingObjectTransform_ = null;
    /// <summary>アラーム中オブジェクトトランスフォーム</summary>
    private Transform alarmingObjectTransform {
        get {
            if(this.alarmingObjectTransform_ == null) {
                this.alarmingObjectTransform_ = this.alarmingObjectController_.gameObject.transform;
            }
            return this.alarmingObjectTransform_;
        }
    }
    [SerializeField, Tooltip("アラーム中オブジェクトの非表示位置")]
    private Vector3 unviewPositionOfAlarmingObject_ = new Vector3(0.0f, 1280.0f, 0.0f);
    [SerializeField, Tooltip("アラーム中オブジェクトの表示位置")]
    private Vector3 viewPositionOfAlarmingObject_ = Vector3.zero;
    /// <summary>アラームインデックス</summary>
    private int alarmIndex_ = -1;
    /// <summary>直前の表示状態</summary>
    private MainSceneManager.ViewState beforeViewState_ = MainSceneManager.ViewState.Main;


    // Start is called before the first frame update
    public void Start() {
        if(this.androidConnector_ == null) {
            Debug.LogError("Android connector is null.");
        }
        if(this.alarmingObjectController_ == null) {
            Debug.LogError("Alarming object controller is null.");
        }
        this.alarmingObjectTransform_ = this.alarmingObjectController_.gameObject.transform;
    }

    /// <summary>アラーム中オブジェクトを表示する</summary>
    /// <param name="_alarmIndex">アラームインデックス</param>
    public void OnViewAlarmingObject(int _alarmIndex) {
        AlarmDataManager.instance.isAlarming = true;
        this.alarmIndex_ = _alarmIndex;
        StartCoroutine(this.ViewAlarmingObject());
    }
    /// <summary>アラーム中オブジェクトを表示する</summary>
    private IEnumerator ViewAlarmingObject() {
        // 直前の表示状態を保存して、アラーム中状態にする
        this.beforeViewState_ = MainSceneManager.instance.viewState;
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.Alarming;
        // BGM停止
        BackgroundMusicManager.instance.Stop();
        // 端末に通知
        if(AlarmDataManager.instance.isConnectAndroid) {
            this.androidConnector_.OnStartAlarm();
        }
        // オブジェクトをアクティブにする
        this.alarmingObjectTransform.gameObject.SetActive(true);
        // 表示切替え、アラーム開始
        this.alarmingObjectController_.OnChangeView(this.alarmIndex_);
        this.alarmingObjectController_.OnStartAlarm();

        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.alarmingObjectTransform.localPosition = Vector3.Lerp(
                this.unviewPositionOfAlarmingObject_,
                this.viewPositionOfAlarmingObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.alarmingObjectTransform.localPosition = this.viewPositionOfAlarmingObject_;
    }

    /// <summary>アラーム中オブジェクトを非表示にする</summary>
    public void OnUnviewAlarmingObject() {
        // アラームを再設定
        AlarmDataManager.instance.ResetAlarm(this.alarmIndex_);
        StartCoroutine(this.UnviewAlarmingObject());
    }
    /// <summary>アラーム中オブジェクトを非表示にする</summary>
    private IEnumerator UnviewAlarmingObject() {
        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.alarmingObjectTransform.localPosition = Vector3.Lerp(
                this.viewPositionOfAlarmingObject_,
                this.unviewPositionOfAlarmingObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.alarmingObjectTransform.localPosition = this.unviewPositionOfAlarmingObject_;

        // アラーム終了
        this.alarmingObjectController_.OnEndAlarm();
        // 端末に通知
        if(AlarmDataManager.instance.isConnectAndroid) {
            this.androidConnector_.OnEndAlarm();
        }
        // オブジェクトを非アクティブにする
        this.alarmingObjectTransform.gameObject.SetActive(false);
        // BGM開始
        BackgroundMusicManager.instance.Play();
        // 直前の状態に戻す
        MainSceneManager.instance.viewState = this.beforeViewState_;
        AlarmDataManager.instance.isAlarming = false;
    }
}
