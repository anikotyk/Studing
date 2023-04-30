import React, {useState} from "react";
import {useAuth0} from "@auth0/auth0-react";
import UserPage from "./UserPage";
import AdminPage from "./AdminPage";
import {isAdmin} from "../DBRequests";

const PageContent = ()=>{
    const { user, isAuthenticated , isLoading} = useAuth0();
    let [isUserAdmin, setIsUserAdmin] = useState(null);

    if(isAuthenticated){
        //isAdmin(user.email).then(r =>{
        isAdmin("an").then(r =>{
            setIsUserAdmin(r);
        });
    }

    if(isUserAdmin == null) return <div>Loading...</div>

    if(isUserAdmin) return <AdminPage/>
    else return <UserPage/>
}

export default PageContent;