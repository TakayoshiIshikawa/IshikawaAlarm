using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アラーム時間設定コントローラ
/// </summary>
public class AlarmTimeSettingController : MonoBehaviour {
    [SerializeField, Tooltip("時カウンタ")]
    private NumberCounterController hourCounter_ = null;
    [SerializeField, Tooltip("分カウンタ")]
    private NumberCounterController minuteCounter_ = null;
    /// <summary>時</summary>
    public int hour {
        get { return this.hourCounter_.intNumber; }
        set { this.hourCounter_.intNumber = value; }
    }
    /// <summary>分</summary>
    public int minute {
        get { return this.minuteCounter_.intNumber; }
        set { this.minuteCounter_.intNumber = value; }
    }
    /// <summary>動いているか</summary>
    public bool isMoving {
        get { return (this.hourCounter_.isMoving || this.minuteCounter_.isMoving); }
    }


    // Start is called before the first frame update
    void Start() {
        if(this.hourCounter_ == null) {
            Debug.LogError("Hour counter is null.");
        }
        if(this.minuteCounter_ == null) {
            Debug.LogError("Minute counter is null.");
        }
    }
    // Update is called once per frame
    void Update() {
        
    }
}
