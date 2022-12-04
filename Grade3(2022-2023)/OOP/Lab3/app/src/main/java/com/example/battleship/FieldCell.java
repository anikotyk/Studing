package com.example.battleship;


import java.io.Serializable;

public class FieldCell implements Serializable {
    private CellType cellType;
    private boolean isHidden;
    private int aveliableLock;
    public int x;
    public int y;

    private Ship ship;

    public FieldCell(int x, int y){
        this.cellType = CellType.Empty;
        this.isHidden = true;
        this.aveliableLock = 0;
        this.x = x;
        this.y = y;
    }

    public boolean IsHidden(){
        return isHidden;
    }

    public void Choose(){
        this.cellType = CellType.Ship;
    }

    public void Unchoose(){
        this.cellType = CellType.Empty;
    }

    public CellType GetType(){
        return this.cellType;
    }

    public void ChangeLockState(boolean isLock){
        if(isLock){
            this.aveliableLock++;
        }else{
            this.aveliableLock--;
        }
    }

    public void SetShip(Ship ship){
        this.ship = ship;
    }

    public Ship GetShip(){
        return ship;
    }

    public boolean IsAveliable(){
        return this.aveliableLock <= 0;
    }

    public void CellIsShown(){
        this.isHidden = false;
    }

    enum CellType {Ship, Empty}
    public enum CellImageType {Simple, Empty, Ship}
}
