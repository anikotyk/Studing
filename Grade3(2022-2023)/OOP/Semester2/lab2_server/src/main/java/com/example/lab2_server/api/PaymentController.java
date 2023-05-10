package com.example.lab2_server.api;

import com.example.lab2_server.service.PaymentService;
import com.fasterxml.jackson.annotation.JsonProperty;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RequestMapping("")
@RestController
public class PaymentController {
    private final PaymentService paymentService;

    @Autowired
    public PaymentController(PaymentService paymentService){
        this.paymentService = paymentService;
    }

    @PostMapping("/add-client-payment")
    public void addClient(@RequestBody @JsonProperty int clientId, @JsonProperty int serviceId){
        paymentService.addPayment(clientId, serviceId);
    }
}
