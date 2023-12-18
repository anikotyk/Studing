package com.example.backend.dao;

import com.example.backend.model.Admin;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface AdminDao extends JpaRepository<Admin, Long> {
    public List<Admin> findAllByEmail(String email);
}
