package com.example.lab2_server;

import com.example.lab2_server.dao.ClientDao;
import com.example.lab2_server.model.Client;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.context.annotation.Bean;


@SpringBootApplication()
@EntityScan("com.example.lab2_server.model")
//@EnableJpaRepositories("com.example.lab2_server.dao")
public class Lab2ServerApplication {
	public static void main(String[] args) {
		SpringApplication.run(Lab2ServerApplication.class, args);
	}

	@Bean
	CommandLineRunner commandLineRunner(ClientDao clientDao){
		return args -> {
			Client client = new Client();
			client.setEmail("anna8");
			clientDao.save(client);

			Client client2 = clientDao.findAllByEmail("anna8").get(0);
			System.out.println(client2.isConfirmed());
		};
	}
}
