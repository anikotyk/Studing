package com.example.dbObjects;

import lombok.*;

@Data
@NoArgsConstructor
@AllArgsConstructor
@ToString
public class Service {
  private long id;
  private long price;
  private String name;
}
