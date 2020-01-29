using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 設定オブジェクトコントローラ
/// </summary>
public class ConfigObjectController : MonoBehaviour {
    [SerializeField, Tooltip("BGM音量テキスト")]
    private Text bgmVolumeText_ = null;
    [SerializeField, Tooltip("BGM音量スライダー")]
    private Slider bgmVolumeSlider_ = null;
    /// <summary>BGM音量</summary>
    private int bgmVolume {
        get { return (int)this.bgmVolumeSlider_.value; }
        set {
            this.bgmVolumeSlider_.value = value;
            this.bgmVolumeText_.text = value.ToString();
        }
    }
    [SerializeField, Tooltip("SE音量テキスト")]
    private Text seVolumeText_ = null;
    [SerializeField, Tooltip("SE音量スライダー")]
    private Slider seVolumeSlider_ = null;
    /// <summary>SE音量</summary>
    private int seVolume {
        get { return (int)this.seVolumeSlider_.value; }
        set {
            this.seVolumeSlider_.value = value;
            this.seVolumeText_.text = value.ToString();
        }
    }
    [SerializeField, Tooltip("アラーム音量テキスト")]
    private Text alarmVolumeText_ = null;
    [SerializeField, Tooltip("アラーム音量スライダー")]
    private Slider alarmVolumeSlider_ = null;
    /// <summary>アラーム音量</summary>
    private int alarmVolume {
        get { return (int)this.alarmVolumeSlider_.value; }
        set {
            this.alarmVolumeSlider_.value = value;
            this.alarmVolumeText_.text = value.ToString();
        }
    }
    /// <summary>設定</summary>
    public ConfigDataManager.SaveData setting {
        get {
            ConfigDataManager.SaveData data = new ConfigDataManager.SaveData();
            data.bgmVolume = this.bgmVolume;
            data.soundEffectVolume = this.seVolume;
            data.alarmVolume = this.alarmVolume;
            return data;
        }
        set {
            this.bgmVolume = value.bgmVolume;
            this.seVolume = value.soundEffectVolume;
            this.alarmVolume = value.alarmVolume;
        }
    }


    // Start is called before the first frame update
    public void Start() {
        if(this.bgmVolumeText_ == null) {
            Debug.LogError("BGM volume text is null.");
        }
        if(this.bgmVolumeSlider_ == null) {
            Debug.LogError("BGM volume slider is null.");
        }
        if(this.seVolumeText_ == null) {
            Debug.LogError("SE volume text is null.");
        }
        if(this.seVolumeSlider_ == null) {
            Debug.LogError("SE volume slider is null.");
        }
        if(this.alarmVolumeText_ == null) {
            Debug.LogError("Alarm volume text is null.");
        }
        if(this.alarmVolumeSlider_ == null) {
            Debug.LogError("Alarm volume slider is null.");
        }
    }


    /// <summary>
    /// BGMスライダーが変更されたときの処理
    /// </summary>
    public void OnChangeBGMSlider() {
        // BGM音量変更
        BackgroundMusicManager.instance.volume = this.bgmVolume;
        this.bgmVolumeText_.text = this.bgmVolume.ToString();
    }
    /// <summary>
    /// SEスライダーが変更されたときの処理
    /// </summary>
    public void OnChangeSESlider() {
        this.seVolumeText_.text = this.seVolume.ToString();
    }
    /// <summary>
    /// アラームスライダーが変更されたときの処理
    /// </summary>
    public void OnChangeAlarmSlider() {
        this.alarmVolumeText_.text = this.alarmVolume.ToString();
    }
}
