package com.example.dbObjects;

import lombok.*;

@Data
@NoArgsConstructor
@AllArgsConstructor
@ToString
public class Client {
  private long id;
  private boolean isConfirmed;
  private boolean isBanned;
  private long phonenumber;
  private String email;
}
