package com.example.battleship;

public class FieldSettings {
    public int fieldSize;
    public float fieldSizeOnScreen;
    public float fieldCreationSizeOnScreen;
    public int ship4count;
    public int ship3count;
    public int ship2count;
    public int ship1count;
    public int maxChosenCellsCount;

    public FieldSettings(){
        this.fieldSize = 10;
        this.fieldSizeOnScreen = 400f/720f;
        this.fieldCreationSizeOnScreen = 600f/720f;
        this.ship4count = 1;
        this.ship3count = 2;
        this.ship2count = 3;
        this.ship1count = 4;

        this.maxChosenCellsCount = this.ship1count + this.ship2count * 2 + this.ship3count * 3 + this.ship4count * 4;
    }
}
