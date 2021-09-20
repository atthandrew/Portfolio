package androidsamples.java.journalapp;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.os.Bundle;

import androidx.fragment.app.DialogFragment;

public class DatePickerFragment extends DialogFragment {

    DatePickerDialog.OnDateSetListener onDateSet;
    private int year, month, day;

    public DatePickerFragment() {};

    public void setCallBack(DatePickerDialog.OnDateSetListener onDate) {
        onDateSet = onDate;
    }

    public void setArguments(Bundle args) {
        super.setArguments(args);
        year = args.getInt("year");
        month = args.getInt("month");
        day = args.getInt("day");
    }

    @Override
    public Dialog onCreateDialog(Bundle savedInstanceState) {
        // Create a new instance of DatePickerDialog and return it
        return new DatePickerDialog(getActivity(), onDateSet, year, month, day);
    }

}
