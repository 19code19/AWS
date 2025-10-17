import com.zaxxer.hikari.HikariDataSource;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class DataSourceConfig {

    @Value("${spring.datasource.url}")
    private String dbUrl;

    @Value("${spring.datasource.username}")
    private String dbUser;

    @Value("${spring.datasource.password}")
    private String dbPassword;

    @Value("${spring.datasource.driver-class-name}")
    private String driverClassName;

    private HikariDataSource hikariDataSource;

    @Bean
    public HikariDataSource dataSource() {
        hikariDataSource = new HikariDataSource();
        hikariDataSource.setDriverClassName(driverClassName);
        hikariDataSource.setJdbcUrl(dbUrl);
        hikariDataSource.setUsername(dbUser);
        hikariDataSource.setPassword(dbPassword);
        return hikariDataSource;
    }

    /** Update HikariCP password dynamically */
    public void updatePassword(String newPassword) {
        if (hikariDataSource != null) {
            hikariDataSource.getHikariConfigMXBean().setPassword(newPassword);
            System.out.println("HikariCP password updated successfully!");
        }
    }
}



import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

@Service
public class PasswordRefreshService {

    private final DataSourceConfig dataSourceConfig;

    public PasswordRefreshService(DataSourceConfig dataSourceConfig) {
        this.dataSourceConfig = dataSourceConfig;
    }

    /** Fetch new password from Vault or internal store */
    private String fetchNewPassword() {
        // TODO: implement your secret retrieval logic
        return "newPassword123"; 
    }

    /** Rotate password every 90 days */
    @Scheduled(fixedRate = 90L * 24 * 60 * 60 * 1000) // 90 days in ms
    public void refreshDatabasePassword() {
        String newPassword = fetchNewPassword();
        dataSourceConfig.updatePassword(newPassword);
    }
}
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@SpringBootApplication
@EnableScheduling
public class MyApplication {
    public static void main(String[] args) {
        SpringApplication.run(MyApplication.class, args);
    }
}
