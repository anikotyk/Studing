package com.example.lab2_server.dao;

import com.example.lab2_server.model.Service;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ServiceDao extends JpaRepository<Service, Long> {
}
