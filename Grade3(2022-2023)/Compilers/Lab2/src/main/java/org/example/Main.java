package org.example;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.List;

public class Main {
    public static void main(String[] args) throws IOException {
        String file = "C:\\Study\\GitHub\\Studing\\Grade3(2022-2023)\\Compilers\\Lab2\\main.py";
        String code = Files.readString(Paths.get(file));

        PythonLexer lexer = new PythonLexer(code);
        PythonParser parser = new PythonParser(lexer);

        List<Statement> statements = parser.parse();
        for (Statement statement : statements) {
            System.out.println(statement);
        }
    }
}