package org.example;
import java.util.*;

public class PythonParser {
    private final PythonLexer lexer;
    private Token currentToken;
    private List<Token> tokens;
    private int indexToken = 0;

    public PythonParser(PythonLexer lexer) {
        this.lexer = lexer;
        this.tokens = lexer.lex();
        this.currentToken = tokens.get(this.indexToken);
    }

    public List<Statement> parse(){
        List<Statement> statements = new ArrayList<>();
        while (indexToken < tokens.size()) {
            statements.add(parseStatement());
        }
        return statements;
    }

    private Statement parseStatement() {
        if (match(TokenType.KEYWORD, "if")) {
            return parseIfStatement();
        }else if (match(TokenType.KEYWORD, "else")) {
            return parseElseStatement();
        }else if (match(TokenType.KEYWORD, "while")) {
            return parseWhileStatement();
        }else if (match(TokenType.KEYWORD, "for")) {
            return parseForStatement();
        }else if (match(TokenType.KEYWORD, "def")) {
            return parseFunctionDefinition();
        }else if (match(TokenType.KEYWORD, "print")) {
            return parsePrintStatement();
        }else if (currentToken.getType() == TokenType.IDENTIFIER) {
            return parseAssignmentStatement();
        }else {
            List<Token> errorTokens = new ArrayList<>();
            errorTokens.add(currentToken);
            NextToken();
            return new Statement(StatementType.ERROR, errorTokens);
        }
    }

    private boolean match(TokenType type, String value){
        if(currentToken.getType() == type && Objects.equals(currentToken.getValue(), value)){
            return true;
        }
        return false;
    }

    private Statement parseIfStatement() {
        Statement ifStatement = new Statement(StatementType.IF, parseCondition());
        return ifStatement;
    }

    private Statement parseElseStatement() {
        Statement ifStatement = new Statement(StatementType.ELSE, parseCondition());
        return ifStatement;
    }

    private Statement parseWhileStatement() {
        Statement whileStatement = new Statement(StatementType.WHILE, parseCondition());
        return whileStatement;
    }

    private Statement parseForStatement() {
        Statement forStatement = new Statement(StatementType.FOR, parseCondition());
        return forStatement;
    }

    private Statement parseFunctionDefinition(){
        Statement defStatement = new Statement(StatementType.FUNCTION_DEF, parseCondition());
        return defStatement;
    }

    private Statement parsePrintStatement(){
        Statement printStatement = new Statement(StatementType.PRINT, parseParentheses());
        return printStatement;
    }

    private Statement parseAssignmentStatement(){
        List<Token> tokensList = new ArrayList<>();
        tokensList.add(currentToken);
        NextToken();
        if(match(TokenType.OPERATOR, "=")){
            tokensList.add(currentToken);
            NextToken();
            if(currentToken.getType() == TokenType.FLOAT || currentToken.getType() == TokenType.INTEGER || currentToken.getType() == TokenType.STRING || currentToken.getType() == TokenType.IDENTIFIER){
                tokensList.add(currentToken);
                NextToken();
            }
        }else{
            return new Statement(StatementType.ERROR, null);
        }

        Statement assignmentStatement = new Statement(StatementType.ASSIGNMENT, tokensList);
        return assignmentStatement;
    }

    private List<Token> parseCondition(){
        List<Token> tokensList = new ArrayList<>();
        while(currentToken!=null && !match(TokenType.OPERATOR, ":")){
            tokensList.add(currentToken);
            NextToken();
        }
        if(currentToken!=null){
            tokensList.add(currentToken);
            NextToken();
        }

        return tokensList;
    }

    private List<Token> parseParentheses(){
        List<Token> tokensList = new ArrayList<>();
        while(currentToken!=null && !match(TokenType.OPERATOR, ")")){
            tokensList.add(currentToken);
            NextToken();
        }
        if(currentToken!=null){
            tokensList.add(currentToken);
            NextToken();
        }

        return tokensList;
    }

    private void NextToken(){
        indexToken++;
        if(indexToken<tokens.size()){
            currentToken = tokens.get(indexToken);
        }else{
            currentToken = null;
        }
    }
}