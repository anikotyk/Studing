package com.example.servlets;

import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.json.JSONException;

import java.io.IOException;
import java.io.PrintWriter;

@WebServlet(name = "setClientPhoneServlet", value = "/set-client-phone")
public class SetClientPhoneServlet extends DBServlet {

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
            int clientPhone = requestJsonData.getInt("clientPhone");
            dbManager.SetClientPhone(clientId, clientPhone);
            out.println("true");
            logger.info("SET CLIENT WITH ID " + clientId +" PHONE NUMBER "+clientPhone);
        } catch (JSONException e) {
            out.println("DOESN'T CONTAIN REQUIRED KEY/S");
            logger.info("DOESN'T CONTAIN REQUIRED KEY/S");
        }

        out.close();
    }
}
