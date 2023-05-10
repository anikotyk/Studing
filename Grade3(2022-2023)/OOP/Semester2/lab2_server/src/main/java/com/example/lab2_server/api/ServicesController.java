package com.example.lab2_server.api;

import com.example.lab2_server.model.PhoneService;
import com.example.lab2_server.service.ClientsServicesService;
import com.example.lab2_server.service.PaymentsService;
import com.example.lab2_server.service.PhoneServicesService;
import com.google.gson.Gson;
import org.json.JSONException;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@RequestMapping("")
@RestController
public class ServicesController {
    private final PhoneServicesService phoneServicesService;
    private final ClientsServicesService clientsServicesService;
    private final PaymentsService paymentsService;

    @Autowired
    public ServicesController(PhoneServicesService serviceService, ClientsServicesService clientsServicesService,  PaymentsService paymentsService){
        this.phoneServicesService = serviceService;
        this.clientsServicesService = clientsServicesService;
        this.paymentsService = paymentsService;
    }

    @GetMapping("/get-all-services")
    public List<PhoneService> getAllServices(){
        return phoneServicesService.getAllServices();
    }

    @PostMapping("/get-all-client-added-services")
    public String getAllClientAddedServices(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        int clientId = obj.getInt("clientId");

        List<Long> servicesIds = clientsServicesService.getClientServices(clientId);
        List<PhoneService> services = phoneServicesService.getAllServicesByIds(servicesIds);

        String json = new Gson().toJson(services);
        return json;
    }

    @PostMapping("/get-all-client-paid-services")
    public String getAllClientPaidServices(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        int clientId = obj.getInt("clientId");

        List<Long> servicesIds = paymentsService.getClientPaidServices(clientId);
        List<PhoneService> services = phoneServicesService.getAllServicesByIds(servicesIds);

        String json = new Gson().toJson(services);
        return json;
    }

    @PostMapping("/get-all-client-unpaid-services")
    public String getAllClientUnpaidServices(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        int clientId = obj.getInt("clientId");

        List<Long> servicesAddedIds = clientsServicesService.getClientServices(clientId);
        List<Long> servicesPaidIds = paymentsService.getClientPaidServices(clientId);

        List<Long> servicesIds = new ArrayList<>(servicesAddedIds);
        servicesIds.removeAll(servicesPaidIds);

        List<PhoneService> services = phoneServicesService.getAllServicesByIds(servicesIds);

        String json = new Gson().toJson(services);
        return json;
    }
}
