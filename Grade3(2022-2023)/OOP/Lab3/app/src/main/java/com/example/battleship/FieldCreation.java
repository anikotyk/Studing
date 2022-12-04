package com.example.battleship;

import java.util.ArrayList;

public class FieldCreation {
    private FieldCreationActivity activity;
    private FieldSettings fieldSettings;
    private ArrayList<ArrayList<FieldCell>> field;
    private boolean isFieldLocked;

    private int chosenCellsCount;

    public FieldCreation(FieldCreationActivity activity, FieldSettings fieldSettings){
        this.activity = activity;
        this.fieldSettings = fieldSettings;
        this.chosenCellsCount = 0;
        this.isFieldLocked = false;

        CreateField();
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
        FieldCell cell = field.get(x).get(y);
        if(cell.GetType() == FieldCell.CellType.Empty){
            if(this.isFieldLocked) { return; }
            cell.Choose();
            activity.ChangeCellImage(x, y, FieldCell.CellImageType.Ship);
            chosenCellsCount++;
        }else{
            cell.Unchoose();
            activity.ChangeCellImage(x, y, FieldCell.CellImageType.Simple);
            chosenCellsCount--;
        }

        CheckIsFieldReady();
    }

    private void CheckIsFieldReady(){
        if(this.chosenCellsCount == fieldSettings.maxChosenCellsCount){
            this.isFieldLocked = true;
            activity.OnFieldCreated();
        }else{
            this.isFieldLocked = false;
            activity.OnFieldNotCreated();
        }
    }

    public ArrayList<ArrayList<FieldCell>> GetField(){
        return field;
    }
}
