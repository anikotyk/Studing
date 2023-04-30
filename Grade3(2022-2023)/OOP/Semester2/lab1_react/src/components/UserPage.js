import React, {useState} from "react";
import {getAllServices} from "../DBRequests";

const UserPage = ()=>{
    let [allServices, setAllServices] = useState(null);

    getAllServices().then(r=>{
        let services = r;
        setAllServices(services);
    });

    if(allServices==null) return <div>User Page</div>
    return (
        <div>
            <p>User Page </p>

            <p>{allServices[0].get("price").toString()}</p>
        </div>
    )
}

export default UserPage;