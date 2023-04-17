package com.example.battleship;

import java.util.ArrayList;
import java.util.Random;

public class BattleFieldComputer extends BattleField{
    private int timeDelay = 1000;
    private boolean isNearShip = false;
    private Ship lastShip;
    private ArrayList<FieldCell> lastShipCells;

    public BattleFieldComputer(GameActivity mainActivity, FieldSettings fieldSettings, ArrayList<ArrayList<FieldCell>> field){
        super(mainActivity, fieldSettings, field);
        isPlayer = false;
        lastShipCells = new ArrayList<FieldCell>();
    }

    @Override
    public void GetTurn() {
        isFieldLocked = false;
        if(isNearShip){
            ChooseCellNearShipCell();
        }else{
            ChooseCellRandomly();
        }
    }

    private void ChooseCellRandomly(){
        int x = randInt(0, fieldSettings.fieldSize-1);
        int y = randInt(0, fieldSettings.fieldSize-1);

        while(!field.get(x).get(y).IsHidden()){
            x = randInt(0, fieldSettings.fieldSize-1);
            y = randInt(0, fieldSettings.fieldSize-1);
        }

        ChooseCellThread(x, y);
    }

    private boolean IsCanChooseCell(int x, int y){
        if(x < 0 || y < 0 || x > fieldSettings.fieldSize-1 || y > fieldSettings.fieldSize-1){
            return false;
        }

        if(field.get(x).get(y).IsHidden()){
            return true;
        }

        return false;
    }

    private void ChooseCellNearShipCell(){
        int x = 0;
        int y = 0;

        if(lastShipCells.size() > 1){
            if(lastShip.xDirection == 0){
                //horizontal ship
                for(int i = 0; i < lastShipCells.size(); i++){
                    FieldCell cell = lastShipCells.get(i);
                    if(IsCanChooseCell(cell.x, cell.y-1)){
                        x = cell.x;
                        y = cell.y - 1;
                        break;
                    }else if(IsCanChooseCell(cell.x, cell.y+1)){
                        x = cell.x;
                        y = cell.y + 1;
                        break;
                    }
                }
            }else{
                //vertical ship
                for(int i = 0; i < lastShipCells.size(); i++){
                    FieldCell cell = lastShipCells.get(i);
                    if(IsCanChooseCell(cell.x-1, cell.y)){
                        x = cell.x - 1;
                        y = cell.y;
                        break;
                    }else if(IsCanChooseCell(cell.x+1, cell.y)){
                        x = cell.x + 1;
                        y = cell.y;
                        break;
                    }
                }
            }
        }else{
            FieldCell cell = lastShipCells.get(0);
            if(IsCanChooseCell(cell.x-1, cell.y)){
                x = cell.x-1;
                y = cell.y;
            }else if(IsCanChooseCell(cell.x, cell.y+1)){
                x = cell.x;
                y = cell.y + 1;
            }else if(IsCanChooseCell(cell.x+1, cell.y)){
                x = cell.x + 1;
                y = cell.y;
            }else if(IsCanChooseCell(cell.x, cell.y-1)){
                x = cell.x;
                y = cell.y - 1;
            }
        }

        ChooseCellThread(x, y);
    }

    private void ChooseCellThread(int x, int y){
        Thread thread = new Thread() {
            @Override
            public void run() {
                try {
                    Thread.sleep(timeDelay);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }

                ChooseCell(x, y);
                FieldCell cell = field.get(x).get(y);
                if(cell.GetType() == FieldCell.CellType.Ship){
                    Ship ship = cell.GetShip();
                    if(!IsShipDead(ship)){
                        isNearShip = true;
                        lastShipCells.add(cell);
                        lastShip = ship;
                        GetTurn();
                    }else{
                        lastShipCells = new ArrayList<FieldCell>();
                        isNearShip = false;
                        GetTurn();
                    }
                }
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
