package com.example.battleship;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;

public class FieldCreationActivity extends AppCompatActivity {
    private FieldSettings fieldSettings;
    private FieldCreation playerField;
    private FieldCreationComputer computerField;
    private Button startBtn;
    private LinearLayout layout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_field_creation);
        Init();
    }

    private void Init(){
        fieldSettings = new FieldSettings();
        playerField = new FieldCreation(this, fieldSettings);
        computerField = new FieldCreationComputer(this, fieldSettings);

        startBtn = (Button) findViewById(R.id.buttonStart);
        startBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                StartGame();
            }
        });
        startBtn.setVisibility(View.INVISIBLE);

        layout =  (LinearLayout) findViewById(R.id.fieldLayout);

        CreateField();
    }

    private void CreateField(){
        int cellSize = fieldSettings.fieldCreationSizeOnScreen / fieldSettings.fieldSize;
        for (int i = 0; i < fieldSettings.fieldSize; i++){
            LinearLayout horizontalLayout = new LinearLayout(this);
            horizontalLayout.setOrientation(LinearLayout.HORIZONTAL);
            horizontalLayout.setGravity(Gravity.CENTER);

            for (int j = 0; j < fieldSettings.fieldSize; j++){
                int x = i;
                int y = j;

                ImageView img = new ImageView(this);
                img.setImageResource(R.drawable.cell_simple);
                img.setTag(i+" "+ j);

                ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(cellSize, cellSize);
                img.setLayoutParams(layoutParams);

                img.setOnClickListener(new View.OnClickListener() {
                    public void onClick(View v) {
                        playerField.ChooseCell(x, y);
                    }
                });

                horizontalLayout.addView(img);
            }

            layout.addView(horizontalLayout);
        }
    }

    public void ChangeCellImage(int x, int y, FieldCell.CellImageType imageType){
        ImageView img = (ImageView) layout.findViewWithTag(x+" "+y);

        if(imageType == FieldCell.CellImageType.Simple){
            img.setImageResource(R.drawable.cell_simple);
        }else if(imageType == FieldCell.CellImageType.Empty){
            img.setImageResource(R.drawable.cell_empty);
        }else if(imageType == FieldCell.CellImageType.Ship){
            img.setImageResource(R.drawable.cell_ship);
        }
    }

    public void OnFieldCreated(){
        startBtn.setVisibility(View.VISIBLE);
    }

    public void OnFieldNotCreated(){
        startBtn.setVisibility(View.INVISIBLE);
    }

    private void StartGame() {
        Intent switchActivityIntent = new Intent(this, GameActivity.class);
        switchActivityIntent.putExtra("playerField", playerField.GetField());
        switchActivityIntent.putExtra("computerField", computerField.GetField());
        startActivity(switchActivityIntent);
    }
}