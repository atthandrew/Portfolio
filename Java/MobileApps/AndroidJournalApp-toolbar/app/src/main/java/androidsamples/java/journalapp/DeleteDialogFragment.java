package androidsamples.java.journalapp;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;

public class DeleteDialogFragment extends DialogFragment {

    DialogInterface.OnClickListener onDeleteConfirm;

    public DeleteDialogFragment() { super(); };

    public void setCallBack(DialogInterface.OnClickListener onDelete) { onDeleteConfirm = onDelete; }

    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        AlertDialog.Builder builder = new AlertDialog.Builder(requireContext());
        builder.setTitle(R.string.delete)
                .setMessage(R.string.delete_confirm)
                .setNegativeButton(R.string.no, (dialogInterface, i) -> dismiss())
                .setPositiveButton(R.string.yes, onDeleteConfirm);
        return builder.create();
    }
}
