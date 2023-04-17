package com.example.battleship;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class MenuActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_menu);

        Button startBtn = (Button) findViewById(R.id.startButton);
        startBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                StartFieldCreation();
            }
        });
    }

    private void StartFieldCreation() {
        Intent switchActivityIntent = new Intent(this, FieldCreationActivity.class);
        startActivity(switchActivityIntent);
    }
}