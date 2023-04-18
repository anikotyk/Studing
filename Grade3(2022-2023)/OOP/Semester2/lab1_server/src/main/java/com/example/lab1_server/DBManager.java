package com.example.lab1_server;

import java.sql.Connection;
import java.sql.DriverManager;
public class DBManager {
    public static void main(String[] args) {
        try{
            Class.forName("org.postgresql.Driver");

            // connect way #1
            String url1 = "jdbc:postgresql://localhost:5432/phonestation";
            String user = "postgres";
            String password = "admin";

            Connection conn1 = DriverManager.getConnection(url1, user, password);
            if (conn1 != null) {
                System.out.println("Connected to the database test1");
            }

        }catch (Exception e){
            System.out.println(e);
        }


    }
}
