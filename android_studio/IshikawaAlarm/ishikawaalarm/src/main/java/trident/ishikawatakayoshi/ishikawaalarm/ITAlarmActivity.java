/**
 * 自作アラームクラス.
 * @author Ishikawa Takayoshi
 */

package trident.ishikawatakayoshi.ishikawaalarm;

import android.app.AlarmManager;
import android.app.KeyguardManager;
import android.app.PendingIntent;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.media.AudioManager;
import android.os.Build;
import android.os.Bundle;
import android.os.IBinder;
import android.os.PowerManager;
import android.util.JsonReader;
import android.view.Window;
import android.view.WindowManager;

import com.unity3d.player.UnityPlayerActivity;

import java.util.Calendar;
import java.util.HashMap;


/**
 * 自作アラームアクティビティ.
 */
public class ITAlarmActivity extends UnityPlayerActivity{
    private static final int ALARM_STREAM_ID = AudioManager.STREAM_MUSIC;
    private final ITLog itLog_ = new ITLog("ITAlarmActivity");
    private AlarmManager alarmManager_ = null;
    private AudioManager audioManager_ = null;
    private boolean isAlarming_ = false;
    private int beforeVolume_ = 0;


    @Override
    public void onCreate(Bundle _bundle){
        super.onCreate(_bundle);

        this.alarmManager_ = (AlarmManager) this.getSystemService(Context.ALARM_SERVICE);
        this.audioManager_ = (AudioManager) this.getSystemService(Context.AUDIO_SERVICE);

        this.itLog_.debug("create");
    }
    @Override
    public void onDestroy(){
        super.onDestroy();

        this.itLog_.debug("destroy");
    }

    @Override
    public void onResume(){
        super.onResume();

        // アラーム中なら音量を最大にする
        if(this.isAlarming_) {
            this.setMaxVolume();
        }

        this.itLog_.debug("resume");
    }
    @Override
    public void onPause(){
        // アラーム中なら音量を戻す
        if(this.isAlarming_) {
            this.resetVolume();
        }

        super.onPause();

        this.itLog_.debug("pause");
    }


    /**
     * アラームの追加.
     * @param _id ID
     * @param _year 年
     * @param _month 月
     * @param _date 日
     * @param _hour 時
     * @param _minute 分
     * @param _second 秒
     * @return true:追加した false:追加できなかった
     */
    public boolean addAlarm(
            int _id,
            int _year, int _month, int _date,
            int _hour, int _minute, int _second
    ) {
        this.itLog_.debug("call add alarm. [id:" + _id + "]");
        this.itLog_.debug(_year + "/" + _month + "/" + _date + " " + _hour + ":" + _minute + "." + _second);

        // アラームの時間を設定
        Calendar calendar = Calendar.getInstance();
        calendar.set(_year, _month - 1, _date, _hour, _minute, _second);
        long time = calendar.getTimeInMillis();
        if (time <= System.currentTimeMillis()) {
            this.itLog_.error("過去へのアラームです。");
            return false;
        }

        // ペンディングインテント作成
        PendingIntent pendingIntent = this.getPendingIntent(_id, PendingIntent.FLAG_UPDATE_CURRENT);

        // アラーム設定
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP){
            this.alarmManager_.setAlarmClock(new AlarmManager.AlarmClockInfo(time, null), pendingIntent);
        }
        else if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT){
            this.alarmManager_.setExact(AlarmManager.RTC_WAKEUP, time, pendingIntent);
        }
        else{
            this.alarmManager_.set(AlarmManager.RTC_WAKEUP, time, pendingIntent);
        }

        this.itLog_.debug("アラーム設定完了 [id:" + _id + "]");
        return true;
    }
    /**
     * アラームの削除.
     * @param _id ID
     */
    public void killAlarm(int _id){
        this.itLog_.debug("call kill alarm. [id:" + _id + "]");

        // ペンディングインテント作成
        PendingIntent pendingIntent = this.getPendingIntent(_id, PendingIntent.FLAG_NO_CREATE);
        if(pendingIntent != null) {
            this.alarmManager_.cancel(pendingIntent);
            pendingIntent.cancel();
            this.itLog_.debug("cancel alarm.");
        }
        else{
            this.itLog_.warning("Not created alarm pending intent. [id:" + _id + "]");
        }
    }

    /**
     * ペンディングインテント作成
     * @param _id 識別子
     * @param _flags 生成フラグ
     * @return 生成されたペンディングインテント
     */
    private PendingIntent getPendingIntent(int _id, int _flags){
        // インテント作成
        Intent intent = new Intent(this.getApplicationContext(), ITAlarmNotificationReceiver.class);
        // ペンディングインテント作成
        return PendingIntent.getBroadcast(this.getApplicationContext(), _id, intent, _flags);
    }

    /**
     * アラーム開始
     */
    public void onStartAlarm(){
        this.setMaxVolume();
        this.isAlarming_ = true;
    }
    /**
     * アラーム終了
     */
    public void onEndAlarm(){
        this.resetVolume();
        this.isAlarming_ = false;
    }

    /**
     * 最大音量に設定
     */
    private void setMaxVolume(){
        // 現在の音量を記録し、最大音量に設定
        this.beforeVolume_ = this.audioManager_.getStreamVolume(ITAlarmActivity.ALARM_STREAM_ID);
        int maxVolume = this.audioManager_.getStreamMaxVolume(ITAlarmActivity.ALARM_STREAM_ID);
        this.audioManager_.setStreamVolume(ITAlarmActivity.ALARM_STREAM_ID, maxVolume, 0);
    }
    /**
     * 音量を元に戻す
     */
    private void resetVolume(){
        // 音量を元に戻す
        this.audioManager_.setStreamVolume(ITAlarmActivity.ALARM_STREAM_ID, this.beforeVolume_, 0);
    }
}
