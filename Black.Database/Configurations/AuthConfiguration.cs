﻿using Black.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Black.Database.Configurations;

public sealed class AuthConfiguration : IEntityTypeConfiguration<Auth>
{
    public void Configure(EntityTypeBuilder<Auth> builder)
    {
        builder.ToTable(nameof(Auth), nameof(Auth));

        builder.HasKey(auth => auth.Id);

        builder.HasIndex(auth => auth.Login).IsUnique();

        builder.Property(auth => auth.Id).ValueGeneratedOnAdd().IsRequired();

        builder.Property(auth => auth.Login).HasMaxLength(100).IsRequired();

        builder.Property(auth => auth.Password).HasMaxLength(1000).IsRequired();

        builder.Property(auth => auth.GuiddId).HasMaxLength(1000).IsRequired();

        builder.Property(auth => auth.Roles).IsRequired();
    }
}
