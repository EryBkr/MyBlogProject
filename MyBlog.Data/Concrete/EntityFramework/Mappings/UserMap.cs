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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(50);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Username).IsRequired();
            builder.Property(u => u.Username).HasMaxLength(20);
            builder.HasIndex(u => u.Username).IsUnique();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.PasswordHash).HasColumnType("VARBINARY(500)");
            builder.Property(u => u.Description).HasMaxLength(500);
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.FirstName).HasMaxLength(30);
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(30);
            builder.Property(u => u.Picture).IsRequired();
            builder.Property(u => u.Picture).HasMaxLength(250);
            builder.Property(u => u.CreatedByName).IsRequired();
            builder.Property(u => u.CreatedByName).HasMaxLength(50);
            builder.Property(u => u.ModifiedByName).IsRequired();
            builder.Property(u => u.ModifiedByName).HasMaxLength(50);
            builder.Property(u => u.CreatedDate).IsRequired();
            builder.Property(u => u.ModifiedDate).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.IsDeleted).IsRequired();
            builder.Property(u => u.Note).HasMaxLength(500);

            builder.HasOne<Role>(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            builder.ToTable("Users");

            //Id dahil bütün dataları eksiksiz vermemiz gerekiyor.
            //HasData o tabloya ait data yoksa oluşacak
            builder.HasData(new User 
            {
                Id=1,
                RoleId=1,
                FirstName="Eray",
                LastName="Bakır",
                Username="Blackerback",
                Email="eray.bkr94@gmail.com",
                Picture= "https://picsum.photos/150",
                IsActive =true,
                IsDeleted=false,
                CreatedByName="Initial Create",
                CreatedDate=DateTime.Now,
                ModifiedByName="Initial Create",
                ModifiedDate=DateTime.Now,
                Description="Admin Kullanıcısı",
                Note="Admin Kullanıcısı",
                PasswordHash= Encoding.ASCII.GetBytes("202cb962ac59075b964b07152d234b70") //123 ün MD5 ile şifrelennmiş hali
            });
        }
    }
}
