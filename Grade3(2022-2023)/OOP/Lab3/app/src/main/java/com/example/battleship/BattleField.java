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
        //CreateField();
    }

    private void CreateField(){
        this.field = new ArrayList<ArrayList<FieldCell>>();

        for(int i = 0; i < this.fieldSettings.fieldSize; i++){
            field.add(new ArrayList<FieldCell>());
            for(int j = 0; j < this.fieldSettings.fieldSize; j++){
                field.get(i).add(new FieldCell(i, j));
            }
        }
    }

    public void ChooseCell(int x, int y){
        if(isFieldLocked){ return; }

        FieldCell cell = field.get(x).get(y);
        if(cell.GetState()){ //check that cell wasn't used yet
            if(cell.GetType() == FieldCell.CellType.Empty){
                mainActivity.ChangeCellImage(x, y, FieldCell.CellImageType.Empty, isPlayer);
                GiveTurnToSecondPlayer();
            }else{
                chosenShipCellsCount++;
                CheckForWin();
                mainActivity.ChangeCellImage(x, y, FieldCell.CellImageType.Ship, isPlayer);
            }
        }
    }

    private void GiveTurnToSecondPlayer(){
        this.isFieldLocked = true;
        mainActivity.NextTurn();
    }

    private void CheckForWin(){
        if(fieldSettings.maxChosenCellsCount == this.chosenShipCellsCount){
            ActionOnWin();
        }
    }

    protected void ActionOnWin(){
        //mainActivity.Win();
    }

    public void GetTurn(){
        isFieldLocked = false;
    }
}
