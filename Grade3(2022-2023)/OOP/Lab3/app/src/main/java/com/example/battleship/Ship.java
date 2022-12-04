package com.example.battleship;

import java.io.Serializable;

public class Ship implements Serializable {
    public int xStart;
    public int yStart;

    public int size;

    public int xDirection;
    public int yDirection;

    public Ship(int xStart, int yStart, int xDirection, int yDirection, int size){
        this.xStart = xStart;
        this.yStart = yStart;
        this.size = size;
        this.xDirection = xDirection;
        this.yDirection = yDirection;
    }

    public void Rotate(){
        if(xDirection == -1 && yDirection==0){
            xDirection = 0;
            yDirection = 1;
        }else if(xDirection == 0 && yDirection==1){
            xDirection = 1;
            yDirection = 0;
        }else if(xDirection == 1 && yDirection==0){
            xDirection = 0;
            yDirection = -1;
        }else{
            xDirection = -1;
            yDirection = 0;
        }
    }
}
