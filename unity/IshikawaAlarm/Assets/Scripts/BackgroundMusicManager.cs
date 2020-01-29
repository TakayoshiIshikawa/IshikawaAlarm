using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGMマネージャ
/// </summary>
public class BackgroundMusicManager : SingletonMonoBehaviour<BackgroundMusicManager> {
    /// <summary>
    /// BGM名
    /// </summary>
    public enum BackgroundMusicName {
        /// <summary>決定</summary>
        MagicCooking = 0
    }
    

    [SerializeField, Tooltip("BGMのオーディオソース")]
    private AudioSource audioSource_ = null;
    [SerializeField, Tooltip("始めのBGM")]
    private BackgroundMusicName initialBackgroundMusic_ = BackgroundMusicName.MagicCooking;
    [SerializeField, Tooltip("BGMリスト")]
    private List<AudioClip> audioClipList_ = new List<AudioClip>();
    /// <summary>音量</summary>
    private float volume_ = 1.0f;
    /// <summary>音量</summary>
    public int volume {
        get { return (int)(100 * this.volume_); }
        set {
            this.volume_ = Mathf.Clamp01(0.01f * value);
            this.audioSource_.volume = this.volume_;
        }
    }
    /// <summary>現在のBGM名</summary>
    private BackgroundMusicName current_ = BackgroundMusicName.MagicCooking;
    /// <summary>現在のBGM名</summary>
    public BackgroundMusicName current {
        get { return this.current_; }
        set {
            if(this.audioSource_.isPlaying) {
                // 停止して、クリップ変えて、流す
                this.audioSource_.Stop();
                this.audioSource_.clip = this.GetBackgroundMusic(value);
                this.audioSource_.Play();
            }
            else {
                // クリップ変更のみ
                this.audioSource_.clip = this.GetBackgroundMusic(value);
            }
            this.current_ = value;
        }
    }
    /// <summary>再生中かのフラグ</summary>
    public bool isPlaying {
        get { return this.audioSource_.isPlaying; }
    }


    // Start is called before the first frame update
    void Start() {
        if(this.audioSource_ == null) {
            Debug.LogError("Audio source is null.");
        }

        this.current = this.initialBackgroundMusic_;
        this.Play();
    } 
    

    /// <summary>
    /// BGM取得
    /// </summary>
    /// <param name="_name">BGM名</param>
    /// <returns>BGMのオーディオクリップ</returns>
    private AudioClip GetBackgroundMusic(BackgroundMusicName _name) {
        return this.audioClipList_[(int)_name];
    }


    /// <summary>
    /// 開始
    /// </summary>
    public void Play() {
        if(this.audioSource_.clip != null) {
            this.audioSource_.Play();
        }
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop() {
        if(this.audioSource_.isPlaying) {
            this.audioSource_.Stop();
        }
    }
}
