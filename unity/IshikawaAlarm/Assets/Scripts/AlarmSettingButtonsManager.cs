using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アラーム設定ボタンマネージャ
/// </summary>
public class AlarmSettingButtonsManager : MonoBehaviour {
    [SerializeField, Tooltip("変更時間")]
    private float changeTime_ = 0.3f;
    [SerializeField, Tooltip("アラーム設定オブジェクトコントローラ")]
    private AlarmSettingObjectController alarmSettingObjectController_ = null;
    [SerializeField, Tooltip("アラーム設定オブジェクトの表示位置")]
    private Vector3 viewPositionOfAlarmSettingObject_ = Vector3.zero;
    [SerializeField, Tooltip("アラーム設定オブジェクトの最小スケール")]
    private Vector3 minimumScaleOfAlarmSettingObject_ = new Vector3(0.3f, 0.0f, 1.0f);
    [Space]
    [SerializeField, Tooltip("有効日曜イメージカラー")]
    private Color sundayActiveColor_ = new Color32(255, 171,   0, 255);
    /// <summary>有効日曜イメージカラー</summary>
    public Color sundayActiveColor { get { return this.sundayActiveColor_; } }
    [SerializeField, Tooltip("無効日曜イメージカラー")]
    private Color sundayDeactiveColor_ = new Color32(192, 150,  64, 255);
    /// <summary>無効日曜イメージカラー</summary>
    public Color sundayDeactiveColor { get { return this.sundayDeactiveColor_; } }
    [SerializeField, Tooltip("有効月曜イメージカラー")]
    private Color mondayActiveColor_ = new Color32(255,   0, 255, 255);
    /// <summary>有効月曜イメージカラー</summary>
    public Color mondayActiveColor { get { return this.mondayActiveColor_; } }
    [SerializeField, Tooltip("無効月曜イメージカラー")]
    private Color mondayDeactiveColor_ = new Color32(192,  64, 192, 255);
    /// <summary>無効月曜イメージカラー</summary>
    public Color mondayDeactiveColor { get { return this.mondayDeactiveColor_; } }
    [SerializeField, Tooltip("有効火曜イメージカラー")]
    private Color tuesdayActiveColor_ = new Color32(255,   0,   0, 255);
    /// <summary>有効火曜イメージカラー</summary>
    public Color tuesdayActiveColor { get { return this.tuesdayActiveColor_; } }
    [SerializeField, Tooltip("無効火曜イメージカラー")]
    private Color tuesdayDeactiveColor_ = new Color32(192,  64,  64, 255);
    /// <summary>無効火曜イメージカラー</summary>
    public Color tuesdayDeactiveColor { get { return this.tuesdayDeactiveColor_; } }
    [SerializeField, Tooltip("有効水曜イメージカラー")]
    private Color wednesdayActiveColor_ = new Color32(  0, 200, 255, 255);
    /// <summary>有効水曜イメージカラー</summary>
    public Color wednesdayActiveColor { get { return this.wednesdayActiveColor_; } }
    [SerializeField, Tooltip("無効水曜イメージカラー")]
    private Color wednesdayDeactiveColor_ = new Color32( 64, 164, 192, 255);
    /// <summary>無効水曜イメージカラー</summary>
    public Color wednesdayDeactiveColor { get { return this.wednesdayDeactiveColor_; } }
    [SerializeField, Tooltip("有効木曜イメージカラー")]
    private Color thursdayActiveColor_ = new Color32(  0, 255,   0, 255);
    /// <summary>有効木曜イメージカラー</summary>
    public Color thursdayActiveColor { get { return this.thursdayActiveColor_; } }
    [SerializeField, Tooltip("無効木曜イメージカラー")]
    private Color thursdayDeactiveColor_ = new Color32( 64, 192,  64, 255);
    /// <summary>無効木曜イメージカラー</summary>
    public Color thursdayDeactiveColor { get { return this.thursdayDeactiveColor_; } }
    [SerializeField, Tooltip("有効金曜イメージカラー")]
    private Color fridayActiveColor_ = new Color32(  0, 255, 255, 255);
    /// <summary>有効金曜イメージカラー</summary>
    public Color fridayActiveColor { get { return this.fridayActiveColor_; } }
    [SerializeField, Tooltip("無効金曜イメージカラー")]
    private Color fridayDeactiveColor_ = new Color32( 64, 192, 192, 255);
    /// <summary>無効金曜イメージカラー</summary>
    public Color fridayDeactiveColor { get { return this.fridayDeactiveColor_; } }
    [SerializeField, Tooltip("有効土曜イメージカラー")]
    private Color saturdayActiveColor_ = new Color32(  0, 100, 255, 255);
    /// <summary>有効土曜イメージカラー</summary>
    public Color saturdayActiveColor { get { return this.saturdayActiveColor_; } }
    [SerializeField, Tooltip("無効土曜イメージカラー")]
    private Color saturdayDeactiveColor_ = new Color32(  64, 114, 192, 255);
    /// <summary>無効土曜イメージカラー</summary>
    public Color saturdayDeactiveColor { get { return this.saturdayDeactiveColor_; } }
    [SerializeField, Tooltip("ボタンリスト")]
    private List<AlarmSettingButtonController> buttonList_ = new List<AlarmSettingButtonController>();
    /// <summary>アラーム設定オブジェクトトランスフォーム</summary>
    private Transform alarmSettingObjectTransform_ = null;
    /// <summary>アラーム設定オブジェクトの非表示位置</summary>
    private Vector3 unviewPositionOfAlarmSettingObject_ = Vector3.zero;
    /// <summary>表示アラームインデックス</summary>
    private int viewAlarmIndex_ = -1;


