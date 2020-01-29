/**
 * 自作ログクラス.
 * @author Ishikawa Takayoshi
 */

package trident.ishikawatakayoshi.ishikawaalarm;

import android.util.Log;

/**
 * 自作ログクラス.
 */
public class ITLog {
    private enum Mode{
        All(0),
        Debug(1),
        Warning(2),
        Error(3);

        private int id_;

        private Mode(int _id){
            this.id_ = _id;
        }
    }
    private static final Mode mode = Mode.Warning;

    private String tag_;


    /**
     * コンストラクタ.
     * @param _tag タグ
     */
    public ITLog(String _tag){
        this.tag_ = _tag;
    }

    /**
     * タグの設定
     * @param _tag タグ
     */
    public void setTag(String _tag){
        this.tag_ = _tag;
    }

    /**
     * デバッグログ
     * @param _message メッセージ
     */
    public void debug(String _message){
        if(ITLog.mode.id_ <= Mode.Debug.id_) {
            Log.d(this.tag_, _message);
        }
    }
    /**
     * 警告ログ
     * @param _message メッセージ
     */
    public void warning(String _message){
        if(ITLog.mode.id_ <= Mode.Warning.id_) {
            Log.w(this.tag_, _message);
        }
    }
    /**
     * エラーログ
     * @param _message メッセージ
     */
    public void error(String _message){
        if(ITLog.mode.id_ <= Mode.Error.id_) {
            Log.e(this.tag_, _message);
        }
    }
}
