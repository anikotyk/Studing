package com.example.lab2_server.service;

import com.example.lab2_server.dao.PaymentDao;
import com.example.lab2_server.model.Payment;
import org.springframework.stereotype.Service;

@Service
public class PaymentService {
    private final PaymentDao paymentDao;

    public PaymentService(PaymentDao paymentDao){
        this.paymentDao = paymentDao;
    }

    public void addPayment(int clientId, int serviceId){
        Payment payment = new Payment(clientId, serviceId);
        paymentDao.save(payment);
    }
}

