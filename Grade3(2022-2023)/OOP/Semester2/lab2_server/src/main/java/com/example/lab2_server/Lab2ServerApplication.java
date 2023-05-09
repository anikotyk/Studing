package com.example.lab2_server;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.jdbc.DataSourceAutoConfiguration;


@SpringBootApplication(exclude = {DataSourceAutoConfiguration.class})
public class Lab2ServerApplication {
	public static void main(String[] args) {
		SpringApplication.run(Lab2ServerApplication.class, args);
	}

	/*@Bean
	CommandLineRunner commandLineRunner(@Autowired ClientDao clientDao){
		return args -> {
			Client client = new Client();
			client.setEmail("anna1");
			clientDao.save(client);
		};
	}*/
}
