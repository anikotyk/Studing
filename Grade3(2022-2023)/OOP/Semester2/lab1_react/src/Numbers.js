import React from 'react';
import Client from "./Client";

export default class Numbers extends React.Component{
    constructor(props) {
        super(props);
        this.client = new Client();
        this.state = {res:"" };

        this.client.getUserInfo().then((r)=>{
            this.setState({res:r});
        });
    }
    render() {
        return(
            <section>{
                <div>Server answers + {this.state.res}</div>
            }
            </section>);
    }
}