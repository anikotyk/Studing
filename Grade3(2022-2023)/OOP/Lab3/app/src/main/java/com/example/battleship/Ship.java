package com.example.battleship;

import java.util.ArrayList;

public class Ship {
    public ArrayList<FieldCell> cells;

    public void Ship(ArrayList<FieldCell> cells){
        this.cells = cells;
    }

    public void Ship(){
        this.cells = new ArrayList<FieldCell>();
    }

    private void GetState(){
        for (int i = 0; i < cells.size(); i++){
            if(cells.get(i).GetState()){

            }
        }
    }

}
