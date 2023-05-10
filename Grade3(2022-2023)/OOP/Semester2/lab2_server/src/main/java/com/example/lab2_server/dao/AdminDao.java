package com.example.lab2_server.dao;

import com.example.lab2_server.model.Admin;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface AdminDao extends JpaRepository<Admin, Long> {
    public List<Admin> findAllByEmail(String email);
}
