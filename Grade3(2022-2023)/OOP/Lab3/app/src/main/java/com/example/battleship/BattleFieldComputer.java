package com.example.battleship;

import java.util.ArrayList;
import java.util.Random;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;

public class BattleFieldComputer extends BattleField{
    private int timeDelay = 1000;
    public BattleFieldComputer(GameActivity mainActivity, FieldSettings fieldSettings, ArrayList<ArrayList<FieldCell>> field){
        super(mainActivity, fieldSettings, field);
        isPlayer = false;
    }

    @Override
    protected void ActionOnWin(){
        mainActivity.Win(false);
    }

    @Override
    public void GetTurn() {
        isFieldLocked = false;

        int x = randInt(0, fieldSettings.fieldSize-1);
        int y = randInt(0, fieldSettings.fieldSize-1);
        while(!field.get(x).get(y).GetState()){
            x = randInt(0, fieldSettings.fieldSize-1);
            y = randInt(0, fieldSettings.fieldSize-1);
        }

        int finalX = x;
        int finalY = y;

        Thread thread = new Thread() {
            @Override
            public void run() {
                try {
                    Thread.sleep(timeDelay);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }

                ChooseCell(finalX, finalY);
            }
        };

        thread.start();
    }

    private int randInt(int min, int max) {
        Random rand = new Random();
        int randomNum = rand.nextInt((max - min) + 1) + min;
        return randomNum;
    }
}
