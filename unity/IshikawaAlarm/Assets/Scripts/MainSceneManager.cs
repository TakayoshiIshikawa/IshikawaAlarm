using System;
using UnityEngine;

/// <summary>
/// メインシーンマネージャ
/// </summary>
public class MainSceneManager : SingletonMonoBehaviour<MainSceneManager> {
    /// <summary>
    /// 表示状態
    /// </summary>
    public enum ViewState {
        /// <summary>メイン</summary>
        Main,
        /// <summary>設定メニュー</summary>
        OptionMenu,
        /// <summary>アラーム表示</summary>
        AlarmsView,
        /// <summary>設定</summary>
        Config,
        /// <summary>アラーム設定</summary>
        AlarmSettings,
        /// <summary>アラーム中</summary>
        Alarming
    }

    [SerializeField, Tooltip("アンドロイド接続用")]
    private AndroidConnector androidConnector_ = null;
    [SerializeField, Tooltip("会話文ジェネレータ")]
    private SpeakWordsGenerator speakWordsGenerator_ = null;
    /// <summary>表示状態</summary>
    private ViewState viewState_ = ViewState.Main;
    /// <summary>表示状態</summary>
    public ViewState viewState {
        get { return this.viewState_; }
        set {
            if(value == ViewState.Main) {
                this.speakWordsGenerator_.OnViewMain();
            }
            this.viewState_ = value;
        }
    }
    /// <summary>戻るボタン押下時アクション配列</summary>
    private readonly Action[] onBackActionArray_ = new Action[6];


    // Start is called before the first frame update
    public void Start() {
        if(this.androidConnector_ == null) {
            Debug.LogError("Android connector is null.");
        }
        if(this.speakWordsGenerator_ == null) {
            Debug.LogError("Speak words generator is null.");
        }
        
        // メイン状態で戻るボタンが押されたら終了
        this.SetBackAction(ViewState.Main, this.OnEndApplication);
    }
    // Update is called once per frame
    public void Update() {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            Action onBack = this.onBackActionArray_[(int)(this.viewState)];
            if(onBack != null) {
                onBack();
            }
        }
    }


    /// <summary>
    /// 戻るボタン押下時アクションの設定
    /// </summary>
    /// <param name="_viewState">表示状態</param>
    /// <param name="_action">設定アクション</param>
    public void SetBackAction(ViewState _viewState, Action _action) {
        this.onBackActionArray_[(int)_viewState] = _action;
    }


    /// <summary>
    /// アプリケーション終了
    /// </summary>
    public void OnEndApplication() {
        //this.androidConnector_.Finish();
        Application.Quit();
    }
}
