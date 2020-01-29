using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 数値カウンタコントローラ
/// </summary>
public class NumberCounterController :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField, Tooltip("コンテンツトランスフォーム")]
    private Transform contentTransform_ = null;
    [SerializeField, Tooltip("上数値テキスト")]
    private Text topNumberText_ = null;
    [SerializeField, Tooltip("中央数値テキスト")]
    private Text middleNumberText_ = null;
    [SerializeField, Tooltip("下数値テキスト")]
    private Text bottomNumberText_ = null;
    [SerializeField, Tooltip("上位カウンタ")]
    private NumberCounterController upperCounter_ = null;
    [Space]
    [SerializeField, Tooltip("変更音")]
    private AudioSource changeSound_ = null;
    [Space]
    [SerializeField, Tooltip("文字変換書式")]
    private string toStringFormat_ = "00";
    [SerializeField, Tooltip("最小数値")]
    private int minimumNumber_ = 0;
    [SerializeField, Tooltip("最大数値")]
    private int maximumNumber_ = 59;
    [Space]
    [SerializeField, Tooltip("移動距離")]
    private float moveDistance_ = 200.0f;
    [SerializeField, Tooltip("移動比率")]
    private float moveRatio_ = 0.03f;
    [SerializeField, Tooltip("回転入力の時間制限")]
    private float rotateTimeLimit_ = 0.3f;
    [SerializeField, Tooltip("回転比率")]
    private float rotateRatio_ = 0.02f;
    [SerializeField, Tooltip("摩擦")]
    private float friction_ = 0.98f;
    [SerializeField, Tooltip("停止制限")]
    private float stopLimit_ = 0.1f;
    [Space]
    [SerializeField, Tooltip("回転時間")]
    private float rotateTime_ = 0.3f;
    /// <summary>数値</summary>
    private float number_ = 0.0f;
    /// <summary>数値</summary>
    private float number {
        get { return this.number_; }
        set {
            float next = this.ConvertToNumberInLimit(value);
            if(Mathf.RoundToInt(next) != this.intNumber) {
                // 値が変わったら音を鳴らす
                if(this.changeSound_.isPlaying) {
                    this.changeSound_.Stop();
                }
                this.changeSound_.Play();
            }

            this.number_ = next;
            if(this.upperCounter_ != null) {
                int to = Mathf.RoundToInt(value);
                // 上位カウンタを変更する
                if(to < this.minimumNumber_) {
                    this.upperCounter_.OnRotateCount(-1);
                }
                else if(to > this.maximumNumber_) {
                    this.upperCounter_.OnRotateCount(1);
                }
            }

            // 回転
            int middle = (int)this.ConvertToNumberInLimit(Mathf.Round(this.number_));
            int top = (int)this.ConvertToNumberInLimit(middle - 1);
            int bottom = (int)this.ConvertToNumberInLimit(middle + 1);
            this.numberDelta_ = this.number_ - middle;
            float rate = this.numberDelta_*this.numberDelta_*this.numberDelta_ + 0.75f*this.numberDelta_;
            float y = this.moveDistance_ * rate;
            this.topNumberText_.text = top.ToString(this.toStringFormat_);
            this.middleNumberText_.text = middle.ToString(this.toStringFormat_);
            this.bottomNumberText_.text = bottom.ToString(this.toStringFormat_);
            this.contentTransform_.localPosition = new Vector3(0.0f, y, 0.0f);
        }
    }
    /// <summary>数値(int型)</summary>
    public int intNumber {
        get { return Mathf.RoundToInt(this.number); }
        set { this.number = this.ConvertToNumberInLimit(value); }
    }
    /// <summary>数値の差分</summary>
    private float numberDelta_ = 0.0f;
    /// <summary>ドラッグされているか</summary>
    private bool isDraging_ = false;
    /// <summary>回転しているか</summary>
    private bool isRotate_ = false;
    /// <summary>回転前の数値</summary>
    private float beforeRotateNumber_ = 0.0f;
    /// <summary>押された時間</summary>
    private float pressTime_ = 0.0f;
    /// <summary>押されたY座標</summary>
    private float pressPositionY_ = 0.0f;
    /// <summary>直前のY座標</summary>
    private float beforePositionY_ = 0.0f;
    /// <summary>回転速度</summary>
    private float rotateSpeed_ = 0.0f;
    /// <summary>回転コルーチン</summary>
    private Coroutine rotateCoroutine_ = null;
    /// <summary>動いているか</summary>
    public bool isMoving {
        get { return (this.isDraging_ || this.isRotate_ || (this.rotateCoroutine_ != null)); }
    }


    // Start is called before the first frame update
    public void Start() {
        if(this.contentTransform_ == null) {
            Debug.LogError("Content transform is null.");
        }
        if(this.topNumberText_ == null) {
            Debug.LogError("Top number text is null.");
        }
        if(this.middleNumberText_ == null) {
            Debug.LogError("Middle number text is null.");
        }
        if(this.bottomNumberText_ == null) {
            Debug.LogError("Bottom number text is null.");
        }
        if(this.minimumNumber_ >= this.maximumNumber_) {
            Debug.LogError("min >= max.");
        }
        if(this.changeSound_ == null) {
            Debug.LogError("Change sound is null.");
        }
    }
    public void Update() {
        if(this.isRotate_) {
            // 回転
            this.number = this.number + this.rotateSpeed_;
            
            // 遅くなったら停止
            if(Mathf.Abs(this.rotateSpeed_) <= this.stopLimit_) {
                // 数字の間のところでは止まらない
                if(Mathf.Abs(this.numberDelta_) < 0.05f) {
                    this.StopCounter();
                }
                else {
                    this.rotateSpeed_ = (this.rotateSpeed_ < 0.0f) ? (-(this.stopLimit_)) : (this.stopLimit_);
                }
            }
            else {
                // 摩擦
                this.rotateSpeed_ *= this.friction_;
            }
        }
    }

    /// <summary>
    /// 押された
    /// </summary>
    public void OnBeginDrag(PointerEventData _data) {
        this.isDraging_ = true;
        this.pressTime_ = Time.time;
        this.pressPositionY_ = _data.position.y;
        this.beforePositionY_ = _data.position.y;
        this.beforeRotateNumber_ = this.number;
    }
    /// <summary>
    /// 押されている
    /// </summary>
    public void OnDrag(PointerEventData _data) {
        if(this.rotateCoroutine_ == null) {
            // 止まってるとき
            float delta = (_data.position.y - this.beforePositionY_) * this.moveRatio_;
            this.number = this.number + delta;
            this.beforePositionY_ = _data.position.y;
        }
        else {
            // ボタンで動いてる最中は値を更新
            this.pressPositionY_ = _data.position.y;
            this.beforePositionY_ = _data.position.y;
            this.beforeRotateNumber_ = this.number;
        }
    }
    /// <summary>
    /// 離された
    /// </summary>
    public void OnEndDrag(PointerEventData _data) {
        // ボタンで動いてる最中は無視
        if(this.rotateCoroutine_ == null) {
            float deltaTime = Time.time - this.pressTime_;
            if(deltaTime < this.rotateTimeLimit_) {
                // 回転
                this.isRotate_ = true;
                float deltaMove = (_data.position.y - this.pressPositionY_) * this.moveRatio_;
                this.rotateSpeed_ = deltaMove * this.rotateRatio_;
            }
            else {
                // 停止
                this.StopCounter();
            }
        }
        this.isDraging_ = false;
    }

    /// <summary>
    /// カウンタの停止
    /// </summary>
    private void StopCounter() {
        this.number = Mathf.Round(this.number);
        this.isRotate_ = false;
        this.rotateSpeed_ = 0.0f;
        this.contentTransform_.localPosition = Vector3.zero;
    }
    
    /// <summary>
    /// 制限内の数値に変換する
    /// </summary>
    /// <param name="_number">数値</param>
    /// <returns>制限内の数値</returns>
    private float ConvertToNumberInLimit(float _number) {
        float roundNumber = Mathf.Round(_number);
        if(roundNumber < this.minimumNumber_) {
            do {
                _number += this.maximumNumber_ - this.minimumNumber_ + 1.0f;
                roundNumber = Mathf.Round(_number);
            } while(roundNumber < this.minimumNumber_);
        }
        else if(roundNumber >= (this.maximumNumber_ + 1.0f)) {
            do {
                _number -= this.maximumNumber_ - this.minimumNumber_ + 1.0f;
                roundNumber = Mathf.Round(_number);
            } while(roundNumber >= (this.maximumNumber_ + 1.0f));
        }
        return _number;
    }


    /// <summary>
    /// カウントを変更する
    /// </summary>
    /// <param name="_value">増減値</param>
    public void OnRotateCount(int _value) {
        // 動いてる最中は無視
        if((this.rotateCoroutine_ == null) && (!(this.isRotate_))) {
            this.rotateCoroutine_ = StartCoroutine(this.RotateCount(_value));
        }
    }
    /// <summary>
    /// カウントを変更する
    /// </summary>
    /// <param name="_value">増減値</param>
    private IEnumerator RotateCount(int _value) {
        int to = this.intNumber + _value;
        if(this.upperCounter_ != null) {
            // 上位カウンタを変更する
            if(to < this.minimumNumber_) {
                this.upperCounter_.OnRotateCount(-1);
            }
            else if(to > this.maximumNumber_) {
                this.upperCounter_.OnRotateCount(1);
            }
        }
        int toNumber = (int)this.ConvertToNumberInLimit(to);

        this.topNumberText_.text = toNumber.ToString(this.toStringFormat_);
        this.middleNumberText_.text = this.intNumber.ToString(this.toStringFormat_);
        this.bottomNumberText_.text = toNumber.ToString(this.toStringFormat_);

        float sign = (_value < 0) ? (-1.0f) : (1.0f);

        float time = 0.0f;
        while(time < this.rotateTime_) {
            float r = time / this.rotateTime_;
            float rate = 3*r*r - 2*r*r*r;
            float y = sign * this.moveDistance_ * rate;
            this.contentTransform_.localPosition = new Vector3(0.0f, y, 0.0f);

            yield return null;
            time += Time.deltaTime;
        }
        
        this.number = toNumber;
        this.rotateCoroutine_ = null;
    }
}
