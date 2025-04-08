public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
               .UseIdentityColumn(); // Configura auto-incremento

        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey(si => si.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}