using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class GamaProductoConfiguration : IEntityTypeConfiguration<GamaProducto>
    {
        public void Configure(EntityTypeBuilder<GamaProducto> builder)
        {
            
            builder.HasKey(e => e.Gama).HasName("PRIMARY");

            builder.ToTable("gama_producto");

            builder.Property(e => e.Gama)
                .HasMaxLength(50)
                .HasColumnName("gama");
            builder.Property(e => e.DescriptionHtml)
                .HasColumnType("text")
                .HasColumnName("description_html");
            builder.Property(e => e.DescriptionTexto)
                .HasColumnType("text")
                .HasColumnName("description_texto");
            builder.Property(e => e.Imagen)
                .HasMaxLength(256)
                .HasColumnName("imagen");
            
        }
    }
}