using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfiguration;
public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees",SchemaNames.TUTORIAL)
            .HasIndex(e=>e.FirstName)
            .HasDatabaseName("IX_Emplooyes_FirstName");

        builder.HasIndex(e => e.LastName)
            .HasDatabaseName("IX_Emplooyes_LastName");
    }
}
