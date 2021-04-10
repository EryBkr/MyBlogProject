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
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(70);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.CreatedByName).IsRequired();
            builder.Property(c => c.CreatedByName).HasMaxLength(50);
            builder.Property(c => c.ModifiedByName).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(50);
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.ModifiedDate).IsRequired();
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();
            builder.Property(c => c.Note).HasMaxLength(500);

            builder.ToTable("Categories");

            //Id dahil bütün dataları eksiksiz vermemiz gerekiyor.
            //HasData o tabloya ait data yoksa oluşacak
            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "C#",
                    Description = "C# Programlama Diliyle İlgili Bilgiler",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "Initial Create",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "Initial Create",
                    ModifiedDate = DateTime.Now,
                    Note = "C# Blog Kategorisi"
                },
                new Category
                {
                    Id = 2,
                    Name = ".Net Core MVC",
                    Description = ".Net Core MVC ile İlgili Bilgiler",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "Initial Create",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "Initial Create",
                    ModifiedDate = DateTime.Now,
                    Note = ".Net Core MVC Blog Kategorisi"
                },
                 new Category
                 {
                     Id = 3,
                     Name = "Javascript",
                     Description = "Javascript ile İlgili Bilgiler",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "Initial Create",
                     CreatedDate = DateTime.Now,
                     ModifiedByName = "Initial Create",
                     ModifiedDate = DateTime.Now,
                     Note = "Javascript Blog Kategorisi"
                 });



        }
    }
}
