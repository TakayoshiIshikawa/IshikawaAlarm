package trident.ishikawatakayoshi.ishikawaalarm;

import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.PowerManager;

public class ITAlarmNotificationReceiver extends BroadcastReceiver {
    private final ITLog itLog_ = new ITLog("ITANReceiver");

    @Override
    public void onReceive(Context _context, Intent _intent){
        this.itLog_.debug("action:" + _intent.getAction());

        this.itLog_.debug("Start unity player activity.");
        Intent notification = new Intent(_context.getApplicationContext(), ITAlarmActivity.class);
        // 画面起動に以下が必要
        notification.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        _context.getApplicationContext().startActivity(notification);
    }
}
