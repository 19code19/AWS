import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.Optional;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

public class HttpService {
    private final HttpClient client = HttpClient.newHttpClient();
    private final ObjectMapper mapper = new ObjectMapper();

    public <T> T sendRequest(
            String url,
            String method,
            Optional<Object> body,
            TypeReference<T> responseType
    ) throws Exception {

        HttpRequest.Builder builder = HttpRequest.newBuilder()
                .uri(URI.create(url))
                .header("Content-Type", "application/json");

        if (body.isPresent()) {
            String json = mapper.writeValueAsString(body.get());
            builder.method(method, HttpRequest.BodyPublishers.ofString(json));
        } else {
            builder.method(method, HttpRequest.BodyPublishers.noBody());
        }

        HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

        // convert JSON to object/list/etc.
        return mapper.readValue(response.body(), responseType);
    }
}



List<Employee> employees = httpService.sendRequest(
    "http://localhost:5000/api/Employee",
    "GET",
    Optional.empty(),
    new TypeReference<List<Employee>>() {}
);




Employee emp = new Employee(1, "John Doe", "Developer", "IT");

Employee created = httpService.sendRequest(
    "http://localhost:5000/api/Employee",
    "POST",
    Optional.of(emp),
    new TypeReference<Employee>() {}
);
