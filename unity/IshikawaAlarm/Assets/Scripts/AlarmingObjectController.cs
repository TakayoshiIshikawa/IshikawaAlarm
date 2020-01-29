using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アラーム中オブジェクトコントローラ
/// </summary>
public class AlarmingObjectController : MonoBehaviour {
    [SerializeField, Tooltip("タイトルテキスト")]
    private Text titleText_ = null;
    [SerializeField, Tooltip("メモテキスト")]
    private Text messageText_ = null;
    [SerializeField, Tooltip("アラーム音コントローラ")]
    private AlarmingSoundController alarmingSoundController_ = null;


    // Start is called before the first frame update
    public void Start() {
        if(this.titleText_ == null) {
            Debug.LogError("Title text is null.");
        }
        if(this.messageText_ == null) {
            Debug.LogError("Message text is null.");
        }
        if(this.alarmingSoundController_ == null) {
            Debug.LogError("Alarming sound controller is null.");
        }
    }


    public void OnChangeView(int _alarmIndex) {
        AlarmSaveObject data = AlarmDataManager.instance.GetAlarm(_alarmIndex);
        this.titleText_.text = data.title;
        this.messageText_.text = data.message;
    }

    /// <summary>
    /// アラーム開始
    /// </summary>
    public void OnStartAlarm() {
        this.alarmingSoundController_.OnPlayAlarming();
    }
    /// <summary>
    /// アラーム終了
    /// </summary>
    public void OnEndAlarm() {
        this.alarmingSoundController_.OnStopAlarming();
    }
}
