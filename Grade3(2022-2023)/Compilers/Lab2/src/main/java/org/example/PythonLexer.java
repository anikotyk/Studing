package org.example;
import java.util.*;

public class PythonLexer {
    private final String input;
    private int pos;
    private final List<Character> OPERATORS = Arrays.asList('+', '-', '*', '/', '%', '=', '<', '>', '&', '|', ':', '(', ')');
    private final List<String> KEYWORDS = Arrays.asList("and", "or", "nor", "if", "else", "while", "def", "for", "print", "input", "do");

    public PythonLexer(String input) {
        this.input = input;
        this.pos = 0;
    }

    public List<Token> lex() {
        List<Token> tokens = new ArrayList<>();
        while (pos < input.length()) {
            char currentChar = input.charAt(pos);

            if (Character.isWhitespace(currentChar)) {
                pos++;
            } else if (Character.isDigit(currentChar)) {
                tokens.add(lexNumber());
            } else if (Character.isLetter(currentChar) || currentChar == '_') {
                tokens.add(lexIdentifier());
            } else if (OPERATORS.contains(currentChar)) {
                tokens.add(lexOperator());
            } else if (currentChar == '\"' || currentChar == '\'') {
                tokens.add(lexString());
            } else {
                tokens.add(new Token(TokenType.ERROR, "Unknown token: " + currentChar));
                pos++;
            }

        }
        return tokens;
    }

    private Token lexNumber() {
        int start = pos;
        boolean isFloat = false;
        while (pos < input.length() && (Character.isDigit(input.charAt(pos)) || input.charAt(pos) == '.')) {
            if (input.charAt(pos) == '.') {
                isFloat = true;
            }
            pos++;
        }
        String value = input.substring(start, pos);
        if (isFloat) {
            return new Token(TokenType.FLOAT, value);
        } else {
            return new Token(TokenType.INTEGER, value);
        }
    }

    private Token lexIdentifier() {
        int start = pos;
        while (pos < input.length() && (Character.isLetterOrDigit(input.charAt(pos)) || input.charAt(pos) == '_')) {
            pos++;
        }
        String value = input.substring(start, pos);

        if(KEYWORDS.contains(value)){
            return new Token(TokenType.KEYWORD, value);
        }
        return new Token(TokenType.IDENTIFIER, value);
    }

    private Token lexOperator() {
        int start = pos;
        while (pos < input.length() && OPERATORS.contains(input.charAt(pos))) {
            pos++;
        }
        String value = input.substring(start, pos);
        return new Token(TokenType.OPERATOR, value);
    }

    private Token lexString() {
        char quoteChar = input.charAt(pos);
        pos++;
        int start = pos;
        while (pos < input.length() && input.charAt(pos) != quoteChar) {
            pos++;
        }
        if (pos == input.length()) {
            return new Token(TokenType.ERROR, "Unterminated string");
        }
        String value = input.substring(start, pos);
        pos++;

        return new Token(TokenType.STRING, value);
    }
}

