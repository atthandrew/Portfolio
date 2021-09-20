package androidsamples.java.dicegames;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.sql.Timestamp;
import java.util.Calendar;
import java.util.Locale;

public class WalletActivity extends AppCompatActivity {
    private static final String TAG = "WalletActivity";
    private static final String KEY_BALANCE="KEY_BALANCE";
    private static final String KEY_DIE_VALUE="KEY_DIE_VALUE";
    private static final String FILENAME = "roll_check.txt";

    private TextView mTxtBalance;
    private TextView mTxtNumWins;
    private TextView mTxtTotalRolls;
    private TextView mTxtPrevRoll;
    private TextView mTxtDblWins;
    private TextView mTxtDblOthers;
    private Button mBtnDie;

    private WalletViewModel mWalletVM;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d(TAG, "onCreate");
        setContentView(R.layout.activity_wallet);

        mTxtBalance = findViewById(R.id.txt_balance);
        mTxtNumWins = findViewById(R.id.txt_num_wins);
        mTxtTotalRolls = findViewById(R.id.txt_total_rolls);
        mTxtPrevRoll = findViewById(R.id.txt_prev_roll);
        mTxtDblWins = findViewById(R.id.txt_dbl_wins);
        mTxtDblOthers = findViewById(R.id.txt_dbl_others);
        mBtnDie = findViewById(R.id.btn_die);

        mWalletVM = new ViewModelProvider(this).get(WalletViewModel.class);
        updateUI();

        /*
        if (savedInstanceState != null) {
            mBalance = savedInstanceState.getInt(KEY_BALANCE, 0);
            int dieValue = savedInstanceState.getInt(KEY_DIE_VALUE, 0);
            mTxtBalance.setText(Integer.toString(mBalance));
            mBtnDie.setText(Integer.toString(dieValue));
            Log.d(TAG, "Restored: balance = " + mBalance + ", die = " + dieValue);
        }
         */

        mBtnDie.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                mWalletVM.rollDie();
                updateUI();

                //Write to the end of the file every time
                try (FileOutputStream fOut = openFileOutput(FILENAME, Context.MODE_APPEND)) {
                    //Create a Timestamp using the current time in milliseconds from Calendar
                    Timestamp ts = new Timestamp(Calendar.getInstance().getTimeInMillis());
                    String time = ts.toString();
                    //Combine the Timestamp with the die's value
                    String roll_line = "<" + time + ">, " + Integer.toString(mWalletVM.getDieValue()) + "\n";
                    //
                    fOut.write(roll_line.getBytes());
                } catch (IOException e) {
                    e.printStackTrace();
                }

                if(mWalletVM.getDieValue() == mWalletVM.getWinValue()) {
                    Context context = getApplicationContext();
                    CharSequence text = "Congratulations!";
                    int duration = Toast.LENGTH_SHORT;

                    Toast.makeText(context, text, duration).show();
                }
            }
        });
    }

    private void updateUI() {
        mTxtBalance.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getmBalance()));
        mTxtNumWins.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getmNumWins()));
        mTxtTotalRolls.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getmTotalRolls()));
        mTxtPrevRoll.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getmPrevRoll()));
        mTxtDblWins.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getmDblWins()));
        mTxtDblOthers.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getmDblOthers()));
        mBtnDie.setText(String.format(Locale.getDefault(), "%d", mWalletVM.getDieValue()));
    }

    /*
    @Override
    protected void onSaveInstanceState(@NonNull Bundle outState) {
        super.onSaveInstanceState(outState);
        Log.d(TAG, "onSaveInstanceState");
        outState.putInt(KEY_BALANCE, mBalance);
        outState.putInt(KEY_DIE_VALUE, mDie.value());
        Log.d(TAG, "Saved: balance = " + mBalance + " die = " + mDie.value());
    }
     */

    @Override
    protected void onStart() {
        super.onStart();
        Log.d(TAG, "onStart");
    }

    @Override
    protected void onResume() {
        super.onResume();
        Log.d(TAG, "onResume");
    }

    @Override
    protected void onStop() {
        super.onStop();
        Log.d(TAG, "onStop");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Log.d(TAG, "onDestroy");
    }

    @Override
    protected void onPause() {
        super.onPause();
        Log.d(TAG, "onPause");
    }

}