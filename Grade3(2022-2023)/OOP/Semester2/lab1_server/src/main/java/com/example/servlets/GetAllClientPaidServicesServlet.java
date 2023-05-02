package com.example.servlets;

import com.google.gson.Gson;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.json.JSONException;

import java.io.IOException;
import java.io.PrintWriter;

@WebServlet(name = "getAllClientPaidServicesServlet", value = "/get-all-client-paid-services")
public class GetAllClientPaidServicesServlet extends DBServlet {
    @Override
    public void doPost(HttpServletRequest request, HttpServletResponse response) throws IOException, ServletException {
        super.doPost(request, response);

        PrintWriter out = response.getWriter();

        if(requestJsonData == null){
            out.println("EMPTY REQUEST");
            logger.info("EMPTY REQUEST");
            return;
        }
        if(dbManager==null){
            out.println("NO DB CONNECTION");
            logger.info("NO DB CONNECTION");
            return;
        }

        try {
            int clientId = requestJsonData.getInt("clientId");
            var res = dbManager.GetAllClientPaidServices(clientId);
            String json = new Gson().toJson(res);
            out.println(json);
            logger.info("GET ALL CLIENT PAID SERVICES, ID: "+ clientId);
        } catch (JSONException e) {
            out.println("DOESN'T CONTAIN REQUIRED KEY");
            logger.info("DOESN'T CONTAIN REQUIRED KEY");
        }

        out.close();
    }
}
