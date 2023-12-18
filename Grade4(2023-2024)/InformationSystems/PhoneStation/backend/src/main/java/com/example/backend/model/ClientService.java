package com.example.backend.model;

import jakarta.persistence.*;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.ToString;

@Data
@NoArgsConstructor
@ToString
@Entity
@Table(name = "ClientsServices")
public class ClientService {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name="clientId")
    private long clientId;

    @Column(name="serviceId")
    private long serviceId;

    public ClientService(long clientId, long serviceId){
        this.clientId = clientId;
        this.serviceId = serviceId;
    }
}
