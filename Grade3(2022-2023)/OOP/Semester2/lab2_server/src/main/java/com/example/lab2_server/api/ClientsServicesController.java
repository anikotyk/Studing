package com.example.lab2_server.api;

import com.example.lab2_server.service.ClientsServicesService;
import org.json.JSONException;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RequestMapping("")
@RestController
public class ClientsServicesController {
    private final ClientsServicesService clientsServiceService;

    @Autowired
    public ClientsServicesController(ClientsServicesService clientsServiceService){
        this.clientsServiceService = clientsServiceService;
    }

    @PostMapping("/add-service-to-client")
    public void addClientService(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        int clientId = obj.getInt("clientId");
        int serviceId = obj.getInt("serviceId");

        clientsServiceService.addClientService(clientId, serviceId);
    }
}
