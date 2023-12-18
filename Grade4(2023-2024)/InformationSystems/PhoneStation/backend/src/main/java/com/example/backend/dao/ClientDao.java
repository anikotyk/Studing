package com.example.backend.dao;

import com.example.backend.model.Client;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ClientDao extends JpaRepository<Client, Long> {
    public List<Client> findAllByEmail(String email);
}
