package com.example.lab2_server.dao;

import com.example.lab2_server.model.Payment;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface PaymentDao extends JpaRepository<Payment, Long> {
    public List<Payment> findAllByClientId(long clientId);
}
