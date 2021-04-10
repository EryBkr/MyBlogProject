using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Concrete.EntityFramework.Mappings
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Text).IsRequired();
            builder.Property(c => c.Text).HasMaxLength(1000);
            builder.Property(c => c.CreatedByName).IsRequired();
            builder.Property(c => c.CreatedByName).HasMaxLength(50);
            builder.Property(c => c.ModifiedByName).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(50);
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.ModifiedDate).IsRequired();
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();
            builder.Property(c => c.Note).HasMaxLength(500);

            builder.HasOne<Article>(c => c.Article).WithMany(a => a.Comments).HasForeignKey(c => c.ArticleId);

            builder.ToTable("Comments");


            //Id dahil bütün dataları eksiksiz vermemiz gerekiyor.
            //HasData o tabloya ait data yoksa oluşacak
            builder.HasData(
                new Comment
                {
                    Id=1,
                    ArticleId=1,
                    Text="C# Yorumu",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "Initial Create",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "Initial Create",
                    ModifiedDate = DateTime.Now,
                    Note="Makale Yorumu",
                },
                new Comment
                {
                    Id = 2,
                    ArticleId = 2,
                    Text = ".Net Core MVC Yorumu",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "Initial Create",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "Initial Create",
                    ModifiedDate = DateTime.Now,
                    Note = "Makale Yorumu",
                });
        }
    }
}
