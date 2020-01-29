using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SEマネージャ
/// </summary>
public class SoundEffectsManager : SingletonMonoBehaviour<SoundEffectsManager> {
    /// <summary>
    /// SE名
    /// </summary>
    public enum SoundEffectName {
        /// <summary>決定</summary>
        Select = 0,
        /// <summary>戻る</summary>
        Cancel
    }

    [SerializeField, Tooltip("SEリスト")]
    private List<AudioSource> audioSourceList_ = new List<AudioSource>();
    /// <summary>音量</summary>
    private float volume_ = 1.0f;
    /// <summary>音量</summary>
    public int volume {
        get { return (int)(100 * this.volume_); }
        set {
            this.volume_ = Mathf.Clamp01(0.01f * value);
            foreach(AudioSource se in this.audioSourceList_) {
                se.volume = this.volume_;
            }
        }
    }


    /// <summary>
    /// SE取得
    /// </summary>
    /// <param name="_name">SE名</param>
    /// <returns>SEのオーディオソース</returns>
    public AudioSource GetSoundEffect(SoundEffectName _name) {
        return this.audioSourceList_[(int)_name];
    }
}
