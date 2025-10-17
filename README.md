spring:
  security:
    oauth2:
      client:
        registration:
          adfs:
            client-id: 
            client-secret: your-secret
            authorization-grant-type: authorization_code
            redirect-uri: "{baseUrl}/login/oauth2/code/{registrationId}"
        provider:
          adfs:
            issuer-uri: 

.oauth2Login(Customizer.withDefaults())


<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-oauth2-client</artifactId>
</dependency>
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-security</artifactId>
</dependency>
