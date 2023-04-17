package com.example.battleship;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Point;
import android.os.Bundle;
import android.view.Display;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;

public class GameActivity extends Activity {
    private Button menuButton;
    private FieldSettings fieldSettings;
    private BattleField battleFieldPlayer;
    private BattleField battleFieldComputer;
    private LinearLayout layoutPlayer;
    private LinearLayout layoutComputer;

    private boolean isPlayerTurn;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Init();
    }

    private void Init(){
        menuButton = (Button) findViewById(R.id.menuButton);
        menuButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                GoToMenu();
            }
        });
        menuButton.setVisibility(View.INVISIBLE);

        isPlayerTurn = true;
        fieldSettings = new FieldSettings();

        ArrayList<ArrayList<FieldCell>> playerField = null;
        ArrayList<ArrayList<FieldCell>> computerField = null;

        Bundle extras = getIntent().getExtras();
        if (extras != null) {
            playerField = ( ArrayList<ArrayList<FieldCell>>) extras.get("computerField");
            computerField = ( ArrayList<ArrayList<FieldCell>>) extras.get("playerField");
        }else{
            GoToMenu();
            return;
        }

        battleFieldPlayer = new BattleFieldPlayer(this, fieldSettings, playerField);
        battleFieldComputer = new BattleFieldComputer(this, fieldSettings, computerField);

        layoutPlayer = (LinearLayout) findViewById(R.id.playerFieldLayout);
        layoutComputer =  (LinearLayout) findViewById(R.id.computerFieldLayout);

        CreateField(layoutPlayer, battleFieldPlayer, true);
        CreateField(layoutComputer, battleFieldComputer, false);

        battleFieldPlayer.GetTurn();
    }

    private void CreateField(LinearLayout layout, BattleField battleField, boolean isToSetOnClick){
        int size = fieldSettings.fieldSize;
        Display display = getWindowManager().getDefaultDisplay();
        Point displaySize = new Point();
        display.getSize(displaySize);
        int width = displaySize.x;
        width *= fieldSettings.fieldSizeOnScreen;

        int cellSize = width / size;

        for(int i = 0; i < size; i++){
            LinearLayout horizontalLayout = new LinearLayout(this);
            horizontalLayout.setOrientation(LinearLayout.HORIZONTAL);
            horizontalLayout.setGravity(Gravity.CENTER);

            for(int j = 0; j < size; j++) {
                int x = i;
                int y = j;

                ImageView img = new ImageView(this);
                img.setImageResource(R.drawable.cell_simple);
                img.setTag(i+" "+ j);

                ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(cellSize, cellSize);
                img.setLayoutParams(layoutParams);

                if(isToSetOnClick){
                    img.setOnClickListener(new View.OnClickListener() {
                        public void onClick(View v) {
                            battleField.ChooseCell(x, y);
                        }
                    });
                }

                horizontalLayout.addView(img);
            }
            layout.addView(horizontalLayout);
        }
    }

    public void ChangeCellImage(int x, int y, FieldCell.CellImageType imageType, boolean isPlayer){
        LinearLayout layout;

        if(isPlayer){
            layout = layoutPlayer;
        }else{
            layout = layoutComputer;
        }

        ImageView img = (ImageView) layout.findViewWithTag(x+" "+y);

        if(imageType == FieldCell.CellImageType.Simple){
            img.setImageResource(R.drawable.cell_simple);
        }else if(imageType == FieldCell.CellImageType.Empty){
            img.setImageResource(R.drawable.cell_empty);
        }else if(imageType == FieldCell.CellImageType.Ship){
            img.setImageResource(R.drawable.cell_ship);
        }
    }

    public void NextTurn(){
        isPlayerTurn = !isPlayerTurn;
        TextView turnText = (TextView)findViewById(R.id.turnText);
        if(isPlayerTurn){
            turnText.setText("YOUR turn!");
            battleFieldPlayer.GetTurn();
        }else{
            turnText.setText("COMPUTER turn!");
            battleFieldComputer.GetTurn();
        }
    }

    public void Win(boolean isPlayer){
        TextView turnText = (TextView)findViewById(R.id.turnText);
        if(isPlayer){
            turnText.setText("YOU WIN!");
        }else{
            turnText.setText("Computer win!");
        }

        menuButton.setVisibility(View.VISIBLE);
    }

    private void GoToMenu() {
        Intent switchActivityIntent = new Intent(this, MenuActivity.class);
        startActivity(switchActivityIntent);
    }
}