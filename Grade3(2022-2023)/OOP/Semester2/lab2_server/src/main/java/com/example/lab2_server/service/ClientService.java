package com.example.lab2_server.service;

import com.example.lab2_server.dao.ClientDao;
import com.example.lab2_server.model.Client;
import org.springframework.stereotype.Service;

@Service
public class ClientService {
    private final ClientDao clientDao;

    public ClientService(ClientDao clientDao){
        this.clientDao = clientDao;
    }

    public int addClient(Client client){
        client.setEmail("anna5");
        clientDao.save(client);
        return 0;
    }
}

