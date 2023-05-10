package com.example.lab2_server.service;

import com.example.lab2_server.dao.ClientsServiceDao;
import org.springframework.stereotype.Service;

@Service
public class ClientsServiceService {
    private final ClientsServiceDao clientsServiceDao;

    public ClientsServiceService(ClientsServiceDao clientsServiceDao){
        this.clientsServiceDao = clientsServiceDao;
    }
}

