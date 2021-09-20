package androidsamples.java.journalapp;

import java.util.UUID;

import androidx.annotation.NonNull;
import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

@Entity(tableName = "journal_table")
public class JournalEntry {
    @PrimaryKey
    @ColumnInfo(name = "id")
    @NonNull
    private UUID mUid;

    @ColumnInfo(name = "title")
    private String mTitle;

    @ColumnInfo(name = "date")
    private String mDate;

    @ColumnInfo(name = "timeStart")
    private String mTimeStart;

    @ColumnInfo(name = "timeEnd")
    private String mTimeEnd;

    public JournalEntry(@NonNull String title, String date, String timeStart, String timeEnd) {
        mUid = UUID.randomUUID();
        mTitle = title;
        mDate = date;
        mTimeStart = timeStart;
        mTimeEnd = timeEnd;
    }

    @NonNull
    public UUID getUid() {
        return mUid;
    }

    public void setUid(@NonNull UUID id) {
        mUid = id;
    }

    @NonNull
    public String title() {
        return mTitle;
    }

    public void setTitle(String title) {
        mTitle = title;
    }

    public String date() { return mDate; }

    public void setDate(String date) { mDate = date; }

    public String timeStart() { return mTimeStart; }

    public void setTimeStart(String timeStart) { mTimeStart = timeStart; }

    public String timeEnd() { return mTimeEnd; }

    public void setTimeEnd(String timeEnd) { mTimeEnd = timeEnd; }

}
