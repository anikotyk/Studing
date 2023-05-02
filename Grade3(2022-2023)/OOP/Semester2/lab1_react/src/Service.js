export default class Service{
    id;
    price;
    name;

    constructor(id, price, name) {
        this.id = id;
        this.price = price;
        this.name = name;
    }

    static from(json){
        return Object.assign(new Service(), json);
    }
}