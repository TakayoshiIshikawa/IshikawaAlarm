using System;
using UnityEngine;

/// <summary>
/// 時計表示コントローラ
/// </summary>
public abstract class ClockViewController : MonoBehaviour {
    /// <summary>表示時間</summary>
    public abstract DateTime time {
        get; set;
    }
}
