package hungyiyang.lab1;

import android.os.Bundle;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.URI;
import java.util.ArrayList;
import java.util.HashMap;

public class CustomListViewActivity extends AppCompatActivity {
    ArrayList<HashMap<String, String>> chatter =
            new ArrayList<HashMap<String, String>>();


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_custom_list_view);

            loadListView();

    }

    private void loadListView() {
        String[] fields = new String[]{"DATE", "REVIEWER", "CATEGORY", "NOMINEE", "REVIEW"};
        int[] ids = new int[]{R.id.custom_row_date,
                R.id.custom_row_reviewer,
                R.id.custom_row_category,
                R.id.custom_row_nominee,
                R.id.custom_row_review,
        };

        populateList();
        SimpleAdapter adapter = new SimpleAdapter(this, chatter, R.layout.custom_list_row,
                fields, ids);
        ListView listView = findViewById(R.id.list_view_custom);
        listView.setAdapter(adapter);

    }

    private void populateList() {
        BufferedReader in = null;

        Bundle bundle = this.getIntent().getExtras();
        String choice = bundle.getString("MESSAGE");


        try {
            HttpClient client = new DefaultHttpClient();
            HttpGet request = new HttpGet();
            request.setURI(new URI("http://www.youcode.ca/Lab01Servlet?CATEGORY=" + choice));
            HttpResponse response = client.execute(request);
            in = new BufferedReader(new InputStreamReader(response.getEntity().getContent()));
            String line = "";
            while ((line = in.readLine()) != null) {
                HashMap<String, String> temp = new HashMap<String, String>();

                temp.put("DATE", line);

                line = in.readLine();
                temp.put("REVIEWER", line);

                line = in.readLine();
                temp.put("CATEGORY", line);

                line = in.readLine();
                temp.put("NOMINEE", line);

                line = in.readLine();
                temp.put("REVIEW", line);

                chatter.add(temp);
            }
            in.close();
        } catch (Exception e) {
            Toast.makeText(this, "Error: " + e, Toast.LENGTH_LONG).show();
        }

    }
}
