﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

namespace Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210503040042_AddTravelPlanStatus")]
    partial class AddTravelPlanStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<Guid>("TravelPlanActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TravelPlanActivityId")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Domain.Models.PlanInvitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InvitedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("InviteeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TravelPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasAlternateKey("InvitedById", "InviteeId", "TravelPlanId");

                    b.HasIndex("TravelPlanId");

                    b.ToTable("PlanInvitations");
                });

            modelBuilder.Entity("Domain.Models.TravelPlan", b =>
                {
                    b.Property<Guid>("TravelPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TravelPlanStatusId")
                        .HasColumnType("int");

                    b.HasKey("TravelPlanId");

                    b.ToTable("TravelPlans");
                });

            modelBuilder.Entity("Domain.Models.TravelPlanActivity", b =>
                {
                    b.Property<Guid>("TravelPlanActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TravelPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TravelPlanActivityId");

                    b.HasIndex("TravelPlanId");

                    b.ToTable("TravelPlanActivities");
                });

            modelBuilder.Entity("Domain.Models.TravelPlanStatus", b =>
                {
                    b.Property<int>("TravelPlanStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TravelPlanStatusId");

                    b.ToTable("TravelPlanStatuses");
                });

            modelBuilder.Entity("Domain.Models.UserTravelPlan", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TravelPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "TravelPlanId");

                    b.HasIndex("TravelPlanId");

                    b.ToTable("UserTravelPlans");
                });

            modelBuilder.Entity("Domain.Models.Location", b =>
                {
                    b.HasOne("Domain.Models.TravelPlanActivity", "TravelPlanActivity")
                        .WithOne("Location")
                        .HasForeignKey("Domain.Models.Location", "TravelPlanActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.PlanInvitation", b =>
                {
                    b.HasOne("Domain.Models.TravelPlan", "TravelPlan")
                        .WithMany()
                        .HasForeignKey("TravelPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.TravelPlanActivity", b =>
                {
                    b.HasOne("Domain.Models.TravelPlan", "TravelPlan")
                        .WithMany("TravelPlanActivities")
                        .HasForeignKey("TravelPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.UserTravelPlan", b =>
                {
                    b.HasOne("Domain.Models.TravelPlan", "TravelPlan")
                        .WithMany("UserTravelPlans")
                        .HasForeignKey("TravelPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
