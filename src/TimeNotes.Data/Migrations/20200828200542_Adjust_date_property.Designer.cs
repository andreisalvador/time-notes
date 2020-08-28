﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TimeNotes.Data;

namespace TimeNotes.Data.Migrations
{
    [DbContext(typeof(TimeNotesContext))]
    [Migration("20200828200542_Adjust_date_property")]
    partial class Adjust_date_property
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TimeNotes.Domain.HourPointConfigurations", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("LunchTime")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("OfficeHour")
                        .HasColumnType("interval");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("WorkDays")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("HourPointConfigurations");
                });

            modelBuilder.Entity("TimeNotes.Domain.HourPoints", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<TimeSpan>("ExtraTime")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("MissingTime")
                        .HasColumnType("interval");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("HourPoints");
                });

            modelBuilder.Entity("TimeNotes.Domain.TimeEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateHourPointed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("HourPointsId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("HourPointsId");

                    b.ToTable("TimeEntries");
                });

            modelBuilder.Entity("TimeNotes.Domain.TimeEntry", b =>
                {
                    b.HasOne("TimeNotes.Domain.HourPoints", "HourPoints")
                        .WithMany("TimeEntries")
                        .HasForeignKey("HourPointsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
