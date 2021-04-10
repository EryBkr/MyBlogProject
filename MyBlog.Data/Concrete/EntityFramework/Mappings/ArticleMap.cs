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
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            //PK Belirlendi
            builder.HasKey(i => i.Id);

            //Eklendikçe değer oluştur
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Title).HasMaxLength(100);

            builder.Property(i => i.Title).IsRequired();
            builder.Property(i => i.Content).IsRequired();
            builder.Property(i => i.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(i => i.Date).IsRequired();
            builder.Property(i => i.SeoAuthor).IsRequired();
            builder.Property(i => i.SeoAuthor).HasMaxLength(50);
            builder.Property(i => i.SeoDescription).HasMaxLength(150);
            builder.Property(i => i.SeoDescription).IsRequired();
            builder.Property(i => i.SeoTags).IsRequired();
            builder.Property(i => i.SeoTags).HasMaxLength(70);
            builder.Property(i => i.ViewsCount).IsRequired();
            builder.Property(i => i.CommentsCount).IsRequired();
            builder.Property(i => i.Thumbnail).IsRequired();
            builder.Property(i => i.Thumbnail).HasMaxLength(250);
            builder.Property(i => i.CreatedByName).IsRequired();
            builder.Property(i => i.CreatedByName).HasMaxLength(50);
            builder.Property(i => i.ModifiedByName).IsRequired();
            builder.Property(i => i.ModifiedByName).HasMaxLength(50);
            builder.Property(i => i.CreatedDate).IsRequired();
            builder.Property(i => i.ModifiedDate).IsRequired();
            builder.Property(i => i.IsActive).IsRequired();
            builder.Property(i => i.IsDeleted).IsRequired();
            builder.Property(i => i.Note).HasMaxLength(500);

            //Relationships
            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);

            builder.ToTable("Articles");

            //Id dahil bütün dataları eksiksiz vermemiz gerekiyor.
            //HasData o tabloya ait data yoksa oluşacak
            builder.HasData(
                new Article
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "C# 9.0 Yenilikleri",
                    Content = "Makale ile ilgili içerikler",
                    Thumbnail = "Default.jpg",
                    SeoDescription = "C# 9.0 Yenilikleri",
                    SeoTags = "C#,Net Core,Web,MVC",
                    SeoAuthor = "Eray Bakır",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "Initial Create",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "Initial Create",
                    ModifiedDate = DateTime.Now,
                    Note = "C# Yenilikleriyle ilgili Makale",
                    UserId = 1,
                    ViewsCount = 50,
                    CommentsCount = 1
                },
                new Article
                {
                    Id = 2,
                    CategoryId = 2,
                    Title = ".Net Core Yenilikleri",
                    Content = "Makale ile ilgili içerikler",
                    Thumbnail = "Default.jpg",
                    SeoDescription = ".Net Core Yenilikleri",
                    SeoTags = "C#,Net Core,Web,MVC",
                    SeoAuthor = "Eray Bakır",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "Initial Create",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "Initial Create",
                    ModifiedDate = DateTime.Now,
                    Note = ".Net Core Yenilikleriyle ilgili Makale",
                    UserId = 1,
                    ViewsCount = 100,
                    CommentsCount = 2
                });

        }
    }
}
