using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メモ入力フィールドコントローラ
/// </summary>
public class MemoInputFeildController : MonoBehaviour {
    [SerializeField, Tooltip("入力フィールド")]
    private InputField inputField_ = null;
    [SerializeField, Tooltip("最大行数")]
    private int maximumRowCount_ = 6;
    
    // Start is called before the first frame update
    public void Start() {
        if(this.inputField_ == null) {
            Debug.LogError("Input field is null.");
        }
    }

    /// <summary>
    /// 入力フィールドの変更
    /// </summary>
    public void OnChangeInputField() {
        string[] text = this.inputField_.text.Split('\n');
        if(text.Length > this.maximumRowCount_) {
            string newText = text[0];
            for(int i=1; i<this.maximumRowCount_; ++i) {
                newText += "\n" + text[i];
            }
            this.inputField_.text = newText;
        }
    }
}
