package com.example.lab2_server.api;

import com.example.lab2_server.service.ClientsServiceService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RequestMapping("")
@RestController
public class ClientsServiceController {
    private final ClientsServiceService clientsServiceService;

    @Autowired
    public ClientsServiceController(ClientsServiceService clientsServiceService){
        this.clientsServiceService = clientsServiceService;
    }
}
