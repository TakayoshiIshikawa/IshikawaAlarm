using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 24時間表記時分時計コントローラ
/// </summary>
public class HM24ClockController : ClockViewController {
    /// <summary>表示テキスト</summary>
    [SerializeField, Tooltip("表示テキスト")]
    private Text viewText_ = null;

    /// <summary>表示時間</summary>
    private DateTime time_;
    /// <summary>表示時間</summary>
    public override DateTime time {
        get { return this.time_; }
        set {
            this.time_ = value;
            this.viewText_.text = value.ToString("HH:mm");
        }
    }


    // Start is called before the first frame update
    void Start() {
        if(this.viewText_ == null) {
            Debug.LogError("View text is null.");
        }
    }
}
