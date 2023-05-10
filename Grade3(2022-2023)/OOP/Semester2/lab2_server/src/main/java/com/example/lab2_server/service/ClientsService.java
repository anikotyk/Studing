package com.example.lab2_server.service;

import com.example.lab2_server.dao.ClientDao;
import com.example.lab2_server.model.Client;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ClientsService {
    private final ClientDao clientDao;

    public ClientsService(ClientDao clientDao){
        this.clientDao = clientDao;
    }

    public void banClient(Long clientId){
        Client client = clientDao.findById(clientId).get();
        if(client!=null){
            client.setBanned(true);
            clientDao.save(client);
        }
    }

    public void confirmClient(Long clientId){
        Client client = clientDao.findById(clientId).get();
        if(client!=null){
            client.setConfirmed(true);
            clientDao.save(client);
        }
    }

    public List<Client> getAllClients(){
        List<Client> clients = clientDao.findAll();
        return clients;
    }

    public Client getClient(String clientEmail){
        List<Client> clients = clientDao.findAllByEmail(clientEmail);
        Client client;
        if(clients.size()<1){
            addNewClient(clientEmail);
            client = clientDao.findAllByEmail(clientEmail).get(0);
        }else{
            client = clients.get(0);
        }
        return client;
    }

    public void addNewClient(String clientEmail){
        Client client = new Client();
        client.setEmail(clientEmail);
        clientDao.save(client);
    }

    public void setClientPhone(Long clientId, long clientPhone){
        Client client = clientDao.findById(clientId).get();
        client.setPhonenumber(clientPhone);
        clientDao.save(client);
    }

    public void unbanClient(Long clientId){
        Client client = clientDao.findById(clientId).get();
        if(client!=null){
            client.setBanned(false);
            clientDao.save(client);
        }
    }
}

