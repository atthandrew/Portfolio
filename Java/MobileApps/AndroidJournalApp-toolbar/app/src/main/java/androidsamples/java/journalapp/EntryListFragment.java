package androidsamples.java.journalapp;

import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.google.android.material.floatingactionbutton.FloatingActionButton;

import java.util.List;
import java.util.UUID;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import org.w3c.dom.Text;

public class EntryListFragment extends Fragment {
    private static final String TAG = "EntryListFragment";
    private EntryListViewModel mEntryListViewModel;

    interface Callbacks {
        void onEntrySelected(UUID id);
    }

    private Callbacks mCallbacks = null;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        mEntryListViewModel = new ViewModelProvider(this).get(EntryListViewModel.class);
        setHasOptionsMenu(true);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_entry_list, container, false);
        FloatingActionButton fab = view.findViewById(R.id.floatingActionButton);
        fab.setOnClickListener(this::addNewEntry);

        RecyclerView entriesList = view.findViewById(R.id.recyclerView);
        entriesList.setLayoutManager(new LinearLayoutManager(getActivity()));
        EntryListAdapter adapter = new EntryListAdapter(getActivity());
        entriesList.setAdapter(adapter);

        mEntryListViewModel.getAllEntries().observe(getActivity(), adapter::setEntries);

        return view;
    }

    public void addNewEntry(View view) {
        JournalEntry entry = new JournalEntry("", "Date", "Time Start", "Time End");
        mEntryListViewModel.insert(entry);
        mCallbacks.onEntrySelected(entry.getUid());
    }


    @Override
    public void onAttach(@NonNull Context context) {
        super.onAttach(context);
        mCallbacks = (Callbacks) context;
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mCallbacks = null;
    }

    @Override
    public void onCreateOptionsMenu(@NonNull Menu menu, @NonNull MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.fragment_entry_list, menu);
    }

    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        if (item.getItemId() == R.id.menu_info) {
            new InfoDialogFragment().show(this.getFragmentManager(), "INFO");
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private class EntryViewHolder extends RecyclerView.ViewHolder {
        private final TextView mTxtTitle;
        private final TextView mTxtDate;
        private final TextView mTxtTimeStart;
        private final TextView mTxtTimeEnd;
        private JournalEntry mEntry;


        public EntryViewHolder(@NonNull View itemView) {
            super(itemView);

            mTxtTitle = itemView.findViewById(R.id.txt_item_title);
            mTxtDate = itemView.findViewById(R.id.txt_item_date);
            mTxtTimeStart = itemView.findViewById(R.id.txt_item_timestart);
            mTxtTimeEnd = itemView.findViewById(R.id.txt_item_timeend);

            itemView.setOnClickListener(this::launchJournalEntryFragment);
        }

        private void launchJournalEntryFragment(View v) {
            Log.d(TAG, "launchJournalEntryFragment with Entry: " + mEntry.title());
            mCallbacks.onEntrySelected(mEntry.getUid());
        }

        void bind(JournalEntry entry) {
            mEntry = entry;
            this.mTxtTitle.setText(mEntry.title());
            this.mTxtDate.setText(mEntry.date());
            this.mTxtTimeStart.setText(mEntry.timeStart());
            this.mTxtTimeEnd.setText(mEntry.timeEnd());
        }
    }

    private class EntryListAdapter extends RecyclerView.Adapter<EntryViewHolder> {
        private final LayoutInflater mInflater;
        private List<JournalEntry> mEntries;

        public EntryListAdapter(Context context) {
            mInflater = LayoutInflater.from(context);
        }

        @NonNull
        @Override
        public EntryViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View itemView = mInflater.inflate(R.layout.journal_item, parent, false);
            return new EntryViewHolder(itemView);
        }

        @Override
        public void onBindViewHolder(@NonNull EntryViewHolder holder, int position) {
            if (mEntries != null) {
                JournalEntry current = mEntries.get(position);
                holder.bind(current);
            }
        }

        @Override
        public int getItemCount() {
            return (mEntries == null) ? 0 : mEntries.size();
        }

        public void setEntries(List<JournalEntry> entries) {
            mEntries = entries;
            notifyDataSetChanged();
        }
    }
}
