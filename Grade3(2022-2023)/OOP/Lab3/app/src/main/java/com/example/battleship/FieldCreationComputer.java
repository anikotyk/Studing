package com.example.battleship;

import java.util.ArrayList;

public class FieldCreationComputer {
    private FieldCreationActivity activity;
    private FieldSettings fieldSettings;
    private ArrayList<ArrayList<FieldCell>> field;

    public FieldCreationComputer(FieldCreationActivity activity, FieldSettings fieldSettings){
        this.activity = activity;
        this.fieldSettings = fieldSettings;

        CreateField();
        FillField();
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

    private void FillField(){

    }

    public ArrayList<ArrayList<FieldCell>> GetField(){
        return field;
    }
}
