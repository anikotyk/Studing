package com.example.lab2_server.api;

import com.example.lab2_server.service.PaymentsService;
import org.json.JSONException;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RequestMapping("")
@RestController
public class PaymentsController {
    private final PaymentsService paymentService;

    @Autowired
    public PaymentsController(PaymentsService paymentService){
        this.paymentService = paymentService;
    }

    @PostMapping("/add-client-payment")
    public void addPayment(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        int clientId = obj.getInt("clientId");
        int serviceId = obj.getInt("serviceId");

        paymentService.addPayment(clientId, serviceId);
    }
}
