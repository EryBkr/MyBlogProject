﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBlog.Data.Concrete.EntityFramework.Contexts;

namespace MyBlog.Data.Migrations
{
    [DbContext(typeof(MyBlogContext))]
    partial class MyBlogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyBlog.Entities.Concrete.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CommentsCount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SeoAuthor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SeoDescription")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("SeoTags")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            CommentsCount = 1,
                            Content = "Makale ile ilgili içerikler",
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 845, DateTimeKind.Local).AddTicks(6963),
                            Date = new DateTime(2021, 7, 6, 0, 16, 30, 845, DateTimeKind.Local).AddTicks(5817),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 845, DateTimeKind.Local).AddTicks(7564),
                            Note = "C# Yenilikleriyle ilgili Makale",
                            SeoAuthor = "Eray Bakır",
                            SeoDescription = "C# 9.0 Yenilikleri",
                            SeoTags = "C#,Net Core,Web,MVC",
                            Thumbnail = "Default.jpg",
                            Title = "C# 9.0 Yenilikleri",
                            UserId = 1,
                            ViewsCount = 50
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            CommentsCount = 2,
                            Content = "Makale ile ilgili içerikler",
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 845, DateTimeKind.Local).AddTicks(8932),
                            Date = new DateTime(2021, 7, 6, 0, 16, 30, 845, DateTimeKind.Local).AddTicks(8930),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 845, DateTimeKind.Local).AddTicks(8934),
                            Note = ".Net Core Yenilikleriyle ilgili Makale",
                            SeoAuthor = "Eray Bakır",
                            SeoDescription = ".Net Core Yenilikleri",
                            SeoTags = "C#,Net Core,Web,MVC",
                            Thumbnail = "Default.jpg",
                            Title = ".Net Core Yenilikleri",
                            UserId = 1,
                            ViewsCount = 100
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 850, DateTimeKind.Local).AddTicks(9672),
                            Description = "C# Programlama Diliyle İlgili Bilgiler",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 850, DateTimeKind.Local).AddTicks(9682),
                            Name = "C#",
                            Note = "C# Blog Kategorisi"
                        },
                        new
                        {
                            Id = 2,
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 850, DateTimeKind.Local).AddTicks(9694),
                            Description = ".Net Core MVC ile İlgili Bilgiler",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 850, DateTimeKind.Local).AddTicks(9695),
                            Name = ".Net Core MVC",
                            Note = ".Net Core MVC Blog Kategorisi"
                        },
                        new
                        {
                            Id = 3,
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 850, DateTimeKind.Local).AddTicks(9699),
                            Description = "Javascript ile İlgili Bilgiler",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 850, DateTimeKind.Local).AddTicks(9701),
                            Name = "Javascript",
                            Note = "Javascript Blog Kategorisi"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArticleId = 1,
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 849, DateTimeKind.Local).AddTicks(1943),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 849, DateTimeKind.Local).AddTicks(1953),
                            Note = "Makale Yorumu",
                            Text = "C# Yorumu"
                        },
                        new
                        {
                            Id = 2,
                            ArticleId = 2,
                            CreatedByName = "Initial Create",
                            CreatedDate = new DateTime(2021, 7, 6, 0, 16, 30, 849, DateTimeKind.Local).AddTicks(2004),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "Initial Create",
                            ModifiedDate = new DateTime(2021, 7, 6, 0, 16, 30, 849, DateTimeKind.Local).AddTicks(2005),
                            Note = "Makale Yorumu",
                            Text = ".Net Core MVC Yorumu"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "a494c847-c93e-4df4-aa06-8cefea66e0a2",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "0bb76b42-6577-4851-86b7-d7411943b0c0",
                            Name = "Editor",
                            NormalizedName = "EDITOR"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fff84967-d36c-4d71-a96c-ebe2a109073b",
                            Email = "adminuser@gmail.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMINUSER@GMAIL.COM",
                            NormalizedUserName = "ADMINUSER",
                            PasswordHash = "AQAAAAEAACcQAAAAEAiSwX/9X3cu8Py1oZgrVidyMKv4HfSNegDGN0P5y2Y44w0iQsnn8/FRlfM4XtGiWA==",
                            PhoneNumber = "5555555555555",
                            PhoneNumberConfirmed = true,
                            Picture = "defaultUser.png",
                            SecurityStamp = "104a2994-022d-4966-a2a0-6da982feef3e",
                            TwoFactorEnabled = false,
                            UserName = "adminuser"
                        },
                        new
                        {
                            Id = 2,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "86048a67-b4cb-4a1f-8370-2eb009180bb8",
                            Email = "editor@gmail.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "editor@GMAIL.COM",
                            NormalizedUserName = "EDITORUSER",
                            PasswordHash = "AQAAAAEAACcQAAAAEMQHP+TTHtT0GWCVy9AapcMKdLChvfNJazbzgHhUt0YGvreoqUcfwNIGD9tdVi0zog==",
                            PhoneNumber = "5555555555555",
                            PhoneNumberConfirmed = true,
                            Picture = "defaultUser.png",
                            SecurityStamp = "b6a90aad-1d00-4f13-8da9-d268580e1a67",
                            TwoFactorEnabled = false,
                            UserName = "editoruser"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 2,
                            RoleId = 2
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Article", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBlog.Entities.Concrete.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Comment", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.RoleClaim", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserClaim", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserLogin", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.UserRole", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBlog.Entities.Concrete.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Article", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Category", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.User", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
