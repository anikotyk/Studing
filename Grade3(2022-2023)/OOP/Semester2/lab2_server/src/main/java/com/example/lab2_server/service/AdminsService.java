package com.example.lab2_server.service;

import com.example.lab2_server.dao.AdminDao;
import com.example.lab2_server.model.Admin;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class AdminsService {
    private final AdminDao adminDao;

    public AdminsService(AdminDao adminDao){
        this.adminDao = adminDao;
    }

    public boolean isAdmin(String email){
        List<Admin> admin = adminDao.findAllByEmail(email);
        return !admin.isEmpty();
    }
}

