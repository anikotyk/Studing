package com.example.battleship;

import java.util.ArrayList;

public abstract class FieldCreation {
    protected FieldCreationActivity activity;
    protected FieldSettings fieldSettings;
    protected ArrayList<ArrayList<FieldCell>> field;
    protected int currentShipSize = 4;

    protected int ship4count = 0;
    protected int ship3count = 0;
    protected int ship2count = 0;
    protected int ship1count = 0;

    protected int chosenCellsCount;

    protected boolean isShowChanges;


    public FieldCreation(FieldCreationActivity activity, FieldSettings fieldSettings){
        this.activity = activity;
        this.fieldSettings = fieldSettings;
        this.chosenCellsCount = 0;

        CreateField();
    }

    protected void CreateField(){
        this.field = new ArrayList<ArrayList<FieldCell>>();

        for(int i = 0; i < this.fieldSettings.fieldSize; i++){
            field.add(new ArrayList<FieldCell>());
            for(int j = 0; j < this.fieldSettings.fieldSize; j++){
                field.get(i).add(new FieldCell(i, j));
            }
        }
    }

    protected void TryPositionShip(int x, int y, int size){
        Ship ship = new Ship(x, y, -1, 0, size);

        for(int i = 0; i < 4; i++){
            if(CheckCanPosition(ship)){
                PositionShip(ship);
                break;
            }

            ship.Rotate();
        }
    }

    protected boolean CheckCanPosition(Ship ship){
        int cnt = 1;
        int x = ship.xStart;
        int y = ship.yStart;

        while(cnt<ship.size){
            x += ship.xDirection;
            y += ship.yDirection;
            if(x < 0 || y < 0 || x > fieldSettings.fieldSize-1 || y > fieldSettings.fieldSize-1){
                return false;
            }

            FieldCell cell = field.get(x).get(y);

            if(cell.GetType() == FieldCell.CellType.Ship){
                return false;
            }

            if(!cell.IsAveliable()){
                return false;
            }

            cnt++;
        }

        return true;
    }

    protected void PositionShip(Ship ship){
        UpdateShipsCount(ship, true);

        chosenCellsCount += ship.size;
        int x = ship.xStart;
        int y = ship.yStart;

        for(int i = 0; i < ship.size; i++){
            FieldCell cell = field.get(x).get(y);

            cell.Choose();
            cell.SetShip(ship);

            if(isShowChanges){
                activity.ChangeCellImage(x, y, FieldCell.CellImageType.Ship);
            }

            x += ship.xDirection;
            y += ship.yDirection;
        }

        LockUnlockCellsNearShip(ship, true);
    }

    protected void RemoveShip(Ship ship){
        LockUnlockCellsNearShip(ship, false);
        UpdateShipsCount(ship, false);

        chosenCellsCount -= ship.size;
        int x = ship.xStart;
        int y = ship.yStart;

        for(int i = 0; i < ship.size; i++){
            FieldCell cell = field.get(x).get(y);
            cell.Unchoose();
            cell.SetShip(null);

            if(isShowChanges){
                activity.ChangeCellImage(x, y, FieldCell.CellImageType.Simple);
            }

            x += ship.xDirection;
            y += ship.yDirection;
        }
    }

    protected void RotateShip(Ship ship){
        RemoveShip(ship);

        while(true){
            ship.Rotate();
            if(CheckCanPosition(ship)){
                PositionShip(ship);
                break;
            }
        }
    }

    protected void LockUnlockCellsNearShip(Ship ship, boolean isLock){
        int x = ship.xStart;
        int y = ship.yStart;

        for(int i = 0; i < ship.size; i++){
            LockUnlockCell(x-1, y-1, isLock);
            LockUnlockCell(x-1, y, isLock);
            LockUnlockCell(x-1, y+1, isLock);
            LockUnlockCell(x, y-1, isLock);
            LockUnlockCell(x, y, isLock);
            LockUnlockCell(x, y+1, isLock);
            LockUnlockCell(x+1, y-1, isLock);
            LockUnlockCell(x+1, y, isLock);
            LockUnlockCell(x+1, y+1, isLock);

            x += ship.xDirection;
            y += ship.yDirection;
        }
    }

    protected void LockUnlockCell(int x, int y, boolean isLock){
        if(x < 0 || y < 0 || x > fieldSettings.fieldSize-1 || y > fieldSettings.fieldSize-1){
            return;
        }
        if(field.get(x).get(y).GetType() != FieldCell.CellType.Ship){
            field.get(x).get(y).ChangeLockState(isLock);
            if(isShowChanges){
                if( field.get(x).get(y).IsAveliable()){
                    activity.ChangeCellImage(x, y, FieldCell.CellImageType.Simple);
                }else{
                    activity.ChangeCellImage(x, y, FieldCell.CellImageType.Empty);
                }
            }
        }
    }

    protected void CheckIsFieldReady(){

    }

    public ArrayList<ArrayList<FieldCell>> GetField(){
        return field;
    }

    protected void UpdateShipsCount(Ship ship, boolean isAdded){
        int delta = 1;
        if(!isAdded){
            delta = -1;
        }
        if(ship.size == 4){
            ship4count += delta;
        }else if(ship.size == 3){
            ship3count += delta;
        }else if(ship.size == 2){
            ship2count += delta;
        }else if(ship.size == 1){
            ship1count += delta;
        }

        UpdateCurrentShipSize();
    }

    protected void UpdateCurrentShipSize(){
        if(ship4count<fieldSettings.ship4count){
            currentShipSize = 4;
        }else if(ship3count<fieldSettings.ship3count){
            currentShipSize = 3;
        }else if(ship2count<fieldSettings.ship2count){
            currentShipSize = 2;
        }else{
            currentShipSize = 1;
        }
    }

}
