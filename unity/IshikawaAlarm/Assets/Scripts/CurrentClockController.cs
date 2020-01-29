using System;
using UnityEngine;

/// <summary>
/// 現在時間コントローラ
/// </summary>
public class CurrentClockController : MonoBehaviour {
    /// <summary>時計表示コントローラ</summary>
    [SerializeField, Tooltip("時計表示コントローラ")]
    private ClockViewController clockViewController_ = null;


    // Start is called before the first frame update
    void Start() {
        if(this.clockViewController_ == null) {
            Debug.LogError("Clock view controller is null.");
        }
    }
    // Update is called once per frame
    void Update() {
        this.clockViewController_.time = DateTime.Now;
    }
}
