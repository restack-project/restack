﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReStack.Persistence;

#nullable disable

namespace ReStack.Persistence.PostgresMigrations.Migrations
{
    [DbContext(typeof(ReStackDbContext))]
    [Migration("20240429141316_20240429_2")]
    partial class _20240429_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ReStack.Domain.Entities.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ComponentLibraryId")
                        .HasColumnType("integer");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FolderName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ComponentLibraryId");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.ComponentLibrary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeOwners")
                        .HasColumnType("text");

                    b.Property<string>("Documentation")
                        .HasColumnType("text");

                    b.Property<string>("LastHashCommit")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ComponentLibrary");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.ComponentParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ComponentId")
                        .HasColumnType("integer");

                    b.Property<int>("DataType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ComponentId");

                    b.ToTable("ComponentParameter");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Ended")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Sequence")
                        .HasColumnType("integer");

                    b.Property<int>("StackId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Started")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("TriggerBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("StackId");

                    b.ToTable("Job");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Error")
                        .HasColumnType("boolean");

                    b.Property<int>("JobId")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Stack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AverageRuntime")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("SuccesPercentage")
                        .HasColumnType("numeric");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Stack");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.StackComponent", b =>
                {
                    b.Property<int>("StackId")
                        .HasColumnType("integer");

                    b.Property<int>("ComponentId")
                        .HasColumnType("integer");

                    b.HasKey("StackId", "ComponentId");

                    b.HasIndex("ComponentId");

                    b.ToTable("StackComponent");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Component", b =>
                {
                    b.HasOne("ReStack.Domain.Entities.ComponentLibrary", "ComponentLibrary")
                        .WithMany("Components")
                        .HasForeignKey("ComponentLibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ComponentLibrary");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.ComponentParameter", b =>
                {
                    b.HasOne("ReStack.Domain.Entities.Component", "Component")
                        .WithMany("Parameters")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Job", b =>
                {
                    b.HasOne("ReStack.Domain.Entities.Stack", "Stack")
                        .WithMany("Jobs")
                        .HasForeignKey("StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Log", b =>
                {
                    b.HasOne("ReStack.Domain.Entities.Job", "Job")
                        .WithMany("Logs")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.StackComponent", b =>
                {
                    b.HasOne("ReStack.Domain.Entities.Component", "Component")
                        .WithMany("Stacks")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReStack.Domain.Entities.Stack", "Stack")
                        .WithMany("Components")
                        .HasForeignKey("StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Component", b =>
                {
                    b.Navigation("Parameters");

                    b.Navigation("Stacks");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.ComponentLibrary", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Job", b =>
                {
                    b.Navigation("Logs");
                });

            modelBuilder.Entity("ReStack.Domain.Entities.Stack", b =>
                {
                    b.Navigation("Components");

                    b.Navigation("Jobs");
                });
#pragma warning restore 612, 618
        }
    }
}
