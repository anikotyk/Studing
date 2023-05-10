package com.example.lab2_server.model;

import jakarta.persistence.*;
import lombok.*;

@Data
@NoArgsConstructor
@ToString
@Entity
@Table(name = "Admins")
public class Admin {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name="email")
    private String email;

    public Admin(String email){
        this.email = email;
    }
}
