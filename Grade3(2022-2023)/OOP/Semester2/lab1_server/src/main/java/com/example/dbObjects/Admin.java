package com.example.dbObjects;

import lombok.*;

@Data
@NoArgsConstructor
@AllArgsConstructor
@ToString
public class Admin {
  private long id;
  private String email;
}
