package androidsamples.java.journalapp;

import java.util.List;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;

public class EntryListViewModel extends ViewModel {
    private final JournalRepository mRepository;

    public EntryListViewModel() {
        mRepository = JournalRepository.getInstance();
    }

    public LiveData<List<JournalEntry>> getAllEntries() {
        return mRepository.getAllEntries();
    }

    public void insert(JournalEntry entry) {
        mRepository.insert(entry);
    }
}
