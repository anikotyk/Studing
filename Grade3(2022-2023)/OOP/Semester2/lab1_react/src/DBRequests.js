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