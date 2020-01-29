using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// キャラ台詞コントローラ
/// </summary>
public class CharacterLinesController : MonoBehaviour {
    [SerializeField, Tooltip("背景イメージ")]
    private Image backgroundImage_ = null;
    [SerializeField, Tooltip("背景色")]
    private Color backgroundColor_ = Color.white;
    /// <summary>背景色</summary>
    public Color backgroundColor {
        get { return this.backgroundColor_; }
        set {
            this.backgroundColor_ = value;
            this.backgroundImage_.color = value;
        }
    }
    [SerializeField, Tooltip("背景枠イメージ")]
    private Image backgroundEdgeImage_ = null;
    [SerializeField, Tooltip("背景枠色")]
    private Color backgroundEdgeColor_ = Color.black;
    /// <summary>背景枠色</summary>
    public Color backgroundEdgeColor {
        get { return this.backgroundEdgeColor_; }
        set {
            this.backgroundEdgeColor_ = value;
            this.backgroundEdgeImage_.color = value;
        }
    }
    [Space]
    [SerializeField, Tooltip("タイトルテキスト")]
    private Text titleText_ = null;
    [SerializeField, Tooltip("タイトル文字")]
    private string title_ = "";
    /// <summary>タイトル文字</summary>
    public string title {
        get { return this.title_; }
        set {
            this.title_ = value;
            this.titleText_.text = value;
        }
    }
    [SerializeField, Tooltip("台詞テキスト")]
    private Text linesText_ = null;
    [SerializeField, Tooltip("台詞文字"), Multiline(3)]
    private string lines_ = "";
    /// <summary>台詞文字</summary>
    public string lines {
        get { return this.lines_; }
        set {
            this.lines_ = value;
            this.linesText_.text = value;
        }
    }


    // Start is called before the first frame update
    void Start() {
        if(this.backgroundImage_ == null) {
            Debug.LogError("The background image is null.");
        }
        if(this.backgroundEdgeImage_ == null) {
            Debug.LogError("The background edge image is null.");
        }
        if(this.titleText_ == null) {
            Debug.LogError("The title text is null.");
        }
        if(this.linesText_ == null) {
            Debug.LogError("The lines text is null.");
        }

        this.backgroundImage_.color = this.backgroundColor_;
        this.backgroundEdgeImage_.color = this.backgroundEdgeColor_;
        this.titleText_.text = this.title_;
        this.linesText_.text = this.lines_;
    }
    // Update is called once per frame
    void Update() {
        
    }
}
