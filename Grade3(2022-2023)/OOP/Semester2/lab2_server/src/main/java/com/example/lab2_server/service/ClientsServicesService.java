package com.example.lab2_server.service;

import com.example.lab2_server.dao.ClientsServiceDao;
import com.example.lab2_server.model.ClientService;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class ClientsServicesService {
    private final ClientsServiceDao clientsServiceDao;

    public ClientsServicesService(ClientsServiceDao clientsServiceDao){
        this.clientsServiceDao = clientsServiceDao;
    }

    public void addClientService(int clientId, int serviceId){
        ClientService clientsService = new ClientService(clientId, serviceId);
        clientsServiceDao.save(clientsService);
    }

    public List<Long> getClientServices(int clientId){
        List<ClientService> clientServices = clientsServiceDao.findAllByClientId(clientId);
        List<Long> servicesIds =  clientServices.stream()
                .map(clientService -> clientService.getServiceId())
                .collect(Collectors.toList());
        return servicesIds;
    }
}

