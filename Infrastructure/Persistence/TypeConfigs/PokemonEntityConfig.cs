using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.TypeConfigs;
public class PokemonEntityConfig : IEntityTypeConfiguration<Pokemon>
{
    public void Configure(EntityTypeBuilder<Pokemon> builder)
    {
        // Entity Custom Configuration goes here
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
