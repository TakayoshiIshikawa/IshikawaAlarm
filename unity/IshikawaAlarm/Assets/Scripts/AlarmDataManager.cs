using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アラームデータマネージャ
/// </summary>
public class AlarmDataManager : SingletonMonoBehaviour<AlarmDataManager> {
    /// <summary>保存用タグ</summary>
    private static readonly string PLAYER_PREFS_TAG = "alarms_save";
    /// <summary>保存データ</summary>
    [Serializable]
    private class SaveData {
        /// <summary>アラーム配列</summary>
        [SerializeField]
        public AlarmSaveObject[] alarmArray_ = null;
    }

    [SerializeField, Tooltip("アンドロイド接続用")]
    private AndroidConnector androidConnector_ = null;
    [SerializeField, Tooltip("アンドロイド接続フラグ")]
    private bool isConnectAndroid_ = true;
    /// <summary>アンドロイド接続フラグ</summary>
    public bool isConnectAndroid {
        get { return this.isConnectAndroid_; }
    }
    [Space]
    [SerializeField, Tooltip("アラーム中オブジェクト移動コントローラ")]
    private AlarmingObjectMoverController alarmingObjectMoverController_ = null;
    [SerializeField, Tooltip("アラームチェック間隔")]
    private float alarmCheckInterval_ = 1.0f;
    [Space]
    [SerializeField, Tooltip("セーブフラグ")]
    private bool isSave_ = true;
    /// <summary>セーブデータ</summary>
    private AlarmDataManager.SaveData saveData_ = new AlarmDataManager.SaveData();
    /// <summary>アラーム中</summary>
    private bool isAlarming_ = false;
    /// <summary>アラーム中</summary>
    public bool isAlarming {
        get { return this.isAlarming_; }
        set { this.isAlarming_ = value; }
    }
    /// <summary>ポーズ中</summary>
    private bool isPause_ = false;
    /// <summary>終了している</summary>
    private bool isQuit_ = false;


    // Start is called before the first frame update
    public void Start() {
        if(this.androidConnector_ == null) {
            Debug.LogError("Android connector is null.");
        }
        if(this.alarmingObjectMoverController_ == null) {
            Debug.LogError("Alarming object mover controller is null.");
        }


        if(PlayerPrefs.HasKey(AlarmDataManager.PLAYER_PREFS_TAG)) {
            this.Load();
            StartCoroutine(this.AlarmCheckCoroutine());
            for(int i=0; i<this.saveData_.alarmArray_.Length; ++i) {
                this.StartSavedAlarm(i);
            }
        }
        else {
            this.Create();
            StartCoroutine(this.AlarmCheckCoroutine());
        }
    }

    
    /// <summary>
    /// アラーム設定の取得
    /// </summary>
    /// <param name="_index">取得するインデックス</param>
    public AlarmSaveObject GetAlarm(int _index) {
        return this.saveData_.alarmArray_[_index];
    }
    /// <summary>
    /// アラーム設定の設定
    /// </summary>
    /// <param name="_index">設定するインデックス</param>
    /// <param name="_saveObject">設定データ</param>
    public void SetAlarm(int _index, AlarmSaveObject _saveObject) {
        bool isKill = this.saveData_.alarmArray_[_index].isActive;
        int killId = this.saveData_.alarmArray_[_index].id;

        this.saveData_.alarmArray_[_index] = _saveObject;

        if(_saveObject.isActive) {
            this.ResetAlarm(_index);
        }
        else if(isKill) {
            if(this.isConnectAndroid_) {
                this.androidConnector_.KillAlarm(killId);
            }
        }
    }


    /// <summary>
    /// 新規作成
    /// </summary>
    private void Create() {
        Debug.Log("Create alarm data.");

        // TODO: サイズを変更できるようにしたい
        int size = 4;
        this.saveData_.alarmArray_ = new AlarmSaveObject[4];

        for(int i=0; i<size; ++i) {
            AlarmSaveObject data = new AlarmSaveObject();
            data.id = i + 1;
            data.title = "Alarm" + data.id.ToString();
            this.saveData_.alarmArray_[i] = data;
        }
    }
    /// <summary>
    /// 読込
    /// </summary>
    private void Load() {
        string alarmArrayJson = PlayerPrefs.GetString(AlarmDataManager.PLAYER_PREFS_TAG);
        Debug.Log("Load alarm data : " + alarmArrayJson);
        this.saveData_ = JsonUtility.FromJson<AlarmDataManager.SaveData>(alarmArrayJson);
    }
    /// <summary>
    /// 保存
    /// </summary>
    private void Save() {
        string alarmArrayJson = JsonUtility.ToJson(this.saveData_);
        PlayerPrefs.SetString(AlarmDataManager.PLAYER_PREFS_TAG, alarmArrayJson);
        Debug.Log("Save alarm data : " + alarmArrayJson);
        PlayerPrefs.Save();
        if(!(this.isSave_)) {
            PlayerPrefs.DeleteKey(AlarmDataManager.PLAYER_PREFS_TAG);
        }
    }


