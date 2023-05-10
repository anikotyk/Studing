package com.example.lab2_server.dao;

import com.example.lab2_server.model.Client;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ClientDao extends JpaRepository<Client, Long> {
    public List<Client> findAllByEmail(String email);
}
