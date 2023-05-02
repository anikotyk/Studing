import React, {useState} from "react";
import {useAuth0} from "@auth0/auth0-react";
import UserPage from "./UserPage";
import AdminPage from "./AdminPage";
import {getClient, isAdmin} from "../DBRequests";
import AddPhoneNumberPage from "./AddPhoneNumberPage";

const PageContent = ()=>{
    const { user, isAuthenticated , isLoading} = useAuth0();
    let [isUserAdmin, setIsUserAdmin] = useState(null);
    let [clientInfo, setClientInfo] = useState(null);

    if(isAuthenticated){
        let email = user.email;
        email = "assssn";
        if(isUserAdmin==null){
            isAdmin(email).then(r =>{
                setIsUserAdmin(r);
            });
        }else if(!isUserAdmin && clientInfo == null){
            getClient(email).then(r=>{
                setClientInfo(r)
            })
        }
    }

    if(isUserAdmin == null) return <div>Loading...</div>

    if(isUserAdmin) return <AdminPage/>

    if(clientInfo==null) return <div>Loading...</div>

    if(clientInfo.phonenumber == 0) return <AddPhoneNumberPage clientId={clientInfo.id}/>

    if(!clientInfo.isConfirmed) return <div>Your account isn't confirmed yet</div>

    if(clientInfo.isBanned) return <div>Your account is banned</div>

    return <UserPage client={clientInfo}/>
}

export default PageContent;