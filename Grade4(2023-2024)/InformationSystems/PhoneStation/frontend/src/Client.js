export default class Client{
    id;
    isConfirmed;
    isBanned;
    phonenumber;
    email;

    constructor(id, isConfirmed, isBanned, phonenumber, email) {
        this.id = id;
        this.isConfirmed = isConfirmed;
        this.isBanned = isBanned;
        this.phonenumber = phonenumber;
        this.email = email;
    }

    static from(json){
        return Object.assign(new Client(), json);
    }
}