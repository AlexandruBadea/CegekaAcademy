using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.DataAccessLayer.Models;

namespace PetShelter.DataAccessLayer.Configuration;

public class FundraiserConfiguration : IEntityTypeConfiguration <Fundraiser>
{
    public void Configure(EntityTypeBuilder<Fundraiser> builder)
    {
        //Primary key
        builder.HasKey(p => p.Id);

        //Columns mapping and constraints
        builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Description).HasMaxLength(50).IsRequired();
        builder.Property(p => p.DonationTarget).IsRequired();
    }
}