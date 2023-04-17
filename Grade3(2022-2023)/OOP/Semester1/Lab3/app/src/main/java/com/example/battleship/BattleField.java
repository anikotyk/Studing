package com.example.battleship;

import java.util.ArrayList;

public abstract class BattleField {
    protected GameActivity mainActivity;
    protected FieldSettings fieldSettings;
    protected ArrayList<ArrayList<FieldCell>> field;
    protected int chosenShipCellsCount;

    protected boolean isPlayer;

    protected boolean isFieldLocked;

    public BattleField(GameActivity mainActivity, FieldSettings fieldSettings, ArrayList<ArrayList<FieldCell>> field){
        this.mainActivity = mainActivity;
        this.fieldSettings = fieldSettings;

        this.isFieldLocked = true;
        this.chosenShipCellsCount = 0;

        this.field = field;
    }

    public void ChooseCell(int x, int y){
        if(isFieldLocked){ return; }

        FieldCell cell = field.get(x).get(y);
        if(cell.IsHidden()){ //check that cell wasn't used yet
            cell.CellIsShown();
            if(cell.GetType() == FieldCell.CellType.Empty){
                mainActivity.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        mainActivity.ChangeCellImage(x, y, FieldCell.CellImageType.Empty, isPlayer);
                    }
                });
                GiveTurnToSecondPlayer();
            }else{
                chosenShipCellsCount++;
                CheckForWin();
                mainActivity.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        mainActivity.ChangeCellImage(x, y, FieldCell.CellImageType.Ship, isPlayer);
                    }
                });
                if(IsShipDead(cell.GetShip())){
                    ShowCellsNearShip(cell.GetShip());
                }
            }
        }
    }

    public boolean IsShipDead(Ship ship){
        int x = ship.xStart;
        int y = ship.yStart;

        for(int i = 0; i < ship.size; i++){
            FieldCell cell = field.get(x).get(y);
            if(cell.IsHidden()){
                return false;
            }
            x += ship.xDirection;
            y += ship.yDirection;
        }
        return true;
    }

    public int ShipShownCellsCount(Ship ship){
        int x = ship.xStart;
        int y = ship.yStart;
        int cnt = 0;

        for(int i = 0; i < ship.size; i++){
            FieldCell cell = field.get(x).get(y);
            if(!cell.IsHidden()){
                cnt++;
            }
            x += ship.xDirection;
            y += ship.yDirection;
        }
        return cnt;
    }

    private void GiveTurnToSecondPlayer(){
        this.isFieldLocked = true;
        mainActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                mainActivity.NextTurn();
            }
        });
    }

    private void CheckForWin(){
        if(fieldSettings.maxChosenCellsCount == this.chosenShipCellsCount){
            ActionOnWin();
        }
    }

    protected void ActionOnWin(){
        this.isFieldLocked = true;
        mainActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                mainActivity.Win(isPlayer);
            }
        });
    }

    public void GetTurn(){
        isFieldLocked = false;
    }

    protected void ShowCellsNearShip(Ship ship){
        int x = ship.xStart;
        int y = ship.yStart;

        for(int i = 0; i < ship.size; i++){
            ShowCell(x-1, y-1);
            ShowCell(x-1, y);
            ShowCell(x-1, y+1);
            ShowCell(x, y-1);
            ShowCell(x, y);
            ShowCell(x, y+1);
            ShowCell(x+1, y-1);
            ShowCell(x+1, y);
            ShowCell(x+1, y+1);

            x += ship.xDirection;
            y += ship.yDirection;
        }
    }

    protected void ShowCell(int x, int y){
        if(x < 0 || y < 0 || x > fieldSettings.fieldSize-1 || y > fieldSettings.fieldSize-1){
            return;
        }
        if(field.get(x).get(y).GetType() != FieldCell.CellType.Ship){
            field.get(x).get(y).CellIsShown();
            mainActivity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    mainActivity.ChangeCellImage(x, y, FieldCell.CellImageType.Empty, isPlayer);
                }
            });

        }
    }
}
