import React, {useState} from "react";
import {
    banClient,
    confirmClient,
    getAllClients,
    getUserUnpaidServices, unbanClient
} from "../DBRequests";
import Client from "../Client";
import Service from "../Service";
import {Button} from "react-bootstrap";
import ReactDomServer from "react-dom/server";
import LoadingComponent from "./LoadingComponent";

function formatPhoneNumber(phoneNumberString) {
    if(phoneNumberString.length!=10){ return phoneNumberString; }
    var cleaned = ('' + phoneNumberString).replace(/\D/g, '');
    var match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);
    if (match) {
        return '(' + match[1] + ') ' + match[2] + '-' + match[3];
    }
    return null;
}

function ShowClientUnpaidServices(elementId, clientId, servicesDivId){
    let servicesDiv = document.getElementById(servicesDivId);

    if(servicesDiv!=null){
        if(servicesDiv.style.display.toString().trim() != "none"){
            servicesDiv.style.display = "none";
        }else{
            servicesDiv.style.display = "block";
        }
    }else{
        CreateClientUnpaidServices(elementId, clientId, servicesDivId);
    }
}

function CreateClientUnpaidServices(clientServicesContainerId, clientId, servicesDivId){
    let clientServicesContainer = document.getElementById(clientServicesContainerId);

    getUserUnpaidServices(clientId).then(r=>{
        let services = r;
        let servicesArray = [];

        for(let i = 0; i < services.length; i++){
            let service = Service.from(services[i]);
            servicesArray.push(service);
        }

        const listServices = servicesArray.map(service =>{
            return <li className="list-group-item text-danger"><b>{service.price}$</b> {service.name}</li>
        });

        let servicesDiv = <div id={servicesDivId} className="list-group list-group-flush">
            <h8 className="text-danger">Unpaid services:</h8>
            {listServices}
        </div>

        if(servicesArray.length<=0){
            servicesDiv = <div id={servicesDivId} className="list-group list-group-flush">
                <h8 className="text-success">No unpaid services</h8>
            </div>
        }

        let servicesDivString = ReactDomServer.renderToString(servicesDiv);
        clientServicesContainer.innerHTML += servicesDivString;
    });
}

function HideClientUnpaidServices(servicesDivId){
    let servicesDiv = document.getElementById(servicesDivId);

    if(servicesDiv!=null){
        servicesDiv.style.display = "none";
    }
}

function GetUsersList(allClients){
    const listUsers = allClients.map(client =>{
            if(client.phonenumber==0){return;}
            const isConfirmed = client.isConfirmed;
            const isBanned = client.isBanned;

            let confirmBtn, banBtn, unbanBtn;

            let banBadge, conformBadge;

            let servicesShowButton;

            let clientServicesContainerId = "clientServicesContainer"+client.id;
            let servicesDivId = "services"+client.id;

            if(!isConfirmed){
                confirmBtn = <Button variant="success" onClick={()=> confirmClient(client.id).then((r)=>{window.location.reload(false);})}> Confirm </Button>;
            }
            else{
                conformBadge = <span className="badge rounded-pill bg-success">Confirmed</span>

                servicesShowButton = <Button variant="primary" onClick={()=>ShowClientUnpaidServices(clientServicesContainerId, client.id, servicesDivId)}>Info</Button>

                if(!isBanned){
                    banBtn = <Button variant="danger" onClick={()=> banClient(client.id).then((r)=>{window.location.reload(false);})}> Ban </Button>;
                }else{
                    banBadge = <span className="badge rounded-pill bg-danger">Banned</span>
                    unbanBtn = <Button variant="danger" onClick={()=> unbanClient(client.id).then((r)=>{window.location.reload(false);})}> Unban </Button>;
                }
            }

            let elem = <li className="list-group-item">
                <div className="d-flex justify-content-between align-items-center">
                    <div>
                        <div className="d-flex flex-client-info">
                            <div className="fw-bold">{formatPhoneNumber(client.phonenumber.toString())}</div>
                            <div>{conformBadge}</div>
                            <div>{banBadge}</div>
                        </div>
                        <div className="text-muted">{client.email}</div>
                    </div>
                    <div className="d-flex flex-client-info">
                        {confirmBtn}{banBtn}{unbanBtn}
                        {servicesShowButton}
                    </div>
                </div>
                <div id={clientServicesContainerId}>
                </div>
            </li>;

            return elem;
        }
    );

    return listUsers;
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
            clientsArray.sort((a, b) =>  b.id - a.id);
            setAllClients(clientsArray);
        });
    }

    if(allClients==null) return <LoadingComponent/>

    const listUsers = GetUsersList(allClients);

    return (
        <div>
            <h3><center>Clients</center></h3>
            <ul className="list-group list-group-light ul-clients shadow-sm p-3 mb-5 bg-white rounded">
                {listUsers}
            </ul>
        </div>
    )
}

export default AdminPage;