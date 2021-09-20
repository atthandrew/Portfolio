package androidsamples.java.journalapp;

import android.app.AlertDialog;
import android.app.Dialog;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;

public class InfoDialogFragment extends DialogFragment {
    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        AlertDialog.Builder builder = new AlertDialog.Builder(requireContext());
        builder.setTitle(R.string.app_name)
                .setMessage(R.string.app_info)
                .setPositiveButton(R.string.ok, (dialogInterface, i) -> dismiss());
        return builder.create();
    }
}
