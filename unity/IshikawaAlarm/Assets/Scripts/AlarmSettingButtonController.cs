using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アラーム設定ボタンコントローラ
/// </summary>
public class AlarmSettingButtonController : MonoBehaviour {
    [SerializeField, Tooltip("表示アラームインデックス")]
    private int viewAlarmIndex_ = -1;
    /// <summary>表示アラームインデックス</summary>
    public int viewAlarmIndex {
        get { return this.viewAlarmIndex_; }
    }
    [Space]
    [SerializeField, Tooltip("アラーム設定ボタンマネージャ")]
    private AlarmSettingButtonsManager alarmSettingButtonsManager_ = null;
    [Space]
    [SerializeField, Tooltip("タイトルテキスト")]
    private Text titleText_ = null;
    [SerializeField, Tooltip("時間テキスト")]
    private Text timeText_ = null;
    [SerializeField, Tooltip("日曜フラグイメージ")]
    private Image sumdayFlagImage_ = null;
    [SerializeField, Tooltip("月曜フラグイメージ")]
    private Image mondayFlagImage_ = null;
    [SerializeField, Tooltip("火曜フラグイメージ")]
    private Image tuesdayFlagImage_ = null;
    [SerializeField, Tooltip("水曜フラグイメージ")]
    private Image wednesdayFlagImage_ = null;
    [SerializeField, Tooltip("木曜フラグイメージ")]
    private Image thursdayFlagImage_ = null;
    [SerializeField, Tooltip("金曜フラグイメージ")]
    private Image fridayFlagImage_ = null;
    [SerializeField, Tooltip("土曜フラグイメージ")]
    private Image saturdayFlagImage_ = null;


    // Start is called before the first frame update
    public void Start() {
        if(this.alarmSettingButtonsManager_ == null) {
            Debug.LogError("Alarm setting buttons manager is null.");
        }
        if(this.titleText_ == null) {
            Debug.LogError("Title text is null.");
        }
        if(this.timeText_ == null) {
            Debug.LogError("Time text is null.");
        }
        if(this.sumdayFlagImage_ == null) {
            Debug.LogError("Sumday flag image is null.");
        }
        if(this.mondayFlagImage_ == null) {
            Debug.LogError("Monday flag image is null.");
        }
        if(this.tuesdayFlagImage_ == null) {
            Debug.LogError("Tuesday flag image is null.");
        }
        if(this.wednesdayFlagImage_ == null) {
            Debug.LogError("Wednesday flag image is null.");
        }
        if(this.thursdayFlagImage_ == null) {
            Debug.LogError("Thursday flag image is null.");
        }
        if(this.fridayFlagImage_ == null) {
            Debug.LogError("Friday flag image is null.");
        }
        if(this.saturdayFlagImage_ == null) {
            Debug.LogError("Saturday flag image is null.");
        }
    }

    /// <summary>
    /// 表示を更新
    /// </summary>
    public void OnUpdateView() {
        AlarmSaveObject savedata = AlarmDataManager.instance.GetAlarm(this.viewAlarmIndex);

        this.titleText_.text = savedata.title;
        this.timeText_.text = savedata.time[3].ToString("00") + ":" + savedata.time[4].ToString("00");
        this.sumdayFlagImage_.color = (savedata.isActivateOnDayOfWeek[0]) ?
            this.alarmSettingButtonsManager_.sundayActiveColor :
            this.alarmSettingButtonsManager_.sundayDeactiveColor;
        this.mondayFlagImage_.color = (savedata.isActivateOnDayOfWeek[1]) ?
            this.alarmSettingButtonsManager_.mondayActiveColor :
            this.alarmSettingButtonsManager_.mondayDeactiveColor;
        this.tuesdayFlagImage_.color = (savedata.isActivateOnDayOfWeek[2]) ?
            this.alarmSettingButtonsManager_.tuesdayActiveColor :
            this.alarmSettingButtonsManager_.tuesdayDeactiveColor;
        this.wednesdayFlagImage_.color = (savedata.isActivateOnDayOfWeek[3]) ?
            this.alarmSettingButtonsManager_.wednesdayActiveColor :
            this.alarmSettingButtonsManager_.wednesdayDeactiveColor;
        this.thursdayFlagImage_.color = (savedata.isActivateOnDayOfWeek[4]) ?
            this.alarmSettingButtonsManager_.thursdayActiveColor :
            this.alarmSettingButtonsManager_.thursdayDeactiveColor;
        this.fridayFlagImage_.color = (savedata.isActivateOnDayOfWeek[5]) ?
            this.alarmSettingButtonsManager_.fridayActiveColor :
            this.alarmSettingButtonsManager_.fridayDeactiveColor;
        this.saturdayFlagImage_.color = (savedata.isActivateOnDayOfWeek[6]) ?
            this.alarmSettingButtonsManager_.saturdayActiveColor :
            this.alarmSettingButtonsManager_.saturdayDeactiveColor;
    }
}
