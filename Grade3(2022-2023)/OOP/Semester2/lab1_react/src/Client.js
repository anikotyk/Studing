export default class Client{
    getUserInfo(){
        let result = new Promise((resolve, reject)=>{
            let request = new XMLHttpRequest();
            request.open("GET", "http://localhost:8080/Lab1_v3_war_exploded/number-resource");
            request.onreadystatechange=()=>{
                if(request.readyState == 4 && request.status == 200){
                    resolve(request.responseText);
                }
            }
            request.send();
        });
        return result;
    }
    numbers(){
        let result = new Promise((resolve, reject)=>{
            let request = new XMLHttpRequest();
            request.open("GET", "http://localhost:8080/Lab1_v3_war_exploded/number-resource");
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