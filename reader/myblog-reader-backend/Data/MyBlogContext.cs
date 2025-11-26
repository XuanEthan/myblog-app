using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using myblog_reader_backend.Models;

namespace myblog_reader_backend.Data;

public partial class MyBlogContext : DbContext
{
    public MyBlogContext(DbContextOptions<MyBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasMany(d => d.Posts).WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryPost",
                    r => r.HasOne<Post>().WithMany().HasForeignKey("PostsId"),
                    l => l.HasOne<Category>().WithMany().HasForeignKey("CategoriesId"),
                    j =>
                    {
                        j.HasKey("CategoriesId", "PostsId");
                        j.ToTable("CategoryPost");
                        j.HasIndex(new[] { "PostsId" }, "IX_CategoryPost_PostsId");
                    });
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasMany(d => d.Tags).WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagsId"),
                    l => l.HasOne<Post>().WithMany().HasForeignKey("PostsId"),
                    j =>
                    {
                        j.HasKey("PostsId", "TagsId");
                        j.ToTable("PostTag");
                        j.HasIndex(new[] { "TagsId" }, "IX_PostTag_TagsId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
