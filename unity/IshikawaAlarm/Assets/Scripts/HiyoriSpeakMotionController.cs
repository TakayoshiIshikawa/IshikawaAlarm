using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ひよりちゃん会話モーションコントローラ
/// </summary>
public class HiyoriSpeakMotionController : MonoBehaviour {
    /// <summary>
    /// 口形名
    /// </summary>
    private enum MouthFormName {
        // == 長音 ==
        /// <summary>ん</summary>
        NN = 0,
        /// <summary>あ</summary>
        A,
        /// <summary>い</summary>
        I,
        /// <summary>う</summary>
        U,
        /// <summary>え</summary>
        E,
        /// <summary>お</summary>
        O,
        // == 短音 ==
        /// <summary>か行の始め</summary>
        K,
        /// <summary>さ行の始め</summary>
        S,
        /// <summary>た行の始め</summary>
        T,
        /// <summary>な行の始め</summary>
        N,
        /// <summary>は行の始め</summary>
        H,
        /// <summary>ま行の始め</summary>
        M,
        /// <summary>や行の始め</summary>
        Y,
        /// <summary>ら行の始め</summary>
        R,
        /// <summary>わ行の始め</summary>
        W,
        /// <summary>が行の始め</summary>
        G,
        /// <summary>ざ行の始め</summary>
        Z,
        /// <summary>だ行の始め</summary>
        D,
        /// <summary>ば行の始め</summary>
        B,
        /// <summary>ぱ行の始め</summary>
        P
    }

    
    /// <summary>設定値</summary>
    private HiyoriMouthController.SettingValues settingValues_ = new HiyoriMouthController.SettingValues();
    /// <summary>設定値</summary>
    public HiyoriMouthController.SettingValues settingValues {
        get { return this.settingValues_; }
    }

    [SerializeField, Tooltip("長音発声時間")]
    private float speakInterval_ = 0.04f;
    /// <summary>長音発声時間</summary>
    private float speakInterval {
        get {
            const float min = 0.01f;
            return Mathf.Max(this.speakInterval_, min);
        }
    }
    [SerializeField, Tooltip("口形リスト")]
    private List<Vector2> mouthFormList_ = new List<Vector2>();
    /// <summary>現在の口形</summary>
    private MouthFormName currentMouthForm_ = MouthFormName.NN;
    /// <summary>次の口形</summary>
    private MouthFormName nextMouthForm_ = MouthFormName.NN;
    /// <summary>口形変更用コルーチン</summary>
    private IEnumerator changeMouthForm_ = null;
    /// <summary>会話口形リスト</summary>
    private List<MouthFormName> speakingList_ = new List<MouthFormName>();
    /// <summary>話しているかのフラグ</summary>
    public bool isSpeaking {
        get { return ((this.speakingList_.Count > 0) || (this.changeMouthForm_ != null)); }
    }


    // Start is called before the first frame update
    public void Start() {
        Vector2 form = this.GetMouthForm(this.currentMouthForm_);
        this.settingValues_.form_ = form.x;
        this.settingValues_.openY_ = form.y;
    }
    // Update is called once per frame
    public void Update() {
        if(this.changeMouthForm_ != null) {
            // 口形変更中
            if(!(this.changeMouthForm_.MoveNext())) {
                // 変更終了
                this.currentMouthForm_ = this.nextMouthForm_;
                this.changeMouthForm_ = null;
            }
        }
        else if(this.speakingList_.Count > 0){
            // 次の音があるとき
            this.nextMouthForm_ = this.speakingList_[0];
            this.speakingList_.RemoveAt(0);
            this.changeMouthForm_ = ChangeMouthForm(this.currentMouthForm_, this.nextMouthForm_);
        }
    }
    
    /// <summary>
    /// 口形情報の取得
    /// </summary>
    /// <param name="_name">口形名</param>
    /// <returns>口形情報</returns>
    private Vector2 GetMouthForm(MouthFormName _name) {
        return this.mouthFormList_[(int)_name];
    }

    /// <summary>
    /// 会話文の設定
    /// </summary>
    /// <param name="_words">会話文</param>
    /// <param name="_isAdd">追加するか</param>
    public void SetSpeak(string _words, bool _isAdd = true) {
        if(_isAdd) {
            this.speakingList_.AddRange(this.ConvertStringToMouthFormList(_words));
        }
        else {
            this.speakingList_ = this.ConvertStringToMouthFormList(_words);
        }
    }

