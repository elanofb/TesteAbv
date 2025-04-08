public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id)
               .UseIdentityColumn(); // Configura auto-incremento
    }
}