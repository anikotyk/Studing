package com.example.backend.api;

import com.example.backend.model.Client;
import com.example.backend.service.ClientsService;
import com.google.gson.Gson;
import org.json.JSONException;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RequestMapping("")
@RestController
public class ClientsController {
    private final ClientsService clientService;

    @Autowired
    public ClientsController(ClientsService clientService){
        this.clientService = clientService;
    }

    @PostMapping("/ban-client")
    public void banClient(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        long clientId = obj.getLong("clientId");

        clientService.banClient(clientId);
    }

    @PostMapping("/confirm-client")
    public void confirmClient(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        long clientId = obj.getLong("clientId");

        clientService.confirmClient(clientId);
    }

    @GetMapping("/get-all-clients")
    public String getAllClients(){
        List<Client> clients = clientService.getAllClients();
        String json = new Gson().toJson(clients);
        return json;
    }

    @PostMapping("/get-client")
    public String getClient(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        String clientEmail = obj.getString("clientEmail");

        Client client = clientService.getClient(clientEmail);
        String json = new Gson().toJson(client);
        return json;
    }

    @PostMapping("/set-client-phone")
    public void setClientPhone(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);

        long clientId = obj.getLong("clientId");
        long clientPhone = obj.getLong("clientPhone");
        clientService.setClientPhone(clientId, clientPhone);
    }

    @PostMapping("/unban-client")
    public void unbanClient(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        long clientId = obj.getLong("clientId");
        clientService.unbanClient(clientId);
    }
}
