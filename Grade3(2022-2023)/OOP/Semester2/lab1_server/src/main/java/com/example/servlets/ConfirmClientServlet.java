package com.example.servlets;

import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.json.JSONException;

import java.io.*;

@WebServlet(name = "confirmClientServlet", value = "/confirm-client-servlet")
public class ConfirmClientServlet extends DBServlet {

    @Override
    public void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
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
            dbManager.ConfirmClient(clientId);
            out.println("CONFIRMED CLIENT WITH ID " + clientId);
            logger.info("CONFIRMED CLIENT WITH ID " + clientId);
        } catch (JSONException e) {
            out.println("DOESN'T CONTAIN REQUIRED KEY");
            logger.info("DOESN'T CONTAIN REQUIRED KEY");
        }

        out.close();
    }
}
