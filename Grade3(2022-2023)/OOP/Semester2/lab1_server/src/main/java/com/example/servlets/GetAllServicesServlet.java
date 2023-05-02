package com.example.servlets;

import com.example.dbObjects.Service;
import com.google.gson.Gson;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;

@WebServlet(name = "getAllServicesServlet", value = "/get-all-services")
public class GetAllServicesServlet extends DBServlet {
    @Override
    public void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException {
        super.doGet(request, response);

        PrintWriter out = response.getWriter();

        if(dbManager==null){
            out.println("NO DB CONNECTION");
            logger.info("NO DB CONNECTION");
            return;
        }

        ArrayList<Service> res = dbManager.GetAllServices();
        String json = new Gson().toJson(res);

        out.println(json);
        logger.info("GET ALL SERVICES");

        out.close();
    }
}
