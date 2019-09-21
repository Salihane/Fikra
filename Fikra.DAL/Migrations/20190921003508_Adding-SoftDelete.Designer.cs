﻿// <auto-generated />
using System;
using Fikra.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fikra.DAL.Migrations
{
    [DbContext(typeof(FikraContext))]
    [Migration("20190921003508_Adding-SoftDelete")]
    partial class AddingSoftDelete
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fikra.Model.Entities.Dashboard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Dashboards");
                });

            modelBuilder.Entity("Fikra.Model.Entities.Effort", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Completed");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<double>("Estimated");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<double>("Remaining");

                    b.HasKey("Id");

                    b.ToTable("Efforts");
                });

            modelBuilder.Entity("Fikra.Model.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int?>("DashboardId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DashboardId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Fikra.Model.Entities.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTime?>("Due");

                    b.Property<Guid?>("EffortId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<int>("Priority");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("EffortId");

                    b.ToTable("Task");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Task");
                });

            modelBuilder.Entity("Fikra.Model.Entities.TaskComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<Guid>("TaskId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskComments");
                });

            modelBuilder.Entity("Fikra.Model.Entities.DashboardTask", b =>
                {
                    b.HasBaseType("Fikra.Model.Entities.Task");

                    b.Property<int>("DashboardId");

                    b.HasIndex("DashboardId");

                    b.HasDiscriminator().HasValue("DashboardTask");
                });

            modelBuilder.Entity("Fikra.Model.Entities.ProjectTask", b =>
                {
                    b.HasBaseType("Fikra.Model.Entities.Task");

                    b.Property<int>("ProjectId");

                    b.HasIndex("ProjectId");

                    b.HasDiscriminator().HasValue("ProjectTask");
                });

            modelBuilder.Entity("Fikra.Model.Entities.Project", b =>
                {
                    b.HasOne("Fikra.Model.Entities.Dashboard")
                        .WithMany("Projects")
                        .HasForeignKey("DashboardId");
                });

            modelBuilder.Entity("Fikra.Model.Entities.Task", b =>
                {
                    b.HasOne("Fikra.Model.Entities.Effort", "Effort")
                        .WithMany()
                        .HasForeignKey("EffortId");
                });

            modelBuilder.Entity("Fikra.Model.Entities.TaskComment", b =>
                {
                    b.HasOne("Fikra.Model.Entities.Task", "Task")
                        .WithMany("Comments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fikra.Model.Entities.DashboardTask", b =>
                {
                    b.HasOne("Fikra.Model.Entities.Dashboard", "Dashboard")
                        .WithMany("Tasks")
                        .HasForeignKey("DashboardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fikra.Model.Entities.ProjectTask", b =>
                {
                    b.HasOne("Fikra.Model.Entities.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
