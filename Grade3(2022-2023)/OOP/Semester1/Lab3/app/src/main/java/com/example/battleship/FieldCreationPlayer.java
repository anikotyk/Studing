package com.example.battleship;

public class FieldCreationPlayer extends FieldCreation{
    private boolean isFieldLocked;

    private boolean isDeletionMode = false;

    public FieldCreationPlayer(FieldCreationActivity activity, FieldSettings fieldSettings){
        super(activity, fieldSettings);
        this.isFieldLocked = false;
        this.isShowChanges = true;
    }

    public void DeletionMode(){
        isDeletionMode = !isDeletionMode;
        activity.ChangeDeletionState(isDeletionMode);
    }

    private void DisableDeletionMode(){
        isDeletionMode = false;
        activity.ChangeDeletionState(isDeletionMode);
    }

    public void ChooseCell(int x, int y){
        FieldCell cell = field.get(x).get(y);
        if(!cell.IsAveliable()){
            DisableDeletionMode();
            return;
        }

        if(isDeletionMode){
            DisableDeletionMode();
            if(cell.GetType() == FieldCell.CellType.Ship){
                RemoveShip(cell.GetShip());
            }
        }else{
            if(cell.GetType() == FieldCell.CellType.Empty){
                if(this.isFieldLocked) { return; }
                TryPositionShip(x, y, currentShipSize);
            }else{
                RotateShip(cell.GetShip());
            }
        }

        CheckIsFieldReady();
    }

    @Override
    protected void CheckIsFieldReady(){
        if(this.chosenCellsCount >= fieldSettings.maxChosenCellsCount){
            this.isFieldLocked = true;
            activity.OnFieldCreated();
        }else{
            this.isFieldLocked = false;
            activity.OnFieldNotCreated();
        }
    }
}