    /// <summary>
    /// 保存していたアラームの開始
    /// </summary>
    /// <param name="_index">アラームインデックス</param>
    public void StartSavedAlarm(int _index) {
        AlarmSaveObject alarm = this.saveData_.alarmArray_[_index];
        if(alarm.isActive) {
            DateTime time = new DateTime(
                alarm.time[0], alarm.time[1], alarm.time[2],
                alarm.time[3], alarm.time[4], alarm.time[5]
            );

            Debug.Log("Start : " + time.ToString());
            if(this.isConnectAndroid_) {
                // アンドロイド端末に通知
                this.androidConnector_.SetAlarm(alarm.id, time);
            }
        }
    }

    /// <summary>
    /// アラームの再設定
    /// </summary>
    /// <param name="_index">アラームインデックス</param>
    public void ResetAlarm(int _index) {
        AlarmSaveObject alarm = this.saveData_.alarmArray_[_index];
        if(alarm.isActive) {
            DateTime time = new DateTime(
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                alarm.time[3], alarm.time[4], alarm.time[5]
            );

            if(alarm.IsActivateOnDayOfWeek(DateTime.Now.DayOfWeek)) {
                // 今日が対象のとき
                if(time > DateTime.Now) {
                    this.saveData_.alarmArray_[_index].time[0] = DateTime.Now.Year;
                    this.saveData_.alarmArray_[_index].time[1] = DateTime.Now.Month;
                    this.saveData_.alarmArray_[_index].time[2] = DateTime.Now.Day;
                    Debug.Log("Set : " + time.ToString());

                    // まだすぎていないなら今日に設定する
                    if(this.isConnectAndroid_) {
                        this.androidConnector_.SetAlarm(alarm.id, time);
                    }
                    return;
                }
            }
            // 1週間分調べる
            for(int i=0; i<7; ++i) {
                time = time.AddDays(1.0);
                if(alarm.IsActivateOnDayOfWeek(time.DayOfWeek)) {
                    this.saveData_.alarmArray_[_index].time[0] = time.Year;
                    this.saveData_.alarmArray_[_index].time[1] = time.Month;
                    this.saveData_.alarmArray_[_index].time[2] = time.Day;
                    Debug.Log("Set : " + time.ToString());
                    // 設定
                    if(this.isConnectAndroid_) {
                        this.androidConnector_.SetAlarm(alarm.id, time);
                    }
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 全てのアラームを再設定
    /// </summary>
    private void ResetAllAlarm() {
        for(int i=0; i<this.saveData_.alarmArray_.Length; ++i) {
            this.ResetAlarm(i);
        }
    }

    
    /// <summary>
    /// アラームのチェック
    /// </summary>
    /// <param name="_index">アラームインデックス</param>
    /// <returns>true:時間になった false:時間になっていない</returns>
    private bool CheckAlarm(int _index) {
        AlarmSaveObject alarm = this.saveData_.alarmArray_[_index];

        // 年が小さすぎる値のときは無視
        if(alarm.time[0] < 1000) {
            return false;
        }

        if(alarm.IsActivateOnDayOfWeek(DateTime.Now.DayOfWeek)) {
            DateTime time = new DateTime(
                alarm.time[0], alarm.time[1], alarm.time[2],
                alarm.time[3], alarm.time[4], alarm.time[5]
            );

            if(time <= DateTime.Now) {
                // 過ぎている
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 全てのアラームをチェック
    /// </summary>
    /// <returns>
    /// 0以上:時間になったアラームのインデックス
    /// -1:時間になったアラームはない
    /// </returns>
    public int CheckAllAlarm() {
        for(int i=0; i<this.saveData_.alarmArray_.Length; ++i) {
            if(this.CheckAlarm(i)) {
                return i;
            }
        }
        return -1;
    }


    /// <summary>
    /// 一時停止、及び再開処理
    /// </summary>
    /// <param name="_isPause">ポーズフラグ</param>
    public void OnApplicationPause(bool _isPause) {
        this.isPause_ = _isPause;
        if(_isPause) {
            // 一時停止時
            this.Save();
        }
        else {
            // 再開時
        }
    }
    /// <summary>
    /// 終了処理 (強制終了時などは呼ばれない)
    /// </summary>
    public void OnApplicationQuit() {
        this.isQuit_ = true;
        this.Save();
    }


    /// <summary>
    /// アラームチェック用コルーチン
    /// </summary>
    /// <returns>コルーチン用のデータ</returns>
    private IEnumerator AlarmCheckCoroutine() {
        while(!(this.isQuit_)) {
            if(!(this.isAlarming || this.isPause_)) {
                int index = this.CheckAllAlarm();
                if(index >= 0) {
                    // アラームを鳴らす
                    this.alarmingObjectMoverController_.OnViewAlarmingObject(index);
                }
            }
            yield return new WaitForSeconds(this.alarmCheckInterval_);
        }
    }

    /// <summary>
    /// アラーム終了
    /// </summary>
    public void OnEndAlarming() {
        this.isAlarming = false;
    }
}
