using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話文マネージャ
/// </summary>
public class SpeakWordsManager : MonoBehaviour {
    /// <summary>
    /// 台詞データ
    /// </summary>
    public struct LinesData {
        /// <summary>誰か</summary>
        public string who_;
        /// <summary>表示用データ</summary>
        public string view_;
        /// <summary>会話モーション用データ</summary>
        public string speakMotion_;
        /// <summary>幸福度</summary>
        public float happiness_;
        /// <summary>困惑度</summary>
        public float worry_;
        /// <summary>眠気</summary>
        public float sleepiness_;
        /// <summary>視線(横) [-1.0f, 0.0f, 1.0f]</summary>
        public float eyeLookX_;
        /// <summary>視線(縦) [-1.0f, 0.0f, 1.0f]</summary>
        public float eyeLookY_;

        /// <summary>
        /// フルコンストラクタ
        /// </summary>
        /// <param name="_who">誰か</param>
        /// <param name="_view">表示用データ</param>
        /// <param name="_speakMotion">会話モーション用データ</param>
        /// <param name="_happiness">幸福度</param>
        /// <param name="_worry">困惑度</param>
        /// <param name="_sleepiness">眠気</param>
        /// <param name="_eyeLookX">視線(横) [-1.0f, 0.0f, 1.0f]</param>
        /// <param name="_eyeLookY">視線(縦) [-1.0f, 0.0f, 1.0f]</param>
        public LinesData(
            string _who,
            string _view,
            string _speakMotion,
            float _happiness,
            float _worry,
            float _sleepiness,
            float _eyeLookX,
            float _eyeLookY
        ) {
            this.who_ = _who;
            this.view_ = _view;
            this.speakMotion_ = _speakMotion;
            this.happiness_ = _happiness;
            this.worry_ = _worry;
            this.sleepiness_ = _sleepiness;
            this.eyeLookX_ = _eyeLookX;
            this.eyeLookY_ = _eyeLookY;
        }
    }

    
    [SerializeField, Tooltip("台詞表示コントローラ")]
    private CharacterLinesController characterLinesController_ = null;
    [SerializeField, Tooltip("ひよりちゃんパラメータコントローラ")]
    private HiyoriParameterController hiyoriParameterController_ = null;
    [SerializeField, Tooltip("ひよりちゃんの目のコントローラ")]
    private HiyoriEyeController hiyoriEyeController_ = null;
    [SerializeField, Tooltip("ひよりちゃん会話モーションコントローラ")]
    private HiyoriSpeakMotionController hiyoriSpeakMotionController_ = null;
    [SerializeField, Tooltip("台詞を言い終わった後の余白時間")]
    private float speakAfterTime_ = 1.0f;
    /// <summary>台詞</summary>
    private List<LinesData> linesList_ = new List<LinesData>();
    /// <summary>台詞表示コルーチン</summary>
    private IEnumerator viewLines_ = null;


    // Start is called before the first frame update
    public void Start() {
        if(this.characterLinesController_ == null) {
            Debug.LogError("Character lines controller is null.");
        }
        if(this.hiyoriParameterController_ == null) {
            Debug.LogError("Hiyori parameter controller is null.");
        }
        if(this.hiyoriEyeController_ == null) {
            Debug.LogError("Hiyori eye controller is null.");
        }
        if(this.hiyoriSpeakMotionController_ == null) {
            Debug.LogError("Hiyori speak motion controller is null.");
        }
    }
    // Update is called once per frame
    public void Update() {
        if(this.viewLines_ == null) {
            if(this.linesList_.Count > 0) {
                this.characterLinesController_.gameObject.SetActive(true);
                this.viewLines_ = this.ViewLines(this.linesList_[0]);
            }
        }
        else{
            if(!(this.viewLines_.MoveNext())) {
                this.viewLines_ = null;
                this.linesList_.RemoveAt(0);
                // 台詞終了
                if(this.linesList_.Count <= 0) {
                    this.characterLinesController_.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 台詞の追加
    /// </summary>
    /// <param name="_lines">台詞</param>
    public void AddLines(LinesData _lines) {
        this.linesList_.Add(_lines);
    }

    /// <summary>
    /// 台詞表示
    /// </summary>
    private IEnumerator ViewLines(LinesData _lines) {
        // 情報入力
        this.characterLinesController_.title = _lines.who_;
        this.characterLinesController_.lines = _lines.view_;
        this.hiyoriParameterController_.toHappiness = _lines.happiness_;
        this.hiyoriParameterController_.toWorry = _lines.worry_;
        this.hiyoriParameterController_.toSleepiness = _lines.sleepiness_;
        this.hiyoriEyeController_.toLookX = _lines.eyeLookX_;
        this.hiyoriEyeController_.toLookY = _lines.eyeLookY_;
        this.hiyoriSpeakMotionController_.SetSpeak(_lines.speakMotion_);
        yield return null;
        // 終わるまで待つ
        while(this.hiyoriSpeakMotionController_.isSpeaking){
            yield return null;
        }
        // 台詞があったら少し待つ
        if(_lines.view_ != "") {
            yield return new WaitForSeconds(this.speakAfterTime_);
        }
    }
}
