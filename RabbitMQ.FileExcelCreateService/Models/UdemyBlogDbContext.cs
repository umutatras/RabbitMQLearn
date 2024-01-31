using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RabbitMQ.FileExcelCreateService.Models
{
    public partial class UdemyBlogDbContext : DbContext
    {
        public UdemyBlogDbContext()
        {
        }

        public UdemyBlogDbContext(DbContextOptions<UdemyBlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;

  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.ContentMain).HasColumnName("content_main");

                entity.Property(e => e.ContentSummary)
                    .HasMaxLength(500)
                    .HasColumnName("content_summary");

                entity.Property(e => e.Picture)
                    .HasMaxLength(300)
                    .HasColumnName("picture");

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("publish_date");

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .HasColumnName("title");

                entity.Property(e => e.ViewCount).HasColumnName("viewCount");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
