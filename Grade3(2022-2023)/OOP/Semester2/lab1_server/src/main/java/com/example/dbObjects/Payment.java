package com.example.dbObjects;

import lombok.*;

@Data
@NoArgsConstructor
@AllArgsConstructor
@ToString
public class Payment {
  private long id;
  private long cliendId;
  private long serviceId;
}
