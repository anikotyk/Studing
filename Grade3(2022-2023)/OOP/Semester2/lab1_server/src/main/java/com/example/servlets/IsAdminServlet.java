package com.example.servlets;

import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.json.JSONException;

import java.io.IOException;
import java.io.PrintWriter;

@WebServlet(name = "isAdminServlet", value = "/is-admin")
public class IsAdminServlet extends DBServlet {
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
            boolean res = dbManager.IsUserAdmin(clientEmail);
            out.println(res);
            logger.info("IS CLIENT WITH EMAIL " + clientEmail+" ADMIN: "+ res);
        } catch (JSONException e) {
            out.println("DOESN'T CONTAIN REQUIRED KEY");
            logger.info("DOESN'T CONTAIN REQUIRED KEY");
        }

        out.close();
    }
}
