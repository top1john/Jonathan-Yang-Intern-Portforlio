package hungyiyang.lab1;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.os.StrictMode;
import android.preference.PreferenceManager;
import android.provider.MediaStore;
import android.text.TextUtils;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;

import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity implements View.OnClickListener,
        SharedPreferences.OnSharedPreferenceChangeListener {
    String choice;
    SharedPreferences settings;
    View mainView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        settings = PreferenceManager.getDefaultSharedPreferences(this);
        settings.registerOnSharedPreferenceChangeListener(this);

        mainView = findViewById(R.id.linear_layout_main);
        // String bgColor = settings.getString("background_main", "#ffffff");
        String bgColor = settings.getString("background_main_list", "#ffffff");
        mainView.setBackgroundColor(Color.parseColor(bgColor));

        Button postButton = findViewById(R.id.button_post_chatter);
        postButton.setOnClickListener(this);

        Button viewButton = findViewById(R.id.button_view_chatter);
        viewButton.setOnClickListener(this);

        if (android.os.Build.VERSION.SDK_INT > 9) {
            StrictMode.ThreadPolicy policy =
                    new StrictMode.ThreadPolicy.Builder().permitAll().build();
            StrictMode.setThreadPolicy(policy);
        }
    }

    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        switch (item.getItemId()) {
            case R.id.menu_item_view_preferences: {
                Intent intent = new Intent(this, PrefsActivity.class);
                startActivity(intent);
                break;
            }
            case R.id.menu_item_view_reviews: {
                if (TextUtils.isEmpty(choice)) {
                    Toast.makeText(getApplicationContext(), "A category MUST be selected", Toast.LENGTH_SHORT).show();
                } else {

                    Bundle bundle = new Bundle();
                    bundle.putString("MESSAGE", choice);

                    Intent intent = new Intent(this, CustomListViewActivity.class);
                    intent.putExtras((bundle));
                    startActivity(intent);
                    break;
                }
            }
        }
        return true;
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.menu_main, menu);

        return true;
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.button_post_chatter: {
                EditText editText = findViewById(R.id.edit_text_chatter);
                String message = editText.getText().toString();

                EditText nominee = findViewById(R.id.edit_text_nominee);
                String nomineeName = nominee.getText().toString();

                postToServer(message, nomineeName, choice);
                editText.setText("");
                nominee.setText("");
                break;
            }
            case R.id.button_view_chatter: {
                if (TextUtils.isEmpty(choice)) {
                    Toast.makeText(getApplicationContext(), "A category MUST be selected", Toast.LENGTH_SHORT).show();
                } else {

                    Bundle bundle = new Bundle();
                    bundle.putString("MESSAGE", choice);

                    Intent intent = new Intent(this, CustomListViewActivity.class);
                    intent.putExtras((bundle));
                    startActivity(intent);
                    break;
                }
            }
        }
    }

    private void postToServer(String message, String nomineeName, String choice) {
        String username = settings.getString("user_name", "unknown");
        String password = settings.getString("password", "Unknown");
        if (TextUtils.isEmpty(message) || TextUtils.isEmpty(nomineeName) || TextUtils.isEmpty(choice)) {
            Toast.makeText(getApplicationContext(), "Review, Nominee Name, and Category CANNOT be left empty", Toast.LENGTH_SHORT).show();
        } else {

            try {
                HttpClient client = new DefaultHttpClient();
                HttpPost form = new HttpPost("http://www.youcode.ca/Lab01Servlet");
                List<NameValuePair> formParameters = new ArrayList<NameValuePair>();
                formParameters.add(new BasicNameValuePair("REVIEW", message));
                formParameters.add(new BasicNameValuePair("REVIEWER", username));
                formParameters.add(new BasicNameValuePair("NOMINEE", nomineeName));
                formParameters.add(new BasicNameValuePair("CATEGORY", choice));
                formParameters.add(new BasicNameValuePair("PASSWORD", password));
                UrlEncodedFormEntity formEntity = new UrlEncodedFormEntity(formParameters);

                Toast.makeText(getApplicationContext(), "Form sent successfully ", Toast.LENGTH_SHORT).show();
                form.setEntity(formEntity);
                client.execute(form);
            } catch (Exception e) {
                Toast.makeText(this, "Error: " + e, Toast.LENGTH_LONG).show();
            }
        }

    }

    @Override
    public void onSharedPreferenceChanged(SharedPreferences sharedPreferences,
                                          String key) {
        //String bgColor = settings.getString("background_main", "#ffffff");

        String bgColor = settings.getString("background_main_list", "#ffffff");
        mainView.setBackgroundColor(Color.parseColor(bgColor));


    }


    public void onRadioButtonClicked(View view) {
        // Is the button now checked?
        boolean checked = ((RadioButton) view).isChecked();


        // Check which radio button was clicked
        switch (view.getId()) {
            case R.id.radio_film:
                if (checked)
                    Toast.makeText(getApplicationContext(), "Selected Best Picture", Toast.LENGTH_SHORT).show();
                choice = "film";
                break;
            case R.id.radio_actor:
                if (checked)
                    Toast.makeText(getApplicationContext(), "Selected Best Actor", Toast.LENGTH_SHORT).show();
                choice = "actor";
                break;
            case R.id.radio_actress:
                if (checked)
                    Toast.makeText(getApplicationContext(), "Selected Best Actress", Toast.LENGTH_SHORT).show();
                choice = "actress";
                break;
            case R.id.radio_editing:
                if (checked)
                    Toast.makeText(getApplicationContext(), "Selected Film Editing", Toast.LENGTH_SHORT).show();
                choice = "editing";
                break;
            case R.id.radio_effects:
                if (checked)
                    Toast.makeText(getApplicationContext(), "Selected Visual Effects", Toast.LENGTH_SHORT).show();
                choice = "effects";
                break;
        }
    }


}
