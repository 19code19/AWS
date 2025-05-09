using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    var employeeConverter = new ValueConverter<Employee, string>(
        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),   // Serialize
        v => JsonSerializer.Deserialize<Employee>(v, new JsonSerializerOptions()) // Deserialize
    );

    modelBuilder.Entity<MyEntity>()
        .Property(e => e.Employee)
        .HasConversion(employeeConverter);
}
