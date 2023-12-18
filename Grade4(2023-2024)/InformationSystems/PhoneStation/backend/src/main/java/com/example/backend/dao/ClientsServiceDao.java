package com.example.backend.dao;

import com.example.backend.model.ClientService;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ClientsServiceDao extends JpaRepository<ClientService, Long> {
    public List<ClientService> findAllByClientId(long clientId);
}
