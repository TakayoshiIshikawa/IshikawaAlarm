using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話文ジェネレータ
/// </summary>
public class SpeakWordsGenerator : MonoBehaviour {
    [SerializeField, Tooltip("挨拶までの時間")]
    private float greetingWait_ = 3.0f;
    [SerializeField, Tooltip("最小生成間隔")]
    private float minInterval_ = 10.0f;
    /// <summary>最小生成間隔</summary>
    private float minInterval {
        get {
            const float min = 1.0f;
            return Mathf.Max(this.minInterval_, min);
        }
    }
    [SerializeField, Tooltip("最大生成間隔")]
    private float maxInterval_ = 60.0f;
    /// <summary>最大生成間隔</summary>
    private float maxInterval {
        get { return Mathf.Max(this.maxInterval_, this.minInterval + 0.1f); }
    }
    /// <summary>生成間隔</summary>
    private float interval {
        get {
            float rate = (
                UnityEngine.Random.Range(0.0f,1.0f) +
                UnityEngine.Random.Range(0.0f,1.0f) +
                UnityEngine.Random.Range(0.0f,1.0f)
                ) / 3.0f;
            return ((1.0f-rate)*this.minInterval + rate*this.maxInterval);
        }
    }
    [SerializeField, Tooltip("会話文マネージャ")]
    private SpeakWordsManager speakWordsManager_ = null;
    /// <summary>時間</summary>
    private float time_ = 0.0f;
    /// <summary>挨拶したかのフラグ</summary>
    private bool isGreeted_ = false;


    // Start is called before the first frame update
    public void Start() {
        if(this.speakWordsManager_ == null) {
            Debug.LogError("Speak lines manager is null.");
        }

        this.time_ = this.greetingWait_;
    }
    // Update is called once per frame
    public void Update() {
        if(MainSceneManager.instance.viewState == MainSceneManager.ViewState.Main) {
            this.time_ -= Time.deltaTime;
            if(this.time_ < 0.0f) {
                this.Create();
                this.time_ = this.interval;
            }
        }
    }

    /// <summary>
    /// メイン画面が表示された
    /// </summary>
    public void OnViewMain() {
        // 時間をリセット
        if(this.isGreeted_) {
            this.time_ = this.interval;
        }
        else {
            this.time_ = this.greetingWait_;
        }
    }

