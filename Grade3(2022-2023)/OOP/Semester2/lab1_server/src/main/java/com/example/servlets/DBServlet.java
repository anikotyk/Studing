package com.example.servlets;

import com.example.lab1_server.DBManager;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.apache.log4j.Logger;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public abstract class DBServlet extends HttpServlet {
    protected Logger logger = Logger.getLogger(this.getClass().getSimpleName());
    protected DBManager dbManager;

    protected JSONObject requestJsonData;

    public void init() {
        dbManager = new DBManager();

        try {
            dbManager.Init();
        } catch (Exception e) {
            dbManager = null;
        }
    }

    public void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException {
        logger.info("RECEIVED GET REQUEST");
    }

    public void doPost(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
        logger.info("RECEIVED POST REQUEST");
        getRequestJsonData(request);
    }

    private void getRequestJsonData(HttpServletRequest request){
        String payloadRequest = null;

        try {
            payloadRequest = getBody(request);
        } catch (IOException e) {
            payloadRequest = "";
        }

        try {
            requestJsonData = new JSONObject(payloadRequest);
        } catch (JSONException e) {
            requestJsonData = null;
        }
    }

    private static String getBody(HttpServletRequest request) throws IOException {

        String body = null;
        StringBuilder stringBuilder = new StringBuilder();
        BufferedReader bufferedReader = null;

        try {
            InputStream inputStream = request.getInputStream();
            if (inputStream != null) {
                bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
                char[] charBuffer = new char[128];
                int bytesRead = -1;
                while ((bytesRead = bufferedReader.read(charBuffer)) > 0) {
                    stringBuilder.append(charBuffer, 0, bytesRead);
                }
            } else {
                stringBuilder.append("");
            }
        } catch (IOException ex) {
            throw ex;
        } finally {
            if (bufferedReader != null) {
                try {
                    bufferedReader.close();
                } catch (IOException ex) {
                    throw ex;
                }
            }
        }

        body = stringBuilder.toString();
        return body;
    }
}
