package com.example.backend.dao;

import com.example.backend.model.PhoneService;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ServiceDao extends JpaRepository<PhoneService, Long> {
}
