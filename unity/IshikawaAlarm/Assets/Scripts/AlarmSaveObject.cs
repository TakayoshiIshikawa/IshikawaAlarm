/// <summary>
/// アラーム記録用オブジェクト
/// </summary>
[System.Serializable]
public class AlarmSaveObject {
    /// <summary>ID</summary>
    [UnityEngine.SerializeField]
    private int id_ = -1;
    /// <summary>ID</summary>
    public int id {
        get { return this.id_; }
        set { this.id_ = value; }
    }
    /// <summary>タイトル</summary>
    [UnityEngine.SerializeField]
    private string title_ = "";
    /// <summary>タイトル</summary>
    public string title {
        get { return this.title_; }
        set { this.title_ = value; }
    }
    /// <summary>メッセージ</summary>
    [UnityEngine.SerializeField]
    private string message_ = "";
    /// <summary>メッセージ</summary>
    public string message {
        get { return this.message_; }
        set { this.message_ = value; }
    }
    /// <summary>時間 [年,月,日,時,分,秒]</summary>
    [UnityEngine.SerializeField]
    private int[] time_ = new int[6]{0,0,0,0,0,0};
    /// <summary>時間 [時,分,秒]</summary>
    public int[] time {
        get { return this.time_; }
    }
    /// <summary>有効化フラグ [日,月,火,水,木,金,土]</summary>
    [UnityEngine.SerializeField]
    private bool[] isActivateOnDayOfWeek_ = new bool[7] {
        false, false, false, false, false, false, false
    };
    /// <summary>有効化フラグ [日,月,火,水,木,金,土]</summary>
    public bool[] isActivateOnDayOfWeek {
        get { return this.isActivateOnDayOfWeek_; }
    }
    /// <summary>
    /// 有効か
    /// </summary>
    /// <returns>true:有効 false:無効</returns>
    public bool isActive {
        get {
            return (
                this.isActivateOnDayOfWeek[0] ||
                this.isActivateOnDayOfWeek[1] ||
                this.isActivateOnDayOfWeek[2] ||
                this.isActivateOnDayOfWeek[3] ||
                this.isActivateOnDayOfWeek[4] ||
                this.isActivateOnDayOfWeek[5] ||
                this.isActivateOnDayOfWeek[6]
            );
        }
    }


    /// <summary>
    /// 指定曜日が有効か
    /// </summary>
    /// <param name="_dayOfWeek">曜日</param>
    /// <returns>true:有効 false:無効</returns>
    public bool IsActivateOnDayOfWeek(System.DayOfWeek _dayOfWeek) {
        switch(_dayOfWeek) {
        case System.DayOfWeek.Sunday:
            return this.isActivateOnDayOfWeek_[0];

        case System.DayOfWeek.Monday:
            return this.isActivateOnDayOfWeek_[1];

        case System.DayOfWeek.Tuesday:
            return this.isActivateOnDayOfWeek_[2];

        case System.DayOfWeek.Wednesday:
            return this.isActivateOnDayOfWeek_[3];

        case System.DayOfWeek.Thursday:
            return this.isActivateOnDayOfWeek_[4];

        case System.DayOfWeek.Friday:
            return this.isActivateOnDayOfWeek_[5];

        case System.DayOfWeek.Saturday:
            return this.isActivateOnDayOfWeek_[6];
        }
        return false;
    }
}
