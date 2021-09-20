package androidsamples.java.journalapp;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.TimePickerDialog;
import android.os.Bundle;
import android.text.format.DateFormat;

import androidx.constraintlayout.motion.widget.TimeCycleSplineSet;
import androidx.fragment.app.DialogFragment;

public class TimePickerFragment extends DialogFragment {
    TimePickerDialog.OnTimeSetListener onTimeSet;
    private int hour, minute;

    public TimePickerFragment() {};

    public void setCallBack(TimePickerDialog.OnTimeSetListener onTime) {
        onTimeSet = onTime;
    }

    public void setArguments(Bundle args) {
        super.setArguments(args);
        hour = args.getInt("hour");
        minute = args.getInt("minute");
    }

    @Override
    public Dialog onCreateDialog(Bundle savedInstanceState) {
        // Create a new instance of DatePickerDialog and return it
        return new TimePickerDialog(getActivity(), onTimeSet, hour, minute, DateFormat.is24HourFormat(getActivity()));
    }
}
