import React, {useState} from "react";
import {useAuth0} from "@auth0/auth0-react";
import UserPage from "./UserPage";
import AdminPage from "./AdminPage";
import {getClient, isAdmin} from "../DBRequests";
import AddPhoneNumberPage from "./AddPhoneNumberPage";
import LoadingComponent from "./LoadingComponent";
import BanComponent from "./BanComponent";
import NotConfirmedComponent from "./NotConfirmedComponent";

const PageContent = ()=>{
    const { user} = useAuth0();
    let [isUserAdmin, setIsUserAdmin] = useState(null);
    let [clientInfo, setClientInfo] = useState(null);

    let email = user.email;

    if(isUserAdmin==null){
        isAdmin(email).then(r =>{
            setIsUserAdmin(r);
        });
    }else if(!isUserAdmin && clientInfo == null){
        getClient(email).then(r=>{
            setClientInfo(r)
        })
    }

    if(isUserAdmin == null) return <LoadingComponent/>

    if(isUserAdmin) return <AdminPage/>

    if(clientInfo==null) return <LoadingComponent/>

    if(clientInfo.phonenumber == 0) return <AddPhoneNumberPage clientId={clientInfo.id}/>

    if(!clientInfo.isConfirmed) return <NotConfirmedComponent/>

    if(clientInfo.isBanned) return <BanComponent/>

    return <UserPage client={clientInfo}/>
}

export default PageContent;