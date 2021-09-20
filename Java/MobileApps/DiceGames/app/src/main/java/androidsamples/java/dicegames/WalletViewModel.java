package androidsamples.java.dicegames;

import android.util.Log;

import androidx.lifecycle.ViewModel;

public class WalletViewModel extends ViewModel {
    private static final String TAG = "WalletViewModel";
    private static final int WIN_VALUE = 6;
    private static final int INCREMENT = 5;
    private int mBalance;
    private int mNumWins;
    private int mTotalRolls;
    private int mPrevRoll;
    private int mCurRoll;
    private int mDblWins;
    private int mDblOthers;
    private Die mDie;

    public WalletViewModel() {
        mBalance = 0;
        mNumWins = 0;
        mTotalRolls = 0;
        mPrevRoll = 0;
        mCurRoll = 0;
        mDblWins = 0;
        mDblOthers = 0;
        mDie = new Die6();
    }

    public static int getWinValue() {
        return WIN_VALUE;
    }

    public int getmBalance() {
        return mBalance;
    }

    public void setmBalance(int mBalance) {
        this.mBalance = mBalance;
    }

    public int getmNumWins() {
        return mNumWins;
    }

    public void setmNumWins(int mNumWins) {
        this.mNumWins = mNumWins;
    }

    public int getmTotalRolls() {
        return mTotalRolls;
    }

    public void setmTotalRolls(int mTotalRolls) {
        this.mTotalRolls = mTotalRolls;
    }

    public int getmPrevRoll() {
        return mPrevRoll;
    }

    public void setmPrevRoll(int mPrevRoll) {
        this.mPrevRoll = mPrevRoll;
    }

    public int getmDblWins() {
        return mDblWins;
    }

    public void setmDblWins(int mDblWins) {
        this.mDblWins = mDblWins;
    }

    public int getmDblOthers() {
        return mDblOthers;
    }

    public void setmDblOthers(int mDblOthers) {
        this.mDblOthers = mDblOthers;
    }

    public int getDieValue() {
        return mDie.value();
    }

    public void rollDie() {
        //Store the old current roll in mPrevRoll
        mPrevRoll = mCurRoll;
        //Roll the Die
        mDie.roll();
        //Store the new current roll.
        mCurRoll = mDie.value();
        mTotalRolls++;
        Log.d(TAG, "Die roll = " + mCurRoll);
        //add coins if the win_value is rolled
        if (mCurRoll == WIN_VALUE) {
            mBalance += INCREMENT;

            //If the previous roll was also a win, add coins again and count as a double win.
            //NOTE: A double win is not also counted as a standard win, hence the else statement
            if(mPrevRoll == WIN_VALUE) {
                mBalance += INCREMENT;
                mDblWins++;
            }
            else {
                mNumWins++;
            }

            Log.d(TAG, "New balance = " + mBalance);
        }
        else {
            //If the current roll is not a win, and is the same as the previous roll, take away
            //coins and count as a double other.
            if(mCurRoll == mPrevRoll) {
                mBalance -= INCREMENT;
                mDblOthers++;
                Log.d(TAG, "New balance = " + mBalance);
            }
        }
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        Log.d(TAG, "onCleared");
    }
}
