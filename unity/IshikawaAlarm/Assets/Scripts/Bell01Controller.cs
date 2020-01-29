using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベル01コントローラ
/// </summary>
public class Bell01Controller : AlarmingSoundController {
    [SerializeField, Tooltip("音")]
    private AudioSource audioSource_ = null;

    
    // Start is called before the first frame update
    void Start() {
        if(this.audioSource_ == null) {
            Debug.LogError("Audio source is null.");
        }
    }

    /// <summary>
    /// アラーム開始
    /// </summary>
    public override void OnPlayAlarming() {
        this.audioSource_.volume = 0.01f * ConfigDataManager.instance.alarmVolume;

        if(!(this.audioSource_.isPlaying)) {
            this.audioSource_.Play();
        }
    }
    /// <summary>
    /// アラーム終了
    /// </summary>
    public override void OnStopAlarming() {
        if(this.audioSource_.isPlaying) {
            this.audioSource_.Stop();
        }
    }
}
