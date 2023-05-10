package com.example.lab2_server.model;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.*;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.ToString;

@Data
@ToString
@NoArgsConstructor
@Entity
@Table(name = "Clients")
public class Client {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name="isConfirmed")
    private boolean isConfirmed;

    @Column(name="isBanned")
    private boolean isBanned;

    @Column(name="phonenumber")
    private long phonenumber;

    @Column(name="email")
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
