import {setClientPhone} from "../DBRequests";

const AddPhoneNumberPage = ({clientId})=>{
    let inputField = <input type="number" id="phone" />;
    let setButton = <button id="setButton" onClick={()=>SetPhoneNumber(clientId)}> Set </button>;
    return <div>Enter your phone number: {inputField} {setButton}</div>;
}

function SetPhoneNumber(clientId){
    var inputField = document.getElementById("phone");
    if(inputField!=null && inputField.value.trim() != ""){
        console.log(clientId+" "+inputField.value);
        document.getElementById("setButton").style.display = 'none';
        setClientPhone(clientId, inputField.value).then(r=>{
            window.location.reload(false);
        })
    }
}

export default AddPhoneNumberPage;