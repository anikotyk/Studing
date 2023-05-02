import React, {useState} from "react";
import {
    addServiceToClient,
    addUserPayment,
    banClient,
    confirmClient,
    getAllClients,
    getAllServices, getUserUnpaidServices, unbanClient
} from "../DBRequests";
import Client from "../Client";
import Service from "../Service";

function ShowClientUnpaidServices(elementId, clientId){
    var elem = document.getElementById(elementId);
    getUserUnpaidServices(clientId).then(r=>{
        let services = r;
        let servicesArray = [];
        for(let i = 0; i < services.length; i++){
            var service = Service.from(services[i]);
            elem.innerHTML += "<div>"+service.id+" "+service.name+" "+service.price+"</div>"
        }
    });
}
const AdminPage = ()=>{
    let [allClients, setAllClients] = useState(null);

    if(allClients==null){
        getAllClients().then(r=>{
            let clients = r;
            let clientsArray = [];
            for(let i = 0; i < clients.length; i++){
                clientsArray.push(Client.from(clients[i]));
            }
            setAllClients(clientsArray);
        });
    }

    if(allClients==null) return <div>Admin Page</div>

    const listUsers = allClients.map(client =>{
            const isConfirmed = client.isConfirmed;
            const isBanned = client.isBanned;

            let confirmDiv;
            let banDiv;

            if(!isConfirmed){
                confirmDiv = <button onClick={()=> confirmClient(client.id).then((r)=>{window.location.reload(false);})}> Confirm </button>;
            }
            else{
                confirmDiv = <div> Confirmed </div>
                if(!isBanned){
                    banDiv = <button onClick={()=> banClient(client.id).then((r)=>{window.location.reload(false);})}> Ban </button>;
                }else{
                    banDiv = <button onClick={()=> unbanClient(client.id).then((r)=>{window.location.reload(false);})}> Unban </button>;
                }
            }

            let elemId = "client"+client.id;
            let servicesButton = <button onClick={()=>ShowClientUnpaidServices(elemId, client.id)}>Show unpaid Services</button>

            let elem = <li id={elemId}> {client.id} {client.email} {client.phonenumber} {servicesButton} {confirmDiv} {banDiv} </li>;

            return elem;
        }
    );


    return (
        <div>
            <p>Admin Page </p>
            <ul>
                {listUsers}
            </ul>
        </div>
    )
}

export default AdminPage;