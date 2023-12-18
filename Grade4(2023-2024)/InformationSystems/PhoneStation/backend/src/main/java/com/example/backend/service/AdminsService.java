package com.example.backend.service;

import com.example.backend.dao.AdminDao;
import com.example.backend.model.Admin;
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

