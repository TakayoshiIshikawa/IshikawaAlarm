using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曜日設定コントローラ
/// </summary>
public class DayOfWeekSettingController : MonoBehaviour {
    [SerializeField, Tooltip("日曜のトグル")]
    private Toggle sumdayToggle_ = null;
    [SerializeField, Tooltip("月曜のトグル")]
    private Toggle mondayToggle_ = null;
    [SerializeField, Tooltip("火曜のトグル")]
    private Toggle tuesdayToggle_ = null;
    [SerializeField, Tooltip("水曜のトグル")]
    private Toggle wednesdayToggle_ = null;
    [SerializeField, Tooltip("木曜のトグル")]
    private Toggle thursdayToggle_ = null;
    [SerializeField, Tooltip("金曜のトグル")]
    private Toggle fridayToggle_ = null;
    [SerializeField, Tooltip("土曜のトグル")]
    private Toggle saturdayToggle_ = null;
    /// <summary>休日がアクティブか?</summary>
    private bool isActiveTheHoliday {
        get {
            return (
                this.sumdayToggle_.isOn &&
                this.saturdayToggle_.isOn
            );
        }
        set {
            this.sumdayToggle_.isOn = value;
            this.saturdayToggle_.isOn = value;
        }
    }
    /// <summary>平日がアクティブか?</summary>
    private bool isActiveTheWorkday {
        get {
            return (
                this.mondayToggle_.isOn &&
                this.tuesdayToggle_.isOn &&
                this.wednesdayToggle_.isOn &&
                this.thursdayToggle_.isOn &&
                this.fridayToggle_.isOn
            );
        }
        set {
            this.mondayToggle_.isOn = value;
            this.tuesdayToggle_.isOn = value;
            this.wednesdayToggle_.isOn = value;
            this.thursdayToggle_.isOn = value;
            this.fridayToggle_.isOn = value;
        }
    }
    /// <summary>毎日がアクティブか?</summary>
    private bool isActiveTheEveryday {
        get {
            return (
                this.sumdayToggle_.isOn &&
                this.mondayToggle_.isOn &&
                this.tuesdayToggle_.isOn &&
                this.wednesdayToggle_.isOn &&
                this.thursdayToggle_.isOn &&
                this.fridayToggle_.isOn &&
                this.saturdayToggle_.isOn
            );
        }
        set {
            this.sumdayToggle_.isOn = value;
            this.mondayToggle_.isOn = value;
            this.tuesdayToggle_.isOn = value;
            this.wednesdayToggle_.isOn = value;
            this.thursdayToggle_.isOn = value;
            this.fridayToggle_.isOn = value;
            this.saturdayToggle_.isOn = value;
        }
    }
    /// <summary>ステート配列 [日,月,火,水,木,金,土]</summary>
    public bool[] states {
        get {
            return new bool[7] {
                this.sumdayToggle_.isOn,
                this.mondayToggle_.isOn,
                this.tuesdayToggle_.isOn,
                this.wednesdayToggle_.isOn,
                this.thursdayToggle_.isOn,
                this.fridayToggle_.isOn,
                this.saturdayToggle_.isOn
            };
        }
        set {
            this.sumdayToggle_.isOn = value[0];
            this.mondayToggle_.isOn = value[1];
            this.tuesdayToggle_.isOn = value[2];
            this.wednesdayToggle_.isOn = value[3];
            this.thursdayToggle_.isOn = value[4];
            this.fridayToggle_.isOn = value[5];
            this.saturdayToggle_.isOn = value[6];
        }
    }


    // Start is called before the first frame update
    public void Start() {
        if(this.sumdayToggle_ == null) {
            Debug.LogError("Sumday toggle is null.");
        }
        if(this.mondayToggle_ == null) {
            Debug.LogError("Monday toggle is null.");
        }
        if(this.tuesdayToggle_ == null) {
            Debug.LogError("Tuesday toggle is null.");
        }
        if(this.wednesdayToggle_ == null) {
            Debug.LogError("Wednesday toggle is null.");
        }
        if(this.thursdayToggle_ == null) {
            Debug.LogError("Thursday toggle is null.");
        }
        if(this.fridayToggle_ == null) {
            Debug.LogError("Friday toggle is null.");
        }
        if(this.saturdayToggle_ == null) {
            Debug.LogError("Saturday toggle is null.");
        }
    }
    // Update is called once per frame
    public void Update() {
        
    }

    /// <summary>
    /// 休日ステートのトグル
    /// </summary>
    public void OnToggleHolidayState() {
        this.isActiveTheHoliday = !(this.isActiveTheHoliday);
    }
    /// <summary>
    /// 平日ステートのトグル
    /// </summary>
    public void OnToggleWorkdayState() {
        this.isActiveTheWorkday = !(this.isActiveTheWorkday);
    }
    /// <summary>
    /// 毎日ステートのトグル
    /// </summary>
    public void OnToggleEverydayState() {
        this.isActiveTheEveryday = !(this.isActiveTheEveryday);
    }
}
