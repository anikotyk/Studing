package com.example.servlets;

import com.example.dbObjects.Client;
import com.google.gson.Gson;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.json.JSONException;

import java.io.IOException;
import java.io.PrintWriter;

@WebServlet(name = "getClientServlet", value = "/get-client")
public class GetClientServlet extends DBServlet {
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
            String clientEmail = requestJsonData.getString("clientEmail");
            Client client = dbManager.GetClient(clientEmail);
            if(client == null){
                logger.info("CREATE CLIENT WITH EMAIL " + clientEmail);
                dbManager.AddNewClient(clientEmail);
                client = dbManager.GetClient(clientEmail);
            }
            String json = new Gson().toJson(client);
            out.println(json);
            logger.info("GET CLIENT WITH EMAIL " + clientEmail);
        } catch (JSONException e) {
            out.println("DOESN'T CONTAIN REQUIRED KEY");
            logger.info("DOESN'T CONTAIN REQUIRED KEY");
        }

        out.close();
    }
}
