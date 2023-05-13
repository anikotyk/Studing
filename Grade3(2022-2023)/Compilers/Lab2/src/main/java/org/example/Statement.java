package org.example;

import java.util.List;

public class Statement {
    private final StatementType stmtType;
    private final List<Token> tokens;

    public Statement(StatementType stmtType, List<Token> tokens) {
        this.stmtType = stmtType;
        this.tokens = tokens;
    }

    public StatementType getStmtType() {
        return stmtType;
    }

    public List<Token> getTokens() {
        return tokens;
    }

    @Override
    public String toString() {
        return stmtType.name() + ": " + tokens.toString();
    }
}
