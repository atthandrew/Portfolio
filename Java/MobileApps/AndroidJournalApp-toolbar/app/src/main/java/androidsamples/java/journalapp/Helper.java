package androidsamples.java.journalapp;

import java.util.Locale;

public class Helper {
    private Helper() {
        // this object cannot be created
    }

    static String toLocalizedString(int n) {
        return String.format(Locale.getDefault(), "%s", n);
    }

}
