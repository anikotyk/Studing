package com.example.dbObjects;

import lombok.*;

@Data
@NoArgsConstructor
@AllArgsConstructor
@ToString
public class ClientsService {
  private long id;
  private long clientId;
  private long serviceId;
}
