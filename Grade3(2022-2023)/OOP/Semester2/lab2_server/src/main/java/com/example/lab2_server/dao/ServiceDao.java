package com.example.lab2_server.dao;

import com.example.lab2_server.model.PhoneService;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ServiceDao extends JpaRepository<PhoneService, Long> {
}
