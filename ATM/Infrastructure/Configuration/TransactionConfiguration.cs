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
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.SourceCardNumber).HasMaxLength(16);
            builder.Property(t => t.DestinationCardNumber).HasMaxLength(16);

            builder.HasOne(t => t.SourceCard)
                .WithMany(c => c.SourceTransactions)
                .HasForeignKey(t => t.SourceCardNumber)
                .HasPrincipalKey(c => c.CardNumber)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DestinationCard)
                .WithMany(c => c.DestinationTransactions)
                .HasForeignKey(t => t.DestinationCardNumber)
                .HasPrincipalKey(c => c.CardNumber)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict); ;
        }
    }
}
