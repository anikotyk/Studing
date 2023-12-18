import Client from "./Client";

export let isAdmin = async (email, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/is-admin", {
            method: "POST",
            body: JSON.stringify({
                clientEmail: email,
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            return resText.valueOf().trim()=="true";
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let getAllServices = async (e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/get-all-services", {
            method: "GET"
        });
        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return jsonObject;
        } else {
            console.log("Some error occurred");
        }
    } catch (err) {
        console.log(err);
    }
}

export let addServiceToClient = async (clientId, serviceId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/add-service-to-client", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId,
                serviceId: serviceId
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            return resText.valueOf().trim()=="true";
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let addUserPayment = async (clientId, serviceId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/add-client-payment", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId,
                serviceId: serviceId
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            return resText.valueOf().trim()=="true";
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}


export let getUserAddedServices = async (clientId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/get-all-client-added-services", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId,
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return jsonObject;
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let getUserPaidServices = async (clientId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/get-all-client-paid-services", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId,
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return jsonObject;
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let getAllClients = async (e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/get-all-clients", {
            method: "GET"
        });
        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return jsonObject;
        } else {
            console.log("Some error occurred");
        }
    } catch (err) {
        console.log(err);
    }
}

export let confirmClient = async (clientId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/confirm-client", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            return resText.valueOf().trim()=="true";
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let banClient = async (clientId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/ban-client", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            return resText.valueOf().trim()=="true";
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let unbanClient = async (clientId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/unban-client", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            return resText.valueOf().trim()=="true";
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let getClient = async (email, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/get-client", {
            method: "POST",
            body: JSON.stringify({
                clientEmail: email,
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return Client.from(jsonObject);
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let getUserUnpaidServices = async (clientId, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/get-all-client-unpaid-services", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId,
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return jsonObject;
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}

export let setClientPhone = async (clientId, clientPhone, e) => {
    try {
        let res = await fetch("http://localhost:8080/lab1_server_war_exploded/set-client-phone", {
            method: "POST",
            body: JSON.stringify({
                clientId: clientId,
                clientPhone: clientPhone
            }),
        });

        if (res.status === 200) {
            let resText = await res.text();
            let jsonObject = JSON.parse(resText);
            return jsonObject;
        } else {
            console.log("Some error occurred.");
        }
    } catch (err) {
        console.log(err);
    }
}