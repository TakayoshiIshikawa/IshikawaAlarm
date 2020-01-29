using System;
using UnityEngine;

/// <summary>
/// シングルトンモノビヘイビア
/// </summary>
/// <typeparam name="T">シングルトン化するクラス</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    /// <summary>インスタンス</summary>
    private static T instance_ = null;
    /// <summary>インスタンス</summary>
    public static T instance {
        get{
            if(instance_ == null) {
                Type t = typeof(T);

                instance_ = (T)FindObjectOfType (t);
                if(instance_ == null) {
                    Debug.LogError (t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance_;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    virtual protected void Awake(){
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        this.CheckInstance();
    }

    /// <summary>
    /// インスタンスをチェックする (既にある場合はこれを削除)
    /// </summary>
    /// <returns>
    /// true:インスタンスはこれだけ
    /// false:2つ以上のインスタンスを作成しようとした
    /// </returns>
    protected bool CheckInstance() {
        if(instance_ == null) {
            instance_ = this as T;
            return true;
        }
        else if(instance == this) {
            return true;
        }
        Destroy(this);
        return false;
    }
}
