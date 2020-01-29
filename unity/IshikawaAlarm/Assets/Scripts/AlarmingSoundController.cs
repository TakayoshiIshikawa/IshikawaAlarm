using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アラーム音コントローラ
/// </summary>
public class AlarmingSoundController : MonoBehaviour　{
    /// <summary>
    /// アラーム開始
    /// </summary>
    public virtual void OnPlayAlarming() { }
    /// <summary>
    /// アラーム終了
    /// </summary>
    public virtual void OnStopAlarming() { }
}
