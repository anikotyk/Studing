package com.example.lab1_server;

import java.io.*;

import jakarta.servlet.http.*;
import jakarta.servlet.annotation.*;
import org.apache.log4j.Logger;

@WebServlet(name = "numberResource", value = "/number-resource")
public class NumbersResource extends HttpServlet{
    private String message;
    private static Logger logger = Logger.getLogger(NumbersResource.class);
    public void init() {
        message = "304!";
    }

    public void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException {
        logger.info("MAKE GET REQUEST");
        response.setContentType("text/html");

        // Hello
        PrintWriter out = response.getWriter();
        out.println("<html><body>");
        out.println("<h1>" + message + "</h1>");

        out.println("<h1>" + request.getMethod().toString().trim().toUpperCase() + "</h1>");
        out.println("<h1>" + (request.getMethod().toString().trim().toUpperCase()=="GET") + "</h1>");
        if(request.getMethod().toString().trim().toUpperCase()=="GET"){
            out.println("<h1>" + "GEEEEEEET" + "</h1>");
        }

        out.println("</body></html>");
    }

    public void destroy() {

    }
}
