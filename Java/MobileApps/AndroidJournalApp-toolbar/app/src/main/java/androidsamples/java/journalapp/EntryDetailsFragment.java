package androidsamples.java.journalapp;

import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TimePicker;

import java.util.Calendar;
import java.util.Locale;
import java.util.UUID;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.room.Delete;

public class EntryDetailsFragment extends Fragment {
    private static final String TAG = "EntryDetailsFragment";
    private EditText mEditTitle;
    private Button mDate, mTimeStart, mTimeEnd;
    private EntryDetailsViewModel mEntryDetailsViewModel;
    private JournalEntry mEntry;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        mEntryDetailsViewModel = new ViewModelProvider(getActivity()).get(EntryDetailsViewModel.class);
        setHasOptionsMenu(true);

        UUID entryId = (UUID) getArguments().getSerializable(MainActivity.KEY_ENTRY_ID);
        Log.d(TAG, "Loading entry: " + entryId);

        mEntryDetailsViewModel.loadEntry(entryId);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.journal_entry_detail, container, false);
        mEditTitle = view.findViewById(R.id.edit_title);
        mDate = view.findViewById(R.id.btn_date);
        mTimeStart = view.findViewById(R.id.btn_timestart);
        mTimeEnd = view.findViewById(R.id.btn_timeend);

        view.findViewById(R.id.btn_save).setOnClickListener(this::saveEntry);
        mDate.setOnClickListener(this::showDatePickerDialog);
        mTimeStart.setOnClickListener(this::showTimeStartPickerDialog);
        mTimeEnd.setOnClickListener(this::showTimeEndPickerDialog);

        return view;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        mEntryDetailsViewModel.getEntryLiveData().observe(getActivity(),
                entry -> {
                    this.mEntry = entry;
                    updateUI();
                });
    }

    @Override
    public void onCreateOptionsMenu(@NonNull Menu menu, @NonNull MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.journal_entry_detail, menu);
    }

    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        if (item.getItemId() == R.id.menu_delete) {
            AlertDialog.Builder builder = new AlertDialog.Builder(requireContext());
            builder.setTitle(R.string.delete)
                    .setMessage(R.string.delete_confirm)
                    .setNegativeButton(R.string.no, (dialogInterface, i) -> dialogInterface.dismiss())
                    .setPositiveButton(R.string.yes, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialogInterface, int i) {
                            Log.d(TAG, "Delete Confirmed");
                            dialogInterface.dismiss();
                            mEntryDetailsViewModel.deleteEntry(mEntry);
                            getActivity().onBackPressed();
                        }
                    }).show();
            return true;
        }
        if (item.getItemId() == R.id.menu_share) {
            String title = mEditTitle.getText().toString();
            String date = mDate.getText().toString();
            String tStart = mTimeStart.getText().toString();
            String tEnd = mTimeEnd.getText().toString();
            String message = "Look what I have been up to: " +
                    title + " on " + date + ", " + tStart + " to " + tEnd;

            Intent sendIntent = new Intent();
            sendIntent.setAction(Intent.ACTION_SEND);
            sendIntent.putExtra(Intent.EXTRA_TEXT, message);
            sendIntent.setType("text/plain");

            Intent shareIntent = Intent.createChooser(sendIntent, null);
            startActivity(shareIntent);

            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private void updateUI() {
        if(mEntry != null) {
            mEditTitle.setText(mEntry.title());
            mDate.setText(mEntry.date());
            mTimeStart.setText(mEntry.timeStart());
            mTimeEnd.setText(mEntry.timeEnd());
        }
    }

    private void saveEntry(View v) {
        Log.d(TAG, "Save button clicked");
        mEntry.setTitle(mEditTitle.getText().toString());
        mEntry.setDate(mDate.getText().toString());
        mEntry.setTimeStart(mTimeStart.getText().toString());
        mEntry.setTimeEnd(mTimeEnd.getText().toString());
        mEntryDetailsViewModel.saveEntry(mEntry);

        getActivity().onBackPressed();
    }

    private void showDatePickerDialog(View v) {
        DatePickerFragment newFragment = new DatePickerFragment();
        Calendar cal = Calendar.getInstance();
        Bundle args = new Bundle();
        args.putInt("year", cal.get(Calendar.YEAR));
        args.putInt("month", cal.get(Calendar.MONTH));
        args.putInt("day", cal.get(Calendar.DAY_OF_MONTH));
        newFragment.setArguments(args);

        newFragment.setCallBack(onDate);
        newFragment.show(this.getFragmentManager(), "datePicker");
    }

    DatePickerDialog.OnDateSetListener onDate = new DatePickerDialog.OnDateSetListener() {
        @Override
        public void onDateSet(DatePicker datePicker, int year, int month, int day) {
            Calendar c = Calendar.getInstance();
            c.set(year, month, day);
            String day_of_week = c.getDisplayName(Calendar.DAY_OF_WEEK, Calendar.SHORT, Locale.getDefault());
            String m = c.getDisplayName(Calendar.MONTH, Calendar.SHORT, Locale.getDefault());
            String d = Helper.toLocalizedString(day);
            String y = Helper.toLocalizedString(year);

            String date = day_of_week + ", " + m + " " + d + ", " + y;
            mDate.setText(date);
        }
    };

    private void showTimeStartPickerDialog(View v) {
        TimePickerFragment newFragment = new TimePickerFragment();
        Calendar cal = Calendar.getInstance();
        Bundle args = new Bundle();
        args.putInt("hour", cal.get(Calendar.HOUR_OF_DAY));
        args.putInt("minute", cal.get(Calendar.MINUTE));
        newFragment.setArguments(args);

        newFragment.setCallBack(onTimeStart);
        newFragment.show(this.getFragmentManager(), "timePicker");
    }

    TimePickerDialog.OnTimeSetListener onTimeStart = new TimePickerDialog.OnTimeSetListener() {
        @Override
        public void onTimeSet(TimePicker timePicker, int hour, int minute) {
            String h = String.format(Locale.getDefault(), "%02d", hour);
            String m = String.format(Locale.getDefault(), "%02d", minute);

            String time = h + ":" + m;
            mTimeStart.setText(time);
        }
    };

    private void showTimeEndPickerDialog(View v) {
        TimePickerFragment newFragment = new TimePickerFragment();
        Calendar cal = Calendar.getInstance();
        Bundle args = new Bundle();
        args.putInt("hour", cal.get(Calendar.HOUR_OF_DAY));
        args.putInt("minute", cal.get(Calendar.MINUTE));
        newFragment.setArguments(args);

        newFragment.setCallBack(onTimeEnd);
        newFragment.show(this.getFragmentManager(), "timePicker");
    }

    TimePickerDialog.OnTimeSetListener onTimeEnd = new TimePickerDialog.OnTimeSetListener() {
        @Override
        public void onTimeSet(TimePicker timePicker, int hour, int minute) {
            String h = String.format(Locale.getDefault(), "%02d", hour);
            String m = String.format(Locale.getDefault(), "%02d", minute);

            String time = h + ":" + m;
            mTimeEnd.setText(time);
        }
    };
}
