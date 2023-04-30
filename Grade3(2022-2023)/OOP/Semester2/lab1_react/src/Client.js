export default class Client{
    getUserInfo(){
        var params = 'clientId=2'
        var data = new FormData();
        data.append('clientId', "2");
        let result = new Promise((resolve, reject)=>{
            let request = new XMLHttpRequest();
            request.open("POST", "http://localhost:8080/lab1_server_war_exploded/confirm-client-servlet");
            request.onreadystatechange=()=>{
                if(request.readyState == 4 && request.status == 200){
                    resolve(request.responseText);
                }
            }
            request.send(data);
        });
        return result;
    }
    numbers(){
        let result = new Promise((resolve, reject)=>{
            let request = new XMLHttpRequest();
            request.open("GET", "http://localhost:8080/lab1_server_war_exploded/number-resource");
            request.onreadystatechange=()=>{
                if(request.readyState == 4 && request.status == 200){
                    resolve(request.responseText);
                }
            }
            request.send();
        });
        return result;
    }
}