package com.example.backend.service;

import com.example.backend.dao.ServiceDao;
import com.example.backend.model.PhoneService;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class PhoneServicesService {
    private final ServiceDao serviceDao;

    public PhoneServicesService(ServiceDao serviceDao){
        this.serviceDao = serviceDao;
    }

    public List<PhoneService> getAllServices(){
        List<PhoneService> services = serviceDao.findAll();
        return services;
    }

    public List<PhoneService> getAllServicesByIds(List<Long> ids){
        List<PhoneService> services = new ArrayList<>();
        for(int i = 0; i < ids.size(); i++){
            services.add(serviceDao.findById(ids.get(i)).get());
        }
        return services;
    }
}

