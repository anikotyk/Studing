package com.example.lab2_server.api;

import com.example.lab2_server.service.AdminsService;
import org.json.JSONException;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RequestMapping("")
@RestController
public class AdminsController {
    private final AdminsService adminService;

    @Autowired
    public AdminsController(AdminsService adminService){
        this.adminService = adminService;
    }

    @PostMapping("/is-admin")
    public boolean isAdmin(@RequestBody String body) throws JSONException {
        JSONObject obj = new JSONObject(body);
        String email = obj.getString("clientEmail");

        return adminService.isAdmin(email);
    }
}
