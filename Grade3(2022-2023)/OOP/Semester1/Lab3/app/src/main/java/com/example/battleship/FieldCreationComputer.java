package com.example.battleship;

import java.util.Random;

public class FieldCreationComputer extends FieldCreation{
    public FieldCreationComputer(FieldCreationActivity activity, FieldSettings fieldSettings){
        super(activity,fieldSettings );
        this.isShowChanges = false;
    }

    public void FillField(){
        while(chosenCellsCount < fieldSettings.maxChosenCellsCount){
            PasteShip();
        }
    }

    private void PasteShip(){
        int x = randInt(0, fieldSettings.fieldSize-1);
        int y = randInt(0, fieldSettings.fieldSize-1);
        FieldCell cell = field.get(x).get(y);

        if(!cell.IsAveliable()){
            return;
        }

        if(cell.GetType() == FieldCell.CellType.Empty){
            int rotation = randInt(0, 3);

            TryPositionShip(x, y, currentShipSize);

            Ship ship = cell.GetShip();
            if(ship!=null){
                for(int i = 0; i < rotation; i++){
                     RotateShip(ship);
                }
            }
        }
    }

    private int randInt(int min, int max) {
        Random rand = new Random();
        int randomNum = rand.nextInt((max - min) + 1) + min;
        return randomNum;
    }
}
