package com.example.lab2_server.dao;

import com.example.lab2_server.model.Client;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository("postgres")
public interface ClientDao extends JpaRepository<Client, Long> {
}
