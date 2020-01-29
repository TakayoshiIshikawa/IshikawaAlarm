using System.Collections;
using UnityEngine;

/// <summary>
/// 設定メニューボタンコントローラ
/// </summary>
public class OptionMenuButtonController : MonoBehaviour {
    [SerializeField, Tooltip("設定メニューボタンイメージトランスフォーム")]
    private Transform optionMenuButtonImageTransform_ = null;
    [SerializeField, Tooltip("回転量")]
    private float rotate_ = -500.0f;
    [SerializeField, Tooltip("変更時間")]
    private float changeTime_ = 0.3f;
    [SerializeField, Tooltip("設定メニューオブジェクトトランスフォーム")]
    private RectTransform optionMenuObjectTransform_ = null;
    [SerializeField, Tooltip("設定メニューオブジェクトの非表示位置")]
    private Vector3 unviewPositionOfOptionMenuObject_ = new Vector3(-720.0f, 0.0f, 0.0f);
    [SerializeField, Tooltip("設定メニューオブジェクトの表示位置")]
    private Vector3 viewPositionOfOptionMenuObject_ = Vector3.zero;


    // Start is called before the first frame update
    public void Start() {
        if(this.optionMenuButtonImageTransform_ == null) {
            Debug.LogError("Option menu button image transform is null.");
        }
        if(this.optionMenuObjectTransform_ == null) {
            Debug.LogError("Option menu object transform is null.");
        }

        // 設定メニュー状態で戻るボタンが押されたら設定メニューを閉じる
        MainSceneManager.instance.SetBackAction(MainSceneManager.ViewState.OptionMenu, this.OnUnviewOptionObject);
    }


    /// <summary>オプションオブジェクトを表示する</summary>
    public void OnViewOptionObject() {
        StartCoroutine(this.ViewOptionObject());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Cancel).Play();
    }
    /// <summary>オプションオブジェクトを表示する</summary>
    private IEnumerator ViewOptionObject() {
        // 設定メニューの表示状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.OptionMenu;
        // オブジェクトをアクティブにする
        this.optionMenuObjectTransform_.gameObject.SetActive(true);

        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.optionMenuObjectTransform_.localPosition = Vector3.Lerp(
                this.unviewPositionOfOptionMenuObject_,
                this.viewPositionOfOptionMenuObject_,
                ratio
            );
            this.optionMenuButtonImageTransform_.eulerAngles = Vector3.Lerp(
                Vector3.zero,
                new Vector3(0.0f, 0.0f, this.rotate_),
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.optionMenuObjectTransform_.localPosition = this.viewPositionOfOptionMenuObject_;
        this.optionMenuButtonImageTransform_.eulerAngles = new Vector3(0.0f, 0.0f, this.rotate_);
    }

    /// <summary>オプションオブジェクトを非表示にする</summary>
    public void OnUnviewOptionObject() {
        StartCoroutine(this.UnviewOptionObject());
        // 音再生
        SoundEffectsManager.instance.GetSoundEffect(SoundEffectsManager.SoundEffectName.Cancel).Play();
    }
    /// <summary>オプションオブジェクトを非表示にする</summary>
    private IEnumerator UnviewOptionObject() {
        float time = 0.0f;
        while(time < this.changeTime_) {
            float r = time / this.changeTime_;
            float ratio = 3*r*r - 2*r*r*r;

            this.optionMenuObjectTransform_.localPosition = Vector3.Lerp(
                this.viewPositionOfOptionMenuObject_,
                this.unviewPositionOfOptionMenuObject_,
                ratio
            );
            this.optionMenuButtonImageTransform_.eulerAngles = Vector3.Lerp(
                new Vector3(0.0f, 0.0f, this.rotate_),
                Vector3.zero,
                ratio
            );

            yield return null;
            time += Time.deltaTime;
        }
        // 停止
        this.optionMenuObjectTransform_.localPosition = this.unviewPositionOfOptionMenuObject_;
        this.optionMenuButtonImageTransform_.eulerAngles = Vector3.zero;

        // オブジェクトを非アクティブにする
        this.optionMenuObjectTransform_.gameObject.SetActive(false);
        // メイン状態になる
        MainSceneManager.instance.viewState = MainSceneManager.ViewState.Main;
    }
}
