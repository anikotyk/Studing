package com.example.battleship;

import java.util.ArrayList;

public class BattleFieldPlayer extends  BattleField {
    public BattleFieldPlayer(GameActivity mainActivity, FieldSettings fieldSettings, ArrayList<ArrayList<FieldCell>> field){
        super(mainActivity, fieldSettings, field);
        isPlayer = true;
    }

    @Override
    protected void ActionOnWin(){
        mainActivity.Win(true);
    }
}
