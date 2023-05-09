package com.example.lab2_server.service;

import com.example.lab2_server.dao.ClientDao;
import com.example.lab2_server.model.Client;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.stereotype.Service;

@Service
public class ClientService {
    private final ClientDao clientDao;

    @Autowired
    public ClientService(@Qualifier("postgres") ClientDao clientDao){
        this.clientDao = clientDao;
    }

    public int addClient(Client client){
        client.setEmail("anna1");
        clientDao.save(client);
        return 0;
    }
}
