using System;
using UnityEngine;

/// <summary>
/// アンドロイド接続用
/// </summary>
public class AndroidConnector : MonoBehaviour {
    /// <summary>UnityPlayerクラス</summary>
    private AndroidJavaClass unityPlayer_ = null;
    /// <summary>UnityPlayerクラス</summary>
    private AndroidJavaClass unityPlayer {
        get {
            if(this.unityPlayer_ == null) {
                this.unityPlayer_ = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                if(this.unityPlayer_ == null) {
                    Debug.LogError("Don't get android java class of UnityPlayer.");
                }
            }
            return this.unityPlayer_;
        }
    }
    /// <summary>アクティビティオブジェクト</summary>
    private AndroidJavaObject currentActivity_ = null;
    /// <summary>アクティビティオブジェクト</summary>
    private AndroidJavaObject currentActivity {
        get {
            if(this.currentActivity_ == null) {
                this.currentActivity_ = this.unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                if(this.currentActivity_ == null) {
                    Debug.LogError("Don't get android java object of current activity.");
                }
            }
            return this.currentActivity_;
        }
    }


    // Start is called before the first frame update
    private void Start() {
#if UNITY_ANDROID
        
#else
        Debug.LogError("Not build to android.");
#endif
    }

    /// <summary>
    /// アラームの設定
    /// </summary>
    /// <param name="_id">設定するアラームのID</param>
    /// <param name="_time">時間</param>
    /// <returns>true:成功 false:失敗</returns>
    public bool SetAlarm(int _id, DateTime _time) {
#if UNITY_ANDROID
        return this.currentActivity.Call<bool>(
            "addAlarm",
            _id,
            _time.Year, _time.Month, _time.Day,
            _time.Hour, _time.Minute, _time.Second
        );
#endif
    }
    /// <summary>
    /// アラームの削除
    /// </summary>
    /// <param name="_id"></param>
    public void KillAlarm(int _id) {
#if UNITY_ANDROID
        this.currentActivity.Call("killAlarm", _id);
#endif
    }
    
    /// <summary>
    /// アラーム開始
    /// </summary>
    public void OnStartAlarm() {
#if UNITY_ANDROID
        this.currentActivity.Call("onStartAlarm");
#endif
    }
    /// <summary>
    /// アラーム終了
    /// </summary>
    public void OnEndAlarm() {
#if UNITY_ANDROID
        this.currentActivity.Call("onEndAlarm");
#endif
    }


    /// <summary>
    /// アラーム終了
    /// </summary>
    public void Finish() {
#if UNITY_ANDROID
        this.currentActivity.Call("finishAndRemoveTask");
#endif
    }
}