    /// <summary>
    /// 会話
    /// </summary>
    private IEnumerator ChangeMouthForm(MouthFormName _beforeFormName, MouthFormName _nextFormName) {
        float time = 0.0f;
        // 短音の場合に加速
        float timeRate = (_nextFormName > MouthFormName.O) ? (2.0f) : (1.0f);

        Vector2 beforeForm = this.GetMouthForm(_beforeFormName);
        Vector2 nextForm = this.GetMouthForm(_nextFormName);

        while(time < this.speakInterval) {
            float r = time / this.speakInterval;
            float rate = 3*r*r - 2*r*r*r;
            
            this.settingValues_.form_ = (1.0f - rate)*beforeForm.x + rate*nextForm.x;
            this.settingValues_.openY_ = (1.0f - rate)*beforeForm.y + rate*nextForm.y;

            yield return null;
            time += timeRate * Time.deltaTime;
        }
        // 最終形にする
        this.settingValues_.form_ = nextForm.x;
        this.settingValues_.openY_ = nextForm.y;
    }

    /// <summary>
    /// 文字列を口形データに変換する
    /// </summary>
    /// <param name="_string">文字列</param>
    /// <returns>口形データ</returns>
    private List<MouthFormName> ConvertStringToMouthFormList(string _string) {
        List<MouthFormName> list = new List<MouthFormName>();
        foreach(char c in _string) {
            // 前処理
            switch(c) {
            case 'ゃ':
            case 'ゅ':
            case 'ょ':
                // 直前の長音を削除
                list.RemoveAt(list.Count - 1);
                break;
            }

            // 変換
            list.AddRange(this.ConvertCharacterToMouthFormList(c));

            // 後処理
            switch(c) {
            case 'ー':
            case '～':
                {
                    // NNを削除
                    list.RemoveAt(list.Count - 1);
                    // 直前の長音を複製
                    list.Add(list[list.Count - 1]);
                }
                break;
            }
        }
        return list;
    }
    /// <summary>
    /// 文字列口形データに変換する
    /// </summary>
    /// <param name="_character">文字</param>
    /// <returns>口形データ</returns>
    private MouthFormName[] ConvertCharacterToMouthFormList(char _character) {
        switch(_character) {
        case 'あ': return new MouthFormName[1] { MouthFormName.A };
        case 'い': return new MouthFormName[1] { MouthFormName.I };
        case 'う': return new MouthFormName[1] { MouthFormName.U };
        case 'え': return new MouthFormName[1] { MouthFormName.E };
        case 'お': return new MouthFormName[1] { MouthFormName.O };
        case 'か': return new MouthFormName[2] { MouthFormName.K, MouthFormName.A };
        case 'き': return new MouthFormName[2] { MouthFormName.K, MouthFormName.I };
        case 'く': return new MouthFormName[2] { MouthFormName.K, MouthFormName.U };
        case 'け': return new MouthFormName[2] { MouthFormName.K, MouthFormName.E };
        case 'こ': return new MouthFormName[2] { MouthFormName.K, MouthFormName.O };
        case 'さ': return new MouthFormName[2] { MouthFormName.S, MouthFormName.A };
        case 'し': return new MouthFormName[2] { MouthFormName.S, MouthFormName.I };
        case 'す': return new MouthFormName[2] { MouthFormName.S, MouthFormName.U };
        case 'せ': return new MouthFormName[2] { MouthFormName.S, MouthFormName.E };
        case 'そ': return new MouthFormName[2] { MouthFormName.S, MouthFormName.O };
        case 'た': return new MouthFormName[2] { MouthFormName.T, MouthFormName.A };
        case 'ち': return new MouthFormName[2] { MouthFormName.T, MouthFormName.I };
        case 'つ': return new MouthFormName[2] { MouthFormName.T, MouthFormName.U };
        case 'て': return new MouthFormName[2] { MouthFormName.T, MouthFormName.E };
        case 'と': return new MouthFormName[2] { MouthFormName.T, MouthFormName.O };
        case 'な': return new MouthFormName[2] { MouthFormName.N, MouthFormName.A };
        case 'に': return new MouthFormName[2] { MouthFormName.N, MouthFormName.I };
        case 'ぬ': return new MouthFormName[2] { MouthFormName.N, MouthFormName.U };
        case 'ね': return new MouthFormName[2] { MouthFormName.N, MouthFormName.E };
        case 'の': return new MouthFormName[2] { MouthFormName.N, MouthFormName.O };
        case 'は': return new MouthFormName[2] { MouthFormName.H, MouthFormName.A };
        case 'ひ': return new MouthFormName[2] { MouthFormName.H, MouthFormName.I };
        case 'ふ': return new MouthFormName[2] { MouthFormName.H, MouthFormName.U };
        case 'へ': return new MouthFormName[2] { MouthFormName.H, MouthFormName.E };
        case 'ほ': return new MouthFormName[2] { MouthFormName.H, MouthFormName.O };
        case 'ま': return new MouthFormName[2] { MouthFormName.M, MouthFormName.A };
        case 'み': return new MouthFormName[2] { MouthFormName.M, MouthFormName.I };
        case 'む': return new MouthFormName[2] { MouthFormName.M, MouthFormName.U };
        case 'め': return new MouthFormName[2] { MouthFormName.M, MouthFormName.E };
        case 'も': return new MouthFormName[2] { MouthFormName.M, MouthFormName.O };
        case 'や': return new MouthFormName[2] { MouthFormName.Y, MouthFormName.A };
        case 'ゆ': return new MouthFormName[2] { MouthFormName.Y, MouthFormName.U };
        case 'よ': return new MouthFormName[2] { MouthFormName.Y, MouthFormName.O };
        case 'ら': return new MouthFormName[2] { MouthFormName.R, MouthFormName.A };
        case 'り': return new MouthFormName[2] { MouthFormName.R, MouthFormName.I };
        case 'る': return new MouthFormName[2] { MouthFormName.R, MouthFormName.U };
        case 'れ': return new MouthFormName[2] { MouthFormName.R, MouthFormName.E };
        case 'ろ': return new MouthFormName[2] { MouthFormName.R, MouthFormName.O };
        case 'わ': return new MouthFormName[2] { MouthFormName.W, MouthFormName.A };
        case 'を': return new MouthFormName[2] { MouthFormName.W, MouthFormName.O };
        case 'ん': return new MouthFormName[1] { MouthFormName.NN };
        case 'が': return new MouthFormName[2] { MouthFormName.G, MouthFormName.A };
        case 'ぎ': return new MouthFormName[2] { MouthFormName.G, MouthFormName.I };
        case 'ぐ': return new MouthFormName[2] { MouthFormName.G, MouthFormName.U };
        case 'げ': return new MouthFormName[2] { MouthFormName.G, MouthFormName.E };
        case 'ご': return new MouthFormName[2] { MouthFormName.G, MouthFormName.O };
        case 'ざ': return new MouthFormName[2] { MouthFormName.Z, MouthFormName.A };
        case 'じ': return new MouthFormName[2] { MouthFormName.Z, MouthFormName.I };
        case 'ず': return new MouthFormName[2] { MouthFormName.Z, MouthFormName.U };
        case 'ぜ': return new MouthFormName[2] { MouthFormName.Z, MouthFormName.E };
        case 'ぞ': return new MouthFormName[2] { MouthFormName.Z, MouthFormName.O };
        case 'だ': return new MouthFormName[2] { MouthFormName.D, MouthFormName.A };
        case 'ぢ': return new MouthFormName[2] { MouthFormName.D, MouthFormName.I };
        case 'づ': return new MouthFormName[2] { MouthFormName.D, MouthFormName.U };
        case 'で': return new MouthFormName[2] { MouthFormName.D, MouthFormName.E };
        case 'ど': return new MouthFormName[2] { MouthFormName.D, MouthFormName.O };
        case 'ば': return new MouthFormName[2] { MouthFormName.B, MouthFormName.A };
        case 'び': return new MouthFormName[2] { MouthFormName.B, MouthFormName.I };
        case 'ぶ': return new MouthFormName[2] { MouthFormName.B, MouthFormName.U };
        case 'べ': return new MouthFormName[2] { MouthFormName.B, MouthFormName.E };
        case 'ぼ': return new MouthFormName[2] { MouthFormName.B, MouthFormName.O };
        case 'ぱ': return new MouthFormName[2] { MouthFormName.P, MouthFormName.A };
        case 'ぴ': return new MouthFormName[2] { MouthFormName.P, MouthFormName.I };
        case 'ぷ': return new MouthFormName[2] { MouthFormName.P, MouthFormName.U };
        case 'ぺ': return new MouthFormName[2] { MouthFormName.P, MouthFormName.E };
        case 'ぽ': return new MouthFormName[2] { MouthFormName.P, MouthFormName.O };
        case 'ぁ': return new MouthFormName[1] { MouthFormName.A };
        case 'ぃ': return new MouthFormName[1] { MouthFormName.I };
        case 'ぅ': return new MouthFormName[1] { MouthFormName.U };
        case 'ぇ': return new MouthFormName[1] { MouthFormName.E };
        case 'ぉ': return new MouthFormName[1] { MouthFormName.O };
        case 'っ': return new MouthFormName[1] { MouthFormName.T };
        case 'ゃ': return new MouthFormName[2] { MouthFormName.Y, MouthFormName.A };
        case 'ゅ': return new MouthFormName[2] { MouthFormName.Y, MouthFormName.U };
        case 'ょ': return new MouthFormName[2] { MouthFormName.Y, MouthFormName.O };
        case '、': return new MouthFormName[2] { MouthFormName.NN, MouthFormName.NN };
        case '。': return new MouthFormName[4] { MouthFormName.NN, MouthFormName.NN, MouthFormName.NN, MouthFormName.NN };
        }
        return new MouthFormName[1] { MouthFormName.NN };
    }
}
