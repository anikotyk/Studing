package com.example.lab2_server.dao;

import com.example.lab2_server.model.ClientsService;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ClientsServiceDao extends JpaRepository<ClientsService, Long> {
}
