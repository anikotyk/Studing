package com.example.battleship;

import java.util.ArrayList;

public class BattleFieldOld {
    private ArrayList<ArrayList<FieldCell>> field;
    private ArrayList<Ship> ships;
    private FieldSettings settings;
    private int chosenCellsCount;

    public BattleFieldOld(FieldSettings settings){
        this.settings = settings;

        this.field = new ArrayList<ArrayList<FieldCell>>();
        this.chosenCellsCount = 0;

        for(int i = 0; i < this.settings.fieldSize; i++){
            field.add(new ArrayList<FieldCell>());
            for(int j = 0; j < this.settings.fieldSize; j++){
               // field.get(i).add(new FieldCell());
            }
        }
    }

    public void ChooseCell(int x, int y){
        if(chosenCellsCount < settings.maxChosenCellsCount){
            if(field.get(x).get(y).IsAveliable()){
                chosenCellsCount++;
                field.get(x).get(y).Choose();
                ChangeLockStateNearPoint(x, y, true);
            }
        }
    }

    public void UnchooseCell(int x, int y){
        if(field.get(x).get(y).GetType().equals(FieldCell.CellType.Ship)){
            chosenCellsCount--;
            field.get(x).get(y).Unchoose();
            ChangeLockStateNearPoint(x, y, false);
        }
    }

    private void ChangeLockStateNearPoint(int x, int y, boolean isLock){
        field.get(x).get(y).ChangeLockState(isLock);

        if(x > 0){
            if(y > 0){
                field.get(x-1).get(y-1).ChangeLockState(isLock);
            }
            if(y < settings.fieldSize - 1){
                field.get(x-1).get(y+1).ChangeLockState(isLock);
            }
        }

        if(x < settings.fieldSize - 1){
            if(y > 0){
                field.get(x+1).get(y-1).ChangeLockState(isLock);
            }
            if(y < settings.fieldSize - 1){
                field.get(x+1).get(y+1).ChangeLockState(isLock);
            }
        }
    }

    private void ChangeLockStateNearShip(Ship ship, boolean isLock){
        for (int i = 0; i < ship.cells.size(); i++){
            int x = ship.cells.get(i).x;
            int y = ship.cells.get(i).y;

            if(x > 0){
                if(field.get(x-1).get(y).GetType().equals(FieldCell.CellType.Empty)){
                    field.get(x-1).get(y).ChangeLockState(isLock);
                }
            }

        }
    }

}
