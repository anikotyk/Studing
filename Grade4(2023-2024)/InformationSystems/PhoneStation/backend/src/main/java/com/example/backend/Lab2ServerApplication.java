package com.example.backend;

import com.example.backend.dao.ClientDao;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.context.annotation.Bean;


@SpringBootApplication()
@EntityScan("com.example.backend.model")
public class Lab2ServerApplication {
	public static void main(String[] args) {
		SpringApplication.run(Lab2ServerApplication.class, args);
	}

	@Bean
	CommandLineRunner commandLineRunner(ClientDao clientDao){
		return args -> {

		};
	}
}
