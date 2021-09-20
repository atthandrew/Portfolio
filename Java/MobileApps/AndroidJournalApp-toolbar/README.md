# AndroidJournalApp

a. AndroidJournalApp, Andrew Thompson, u0879848, a.t.thandrew@gmail.com
b. This app maintains a database of Journal Entries, which contain a title, date, time started, and
   time completed. Upon adding new a new entry, users are taken to the entry details screen, where
   they can edit the appropriate fields, share all of the fields using other apps, and delete the
   current entry. Selecting a saved entry from the main entry list will allow you to edit, share, or
   delete the entry as well. Performing any of these activities does not seem to have any bugs,
   however, on occasion, when left alone for a while, I have seen that the app can become
   unresponsive and say the app keeps shutting down. Since these issues were accompanied by other
   system-related bugs, like being unable to exit the app, losing the home and back buttons on the
   bottom, and other visual bugs, I assume it was an issue related to the Virtual Device, and not
   the app itself, since the app performs all the needed actions just fine (wiping data on the
   virtual device resolved the problem.)
c. 1. No major changes
   2. To implement both the date, time start, and time end buttons, I first had to make them in the
      journal_entry_detail.xml and journal_item.xml layouts. I followed the same ideas used by the
      duration field, except I made them buttons. I made sure to add these new fields to the
      JournalEntry, updated EntryListFragment to add these adjusted entries, and made the buttons
      and OnClickListeners in EntryDetailsFragment. I made two more classes, DatePickerFragment and
      TimePickerFragment, and found that since I am using a DatePicker and TimePicker to set button
      text, I had to make the OnDateSet and OnTimeSet listeners inside the EntryDetailsFragment, and
      send the listeners to the Date and Time Pickers as found here:
      https://stackoverflow.com/questions/20673609/implement-a-datepicker-inside-a-fragment
      https://developer.android.com/guide/topics/ui/controls/pickers
      This let me get the info from the date and time pickers, and update the button text to save.
   3. To get the delete functionality, I first made a new menu layout for the EntryDetailsFragment,
      called journal_entry_detail.xml, and made the menu button like we did for info. I made the
      delete function in the JournalRepository/JournalEntryDao, and updated EntryDetailsViewModel
      to use this function. Once the delete function was set up, I adjusted EntryDetailsFragment to
      have an options menu, and in the OnOptionItemSelected function, I checked to see if the
      delete menu button was clicked, and if it was, I used an AlertDialog.Builder to make the
      dialog inside of the EntryDetailsFragment rather than use a separate fragment (an attempt at
      this can be found in DeleteDialogFragment, but it is not being used.) On clicking the positive
      button, the entry is deleted and the back button is pressed. Sources:
      https://stackoverflow.com/questions/2478517/how-to-display-a-yes-no-dialog-box-on-android
   4. To share a string with other apps, I created another menu button in the menu layout we made
      for delete, and then was able to go straight to the EntryDetailsFragment to implement sharing
      with other apps. All I had to do was make a case for selecting the share button in the
      OnOptionItemSelected function, make the string using the current button texts (I chose to do
      this instead of using the database entry, in case users wanted to share before saving the
      entry), and send the string using the Android Sharesheet and Intents, found here:
      https://developer.android.com/training/sharing/send
d. To further enhance the JournalApp, it would be nice if there was some form of organization for
   your jounal entries. It would be cool if you could drag and drop entries to reorder them on the
   list, and to perhaps create categories/folders to group similar entries together. You could also
   turn entries into a form of a to-do list, with notifications for upcoming entries and check
   boxes that would move entries into a completed folder.
e. Estimated 18 hours
f. Difficulty: 7