    /// <summary>
    /// 会話文の生成
    /// </summary>
    private void Create() {
        if(this.isGreeted_) {
            List<SpeakWordsManager.LinesData> linesList = this.CreateLinesList();
            if(linesList.Count > 0) {
                this.speakWordsManager_.AddLines(linesList[UnityEngine.Random.Range(0, linesList.Count)]);
            }
        }
        else {
            this.speakWordsManager_.AddLines(this.CreateGreet());
        }
    }
    /// <summary>
    /// 挨拶の生成
    /// </summary>
    private SpeakWordsManager.LinesData CreateGreet() {
        this.isGreeted_ = true;
        
        int hour = DateTime.Now.Hour;
        if(hour <= 3) {
            // 0 - 3
            return new SpeakWordsManager.LinesData(
                "ひより",
                "ふぁー......\nこんばんは......",
                "ふぁーーー。。こーんーばーんーはーーーー。。",
                -0.5f,
                0.5f,
                0.5f,
                0.0f, 0.0f
            );
        }
        else if(hour <= 5) {
            // 4 - 5
            return new SpeakWordsManager.LinesData(
                "ひより",
                "ぁー...\nおはようございます...\nわたし、まだ眠たいです...",
                "ぁーー。。おはようございますーー。。わたし、まだーねむたいですー。。。",
                -0.2f,
                0.5f,
                0.6f,
                0.0f, 0.0f
            );
        }
        else if(hour <= 9) {
            // 6 - 9
            return new SpeakWordsManager.LinesData(
                "ひより",
                "おはようございます！\n今日も一日頑張りましょう！！",
                "おはようございます。。きょうもいちにちがんばりましょう。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            );
        }
        else if(hour <= 17) {
            // 10 - 17
            return new SpeakWordsManager.LinesData(
                "ひより",
                "こんにちは！！",
                "こんにちは。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            );
        }
        else if(hour <= 22) {
            // 18 - 22
            return new SpeakWordsManager.LinesData(
                "ひより",
                "こんばんは！！",
                "こんばんは。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            );
        }
        // 23 - 24
        return new SpeakWordsManager.LinesData(
            "ひより",
            "こんばんは！",
            "こんばんは。。",
            -0.1f,
            0.0f,
            0.3f,
            0.0f, 0.0f
        );
    }
    /// <summary>
    /// 会話文リストの生成
    /// </summary>
    private List<SpeakWordsManager.LinesData> CreateLinesList() {
        List<SpeakWordsManager.LinesData> linesList = new List<SpeakWordsManager.LinesData>();

        int hour = DateTime.Now.Hour;
        //DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

        if(hour <= 3) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "どうしました？",
                "どうしました。。",
                0.0f,
                0.5f,
                0.5f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 4) && (hour <= 5)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "どうしました？",
                "どうしました。。",
                0.0f,
                0.5f,
                0.3f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 6) && (hour <= 21)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "どうしました？",
                "どうしました。。",
                0.0f,
                0.5f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 22) && (hour <= 23)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "どうしました？",
                "どうしました。。",
                0.0f,
                0.5f,
                0.2f,
                0.0f, 0.0f
            ));
        }
        if(hour == 0) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "日付変わってますよ？\n早く寝ましょ？",
                "ひづけかわってますよ。。はやくねましょ。。",
                -0.1f,
                0.8f,
                0.4f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 23) && (hour <= 3)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "夜更かしは体に悪いですよ？",
                "よふかしわからだにわるいですよ。。",
                -0.1f,
                0.9f,
                0.3f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 0) && (hour <= 3)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "そろそろ寝たほうがいいですよ？",
                "そろそろねたほうがいいですよ。。",
                -0.1f,
                0.8f,
                0.5f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 3) && (hour <= 5)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "私、寝たいです...",
                "わたし。。。ねたいです。。。",
                -0.1f,
                0.2f,
                0.7f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 5) && (hour <= 6)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "そろそろ日の出ですね。\n一日の始まりです！！",
                "そろそろひのでですね。。いちにちのはじまりです。。",
                0.5f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 6) && (hour <= 7)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "今日の天気はどうですか？",
                "きょうのてんきわどうですか。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 6) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "朝ごはんの準備しなきゃ！？",
                "あさごはんのじゅんびしなきゃ。。",
                -0.1f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 7) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "朝ごはん、何食べました？",
                "あさごはん、なにたべました。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 7) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "朝はごはん派です？それともパン派？",
                "あさわごはんはです。。それともぱんは。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "私はごはん派です！！",
                "あさわごはんはです。。。。",
                0.5f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "でも、パンは手軽なんですよね。",
                "でもぱんわてがるなんですよね。。",
                -0.1f,
                0.3f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 7) && (hour <= 9)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お仕事頑張ってください！！",
                "おしごとがんばってください。。",
                0.3f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 7) && (hour <= 9)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "道が混んでいないように\nお祈りしています！！",
                "みちが、こんでいないように。。おいのりしています。。",
                0.2f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 9) && (hour <= 10)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お仕事中ですか？",
                "おしごとちゅうですか。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 9) && (hour <= 11)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お仕事してる姿、素敵です！！",
                "おしごとしてるすがた、すてきです。。",
                0.5f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 11) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お昼まであと少しです！",
                "おひるまで、あとすこしです。。",
                0.2f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 12) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お昼、何食べました？",
                "おひる、なにたべました。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 12) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お昼休み......\nずっと続いて欲しいです......",
                "おひるやすみ。。。ずっとつづいてほしいです。。。。",
                -0.6f,
                0.3f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 13) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "午後からも\nまた頑張りましょう！！",
                "ごごからもまた、がんばりましょう。。",
                0.5f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 13) && (hour <= 17)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お仕事中ですか？",
                "おしごとちゅうですか。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 13) && (hour <= 17)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お仕事中ファイトです！！",
                "おしごとふぁいとです。。",
                0.5f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 15) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "15時です！！",
                "じゅうごじです。",
                0.3f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "至福のおやつタイムです！！",
                "しふくのおやつたいむです。。",
                0.7f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "今日は何食べよっかな～",
                "きょうわなにたべよっかなーー。",
                0.5f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 16) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "そろそろ17時ですね。",
                "そろそろじゅうしちじですね。。",
                0.1f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 17) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "晩ご飯の食料はありますか？",
                "ごゆうはんのしょくりょうわありますか。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 17) && (hour <= 18)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "夕暮れが近づいてきてますね...",
                "ゆうぐれがちかづいてきてますね。。",
                -0.2f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 18) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "晩ご飯の準備をしましょう！！",
                "ばんごはんのじゅんびをしましょう。。",
                0.2f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 19) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "晩ご飯はもう食べ終わりました？",
                "ばんごはんわもうたべおわりました。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 19) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お仕事、お疲れ様。",
                "おしごと、おつかれさま。。",
                0.2f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 19) && (hour <= 22)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "あの番組やってるかな？",
                "あのばんぐみやってるかな。。",
                0.0f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 20) && (hour <= 22)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "夜はゆっくりしましょ？",
                "よるはゆっくりしましょ。。",
                0.0f,
                0.4f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if(hour == 21) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "お風呂の用意しなきゃ！！",
                "おふろのよういしなきゃ。。",
                0.2f,
                0.0f,
                0.0f,
                0.0f, 0.0f
            ));
        }
        if((hour >= 22) && (hour <= 23)) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "そろそろ寝ましょう？",
                "そろそろねましょう。。",
                -0.1f,
                0.7f,
                0.1f,
                0.0f, 0.0f
            ));
        }
        if(hour == 23) {
            linesList.Add(new SpeakWordsManager.LinesData(
                "ひより",
                "日付変わっちゃいますよ？",
                "ひづけかわっちゃいますよ。。",
                -0.5f,
                0.9f,
                0.3f,
                0.0f, 0.0f
            ));
        }

        return linesList;
    }
}