    // Start is called before the first frame update
    void Start() {
        if(this.alarmSettingObjectController_ == null) {
            Debug.LogError("Alarm setting object controller is null.");
        }
        this.alarmSettingObjectTransform_ = this.alarmSettingObjectController_.gameObject.transform;

        // アラーム設定状態で戻るボタンが押されたらキャンセル
        MainSceneManager.instance.SetBackAction(MainSceneManager.ViewState.AlarmSettings, this.OnCanselAlarmSetting);
    }


    /// <summary>
    /// オプションオブジェクトを表示する
    /// </summary>
    /// <param name="_settingButtonController">アラーム設定ボタンコントローラ</param>
    public void OnViewAlarmSettingObject(
        AlarmSettingButtonController _settingButtonController
    ) {
        if(_settingButtonController != null) {
            this.viewAlarmIndex_ = _settingButtonController.viewAlarmIndex;
            this.unviewPositionOfAlarmSettingObject_ =
                this.gameObject.transform.localPosition +
                _settingButtonController.gameObject.transform.localPosition;
        }
        else {
            Debug.LogError("Setting button controller is null.");
        }
        StartCoroutine(this.ViewAlarmSettingObject());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Select).Play();
    }
    /// <summary>オプションオブジェクトを表示する</summary>
    private IEnumerator ViewAlarmSettingObject() {
        // アラーム設定状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.AlarmSettings;
        // オブジェクトをアクティブにする
        this.alarmSettingObjectTransform_.gameObject.SetActive(true);
        // 表示を更新
        this.alarmSettingObjectController_.setting = AlarmDataManager.instance.GetAlarm(this.viewAlarmIndex_);

        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.alarmSettingObjectTransform_.localPosition = Vector3.Lerp(
                this.unviewPositionOfAlarmSettingObject_,
                this.viewPositionOfAlarmSettingObject_,
                ratio
            );
            this.alarmSettingObjectTransform_.localScale = Vector3.Lerp(
                this.minimumScaleOfAlarmSettingObject_,
                Vector3.one,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.alarmSettingObjectTransform_.localPosition = this.viewPositionOfAlarmSettingObject_;
        this.alarmSettingObjectTransform_.localScale = Vector3.one;
    }
    
    /// <summary>設定を保存する</summary>
    public void OnSaveAlarmSetting() {
        if(this.alarmSettingObjectController_.isClosable) {
            // 音再生
            SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Select).Play();
            // 保存して閉じる
            AlarmDataManager.instance.SetAlarm(
                this.viewAlarmIndex_,
                this.alarmSettingObjectController_.setting
            );
            StartCoroutine(this.UnviewAlarmSettingObject());
        }
    }
    /// <summary>設定をキャンセルする</summary>
    public void OnCanselAlarmSetting() {
        if(this.alarmSettingObjectController_.isClosable) {
            // 音再生
            SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Cancel).Play();
            // そのまま閉じる
            StartCoroutine(this.UnviewAlarmSettingObject());
        }
    }
    /// <summary>オプションオブジェクトを非表示にする</summary>
    private IEnumerator UnviewAlarmSettingObject() {
        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.alarmSettingObjectTransform_.localPosition = Vector3.Lerp(
                this.viewPositionOfAlarmSettingObject_,
                this.unviewPositionOfAlarmSettingObject_,
                ratio
            );
            this.alarmSettingObjectTransform_.localScale = Vector3.Lerp(
                Vector3.one,
                this.minimumScaleOfAlarmSettingObject_,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.alarmSettingObjectTransform_.localPosition = this.unviewPositionOfAlarmSettingObject_;
        this.alarmSettingObjectTransform_.localScale = this.minimumScaleOfAlarmSettingObject_;

        // オブジェクトを非アクティブにする
        this.alarmSettingObjectTransform_.gameObject.SetActive(false);
        // アラームIDをリセット
        this.viewAlarmIndex_ = -1;
        // アラーム表示状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.AlarmsView;
    }


    /// <summary>
    /// 表示を更新
    /// </summary>
    public void OnUpdateAllButtonsView() {
        foreach(AlarmSettingButtonController button in this.buttonList_) {
            button.OnUpdateView();
        }
    }
}
