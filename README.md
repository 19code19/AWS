public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;
    private WireMockServer _wireMockServer;

    public CustomWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        // Start WireMock
        _wireMockServer = WireMockServer.Start();
    }

    public void SetupWireMockResponse(string path, string jsonResponse, HttpStatusCode httpStatusCode)
    {
        _wireMockServer.Reset(); // Clear previous mappings
        _wireMockServer
            .Given(Request.Create().WithPath(path).UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(httpStatusCode)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(jsonResponse)
            );
    }

    public string WireMockUrl => _wireMockServer.Url;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registrations
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(AppDbContext));
            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            // Add SQLite in-memory DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            // Add HttpClient pointing to WireMock
            services.AddHttpClient("ProductsClient", client =>
            {
                client.BaseAddress = new Uri(_wireMockServer.Url);
            });

            // Build service provider to initialize DB
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                InitializeDbForTests(db);
            }
        });
    }

    private void InitializeDbForTests(AppDbContext db)
    {
        db.Products.RemoveRange(db.Products);
        db.SaveChanges();

        db.Products.Add(new Product { Id = 1, Name = "Test Product", Price = 100 });
        db.SaveChanges();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Close();
        _connection?.Dispose();
        _wireMockServer?.Stop();
        _wireMockServer?.Dispose();
    }
}



















    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public ProductsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostProduct_ReturnsWireMockedForecast()
        {
            var client = _factory.CreateClient();
            string jsonData = @"[{""date"":""2025-08-23"",""temperatureC"":31,""temperatureF"":87,""summary"":""Mild""},{""date"":""2025-08-24"",""temperatureC"":-1,""temperatureF"":31,""summary"":""Chilly""},{""date"":""2025-08-25"",""temperatureC"":44,""temperatureF"":111,""summary"":""Freezing""},{""date"":""2025-08-26"",""temperatureC"":51,""temperatureF"":123,""summary"":""Scorching""},{""date"":""2025-08-27"",""temperatureC"":27,""temperatureF"":80,""summary"":""Mild""}]";
            _factory.SetupWireMockResponse("/WeatherForecast", jsonData, System.Net.HttpStatusCode.OK);

            var newProduct = new Product { Name = "WireMock Product", Price = 150 };
            var json = new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/products", json);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<WeatherForecast>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotEmpty(data); // WireMock response
        }

        [Fact]
        public async Task Products_CRUD_Test_WithSQLite_AndWireMock()
        {
            var client = _factory.CreateClient();

            // GET
            var getResponse = await client.GetAsync("/api/products");
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("Test Product", getContent);

            // POST
            var newProduct = new Product { Name = "New Product", Price = 200 };
            var json = new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync("/api/products", json);
            postResponse.EnsureSuccessStatusCode();
            var postContent = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains("Sunny", postContent); // WireMock
            Assert.Contains("New Product", postContent);

            // PUT
            var putProduct = new Product { Id = 1, Name = "Updated Product", Price = 150 };
            var putJson = new StringContent(JsonSerializer.Serialize(putProduct), Encoding.UTF8, "application/json");
            var putResponse = await client.PutAsync("/api/products/1", putJson);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);

            // Verify update
            var verifyResponse = await client.GetAsync("/api/products");
            verifyResponse.EnsureSuccessStatusCode();
            var verifyContent = await verifyResponse.Content.ReadAsStringAsync();
            Assert.Contains("Updated Product", verifyContent);

            // DELETE
            var deleteResponse = await client.DeleteAsync("/api/products/1");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify deletion
            var finalGetResponse = await client.GetAsync("/api/products/1");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, finalGetResponse.StatusCode);
        }
    }
}
