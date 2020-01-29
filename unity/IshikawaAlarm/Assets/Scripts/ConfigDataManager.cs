using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 設定データマネージャ
/// </summary>
public class ConfigDataManager : SingletonMonoBehaviour<ConfigDataManager> {
    /// <summary>保存用タグ</summary>
    private static readonly string PLAYER_PREFS_TAG = "config_save";
    /// <summary>保存データ</summary>
    [Serializable]
    public class SaveData {
        /// <summary>BGM音量</summary>
        [SerializeField]
        private int bgmVolume_ = 100;
        /// <summary>BGM音量</summary>
        public int bgmVolume {
            get { return this.bgmVolume_; }
            set { this.bgmVolume_ = value; }
        }
        /// <summary>SE音量</summary>
        [SerializeField]
        private int soundEffectVolume_ = 100;
        /// <summary>SE音量</summary>
        public int soundEffectVolume {
            get { return this.soundEffectVolume_; }
            set { this.soundEffectVolume_ = value; }
        }
        /// <summary>アラーム音量</summary>
        [SerializeField]
        private int alarmVolume_ = 100;
        /// <summary>アラーム音量</summary>
        public int alarmVolume {
            get { return this.alarmVolume_; }
            set { this.alarmVolume_ = value; }
        }
    }


    [Header("デフォルト値")]
    [SerializeField, Tooltip("BGM音量"), Range(0, 100)]
    private int defaultBGMVolume_ = 20;
    [SerializeField, Tooltip("SE音量"), Range(0, 100)]
    private int defaultSEVolume_ = 60;
    [SerializeField, Tooltip("アラーム音量"), Range(0, 100)]
    private int defaultAlarmVolume_ = 60;
    [Space]
    [SerializeField, Tooltip("セーブフラグ")]
    private bool isSave_ = true;
    /// <summary>保存データ</summary>
    private SaveData saveData_ = new SaveData();
    /// <summary>保存データ</summary>
    public SaveData saveData {
        get { return this.saveData_; }
        set {
            this.bgmVolume = value.bgmVolume;
            this.soundEffectVolume = value.soundEffectVolume;
            this.alarmVolume = value.alarmVolume;
        }
    }
    /// <summary>BGM音量</summary>
    public int bgmVolume {
        get { return this.saveData_.bgmVolume; }
        set {
            this.saveData_.bgmVolume = value;
            BackgroundMusicManager.instance.volume = value;
        }
    }
    /// <summary>SE音量</summary>
    public int soundEffectVolume {
        get { return this.saveData_.soundEffectVolume; }
        set {
            this.saveData_.soundEffectVolume = value;
            SoundEffectsManager.instance.volume = value;
        }
    }
    /// <summary>アラーム音量</summary>
    public int alarmVolume {
        get { return this.saveData_.alarmVolume; }
        set {
            this.saveData_.alarmVolume = value;
        }
    }


    // Start is called before the first frame update
    public void Start() {
        if(PlayerPrefs.HasKey(ConfigDataManager.PLAYER_PREFS_TAG)) {
            this.Load();
        }
        else {
            this.Create();
        }
    }


    /// <summary>
    /// 新規作成
    /// </summary>
    private void Create() {
        Debug.Log("Create config data.");

        this.bgmVolume = defaultBGMVolume_;
        this.soundEffectVolume = defaultSEVolume_;
        this.alarmVolume = defaultAlarmVolume_;
    }
    /// <summary>
    /// 読込
    /// </summary>
    private void Load() {
        string configSaveJson = PlayerPrefs.GetString(ConfigDataManager.PLAYER_PREFS_TAG);
        Debug.Log("Load config data : " + configSaveJson);
        this.saveData = JsonUtility.FromJson<ConfigDataManager.SaveData>(configSaveJson);
    }
    /// <summary>
    /// 保存
    /// </summary>
    private void Save() {
        string configSaveJson = JsonUtility.ToJson(this.saveData_);
        PlayerPrefs.SetString(ConfigDataManager.PLAYER_PREFS_TAG, configSaveJson);
        Debug.Log("Save config data : " + configSaveJson);
        PlayerPrefs.Save();
        if(!(this.isSave_)) {
            PlayerPrefs.DeleteKey(ConfigDataManager.PLAYER_PREFS_TAG);
        }
    }

    /// <summary>
    /// 一時停止、及び再開処理
    /// </summary>
    /// <param name="_isPause">ポーズフラグ</param>
    public void OnApplicationPause(bool _isPause) {
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
        this.Save();
    }
}
