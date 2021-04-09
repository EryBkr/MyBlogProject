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



        }
    }
}
