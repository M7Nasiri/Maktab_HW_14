using ATM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Infrastructure.Configuration
{
    internal class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(c => c.CardNumber);

            //builder.HasMany(c => c.Transactions)
            //    .WithOne(t => t.SourceCard)
            //    .HasForeignKey(t => t.SourceCardNumber)
            //    .HasPrincipalKey(c => c.CardNumber)
            //    .IsRequired(true)
            //    .OnDelete(DeleteBehavior.Restrict);
            
            //builder.HasMany(c => c.Transactions)
            //    .WithOne(t => t.DestinationCard)
            //    .HasForeignKey(t => t.DestinationCardNumber)
            //    .HasPrincipalKey(c => c.CardNumber)
            //    .IsRequired(true)
            //    .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(c => c.CardNumber).HasMaxLength(16).IsRequired(true);
            builder.Property(c => c.VerificationCode).HasMaxLength(5).IsRequired(true);


        }
    }
}
