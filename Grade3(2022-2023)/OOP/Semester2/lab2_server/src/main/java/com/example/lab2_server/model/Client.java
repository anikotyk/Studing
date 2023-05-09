package com.example.lab2_server.model;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.*;
import lombok.*;

@Data
@NoArgsConstructor
@ToString
@Entity
@Table(name = "Clients")
public class Client {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;
    private boolean isConfirmed;
    private boolean isBanned;
    private long phonenumber;
    private String email;

    public Client(
                  @JsonProperty("isConfirmed") boolean isConfirmed,
                  @JsonProperty("isBanned") boolean isBanned,
                  @JsonProperty("phonenumber") long phonenumber,
                  @JsonProperty("email") String email)
    {
        this.isConfirmed = isConfirmed;
        this.isBanned = isBanned;
        this.phonenumber = phonenumber;
        this.email = email;
    }
}
