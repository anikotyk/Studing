import {setClientPhone} from "../DBRequests";
import {Button} from "react-bootstrap";

const AddPhoneNumberPage = ({clientId})=>{
    let inputFieldId ="phone";

    return <div className="form-group centered shadow-sm p-3 mb-5 bg-white rounded">
        <label htmlFor="inputNumber">Phone number</label>
        <div className="d-flex flex-client-info">
            <input type="number" className="form-control" id={inputFieldId} aria-describedby="phoneHelp"
               placeholder="Enter phone number"/>
            <Button variant="primary" id="setButton" onClick={()=>SetPhoneNumber(clientId, inputFieldId)}> Enter </Button>
        </div>
        <small id="phoneHelp" className="form-text text-muted">Write number without any symbols or spaces</small>
    </div>
}

function SetPhoneNumber(clientId, inputFieldId){
    var inputField = document.getElementById(inputFieldId);
    if(inputField!=null && inputField.value.trim() != ""){
        setClientPhone(clientId, inputField.value).then(r=>{
            window.location.reload(false);
        })
    }
}

export default AddPhoneNumberPage;