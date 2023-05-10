package com.example.lab2_server.dao;

import com.example.lab2_server.model.ClientService;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ClientsServiceDao extends JpaRepository<ClientService, Long> {
    public List<ClientService> findAllByClientId(long clientId);
}
