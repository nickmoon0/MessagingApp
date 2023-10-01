﻿// <auto-generated />
using System;
using MessagingApp.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MessagingApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230924133758_RemovedRequestingUserId")]
    partial class RemovedRequestingUserId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MessagingApp.Domain.Aggregates.FriendRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FromUserId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("FriendRequest", (string)null);
                });

            modelBuilder.Entity("MessagingApp.Domain.Aggregates.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("MessagingApp.Domain.Aggregates.FriendRequest", b =>
                {
                    b.HasOne("MessagingApp.Domain.Aggregates.User", "FromUser")
                        .WithMany("SentFriendRequests")
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MessagingApp.Domain.Aggregates.User", "ToUser")
                        .WithMany("ReceivedFriendRequests")
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("MessagingApp.Domain.Aggregates.User", b =>
                {
                    b.HasOne("MessagingApp.Domain.Aggregates.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MessagingApp.Domain.Aggregates.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("ReceivedFriendRequests");

                    b.Navigation("SentFriendRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
