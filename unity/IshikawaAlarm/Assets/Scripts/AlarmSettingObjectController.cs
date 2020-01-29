using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アラーム設定オブジェクトコントローラ
/// </summary>
public class AlarmSettingObjectController : MonoBehaviour {
    [SerializeField, Tooltip("タイトル入力フィールド")]
    private InputField titleInputField_ = null;
    [SerializeField, Tooltip("メモ入力フィールド")]
    private InputField memoInputField_ = null;
    [SerializeField, Tooltip("アラーム時間設定コントローラ")]
    private AlarmTimeSettingController alarmTimeSettingController_ = null;
    [SerializeField, Tooltip("曜日設定コントローラ")]
    private DayOfWeekSettingController dayOfWeekSettingController_ = null;
    /// <summary>ID</summary>
    private int id_ = -1;
    /// <summary>設定データ</summary>
    public AlarmSaveObject setting {
        get {
            AlarmSaveObject data = new AlarmSaveObject();
            data.id = this.id_;
            data.title = this.titleInputField_.text;
            data.message = this.memoInputField_.text;
            data.time[3] = this.alarmTimeSettingController_.hour;
            data.time[4] = this.alarmTimeSettingController_.minute;
            data.isActivateOnDayOfWeek[0] = this.dayOfWeekSettingController_.states[0];
            data.isActivateOnDayOfWeek[1] = this.dayOfWeekSettingController_.states[1];
            data.isActivateOnDayOfWeek[2] = this.dayOfWeekSettingController_.states[2];
            data.isActivateOnDayOfWeek[3] = this.dayOfWeekSettingController_.states[3];
            data.isActivateOnDayOfWeek[4] = this.dayOfWeekSettingController_.states[4];
            data.isActivateOnDayOfWeek[5] = this.dayOfWeekSettingController_.states[5];
            data.isActivateOnDayOfWeek[6] = this.dayOfWeekSettingController_.states[6];
            return data;
        }
        set {
            this.id_ = value.id;
            this.titleInputField_.text = value.title;
            this.memoInputField_.text = value.message;
            this.alarmTimeSettingController_.hour = value.time[3];
            this.alarmTimeSettingController_.minute = value.time[4];
            this.dayOfWeekSettingController_.states = value.isActivateOnDayOfWeek;
        }
    }
    public bool isClosable {
        get { return !(this.alarmTimeSettingController_.isMoving); }
    }


    // Start is called before the first frame update
    public void Start() {
        if(this.titleInputField_ == null) {
            Debug.LogError("Title input field is null.");
        }
        if(this.memoInputField_ == null) {
            Debug.LogError("Memo input field is null.");
        }
        if(this.alarmTimeSettingController_ == null) {
            Debug.LogError("Alarm time setting controller is null.");
        }
        if(this.dayOfWeekSettingController_ == null) {
            Debug.LogError("Day of week setting controller is null.");
        }
    }
}
