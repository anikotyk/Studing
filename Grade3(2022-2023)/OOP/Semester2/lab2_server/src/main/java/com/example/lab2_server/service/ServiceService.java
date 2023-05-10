package com.example.lab2_server.service;

import com.example.lab2_server.dao.ServiceDao;
import org.springframework.stereotype.Service;

@Service
public class ServiceService {
    private final ServiceDao serviceDao;

    public ServiceService(ServiceDao serviceDao){
        this.serviceDao = serviceDao;
    }
}

