﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceQuotes.Infrastructure.Context;

namespace ServiceQuotes.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7542b6b8-638c-44c9-806b-0040667c32a9"),
                            Created = new DateTime(2021, 3, 1, 22, 46, 58, 919, DateTimeKind.Utc).AddTicks(9540),
                            Email = "manager@service-quotes.com",
                            PasswordHash = "$2a$11$RtlK2XAwpq2cY0EOnZlVJOpcM7BKnTbUNy50tZ14D57Og8iZcP5pi",
                            Role = 0
                        });
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("VatNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.CustomerAddress", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("CustomerId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("CustomerAddresses");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.EmployeeSpecialization", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SpecializationId")
                        .HasColumnType("uuid");

                    b.HasKey("EmployeeId", "SpecializationId");

                    b.HasIndex("SpecializationId");

                    b.ToTable("EmployeeSpecializations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.JobValuation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("HourlyRate")
                        .HasPrecision(7, 2)
                        .HasColumnType("numeric(7,2)");

                    b.Property<TimeSpan>("LaborHours")
                        .HasColumnType("time");

                    b.Property<string>("WorkType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("JobValuations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Material", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("ServiceRequestId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(7, 2)
                        .HasColumnType("numeric(7,2)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasPrecision(7, 2)
                        .HasColumnType("numeric(7,2)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("QuoteId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("QuoteId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Quote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ReferenceNumber")
                        .HasColumnType("integer");

                    b.Property<Guid>("ServiceRequestId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasPrecision(7, 2)
                        .HasColumnType("numeric(7,2)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceRequestId")
                        .IsUnique();

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("PlannedExecutionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId", "AddressId");

                    b.ToTable("ServiceRequests");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequestEmployee", b =>
                {
                    b.Property<Guid>("ServiceRequestId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.HasKey("ServiceRequestId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ServiceRequestEmployee");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequestJobValuation", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobValuationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ServiceRequestId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("EmployeeId", "JobValuationId", "ServiceRequestId");

                    b.HasIndex("JobValuationId");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("ServiceRequestJobValuations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Specialization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Account", b =>
                {
                    b.OwnsMany("ServiceQuotes.Domain.Entities.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<Guid>("AccountId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("CreatedByIp")
                                .HasColumnType("text");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("ReplacedByToken")
                                .HasColumnType("text");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("RevokedByIp")
                                .HasColumnType("text");

                            b1.Property<string>("Token")
                                .HasColumnType("text");

                            b1.HasKey("Id");

                            b1.HasIndex("AccountId");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner("Account")
                                .HasForeignKey("AccountId");

                            b1.Navigation("Account");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Customer", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.CustomerAddress", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Address", "Address")
                        .WithMany("CustomerAddresses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceQuotes.Domain.Entities.Customer", "Customer")
                        .WithMany("CustomerAddresses")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Employee", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Account", "Account")
                        .WithOne()
                        .HasForeignKey("ServiceQuotes.Domain.Entities.Employee", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.EmployeeSpecialization", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Employee", "Employee")
                        .WithMany("EmployeeSpecializations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceQuotes.Domain.Entities.Specialization", "Specialization")
                        .WithMany("EmployeeSpecializations")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Material", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("Materials")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Payment", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Customer", "Customer")
                        .WithMany("Payments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceQuotes.Domain.Entities.Quote", "Quote")
                        .WithMany("Payments")
                        .HasForeignKey("QuoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Quote");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Quote", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithOne()
                        .HasForeignKey("ServiceQuotes.Domain.Entities.Quote", "ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequest", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.CustomerAddress", "CustomerAddress")
                        .WithMany("ServiceRequests")
                        .HasForeignKey("CustomerId", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerAddress");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequestEmployee", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Employee", "Employee")
                        .WithMany("ServiceRequestEmployees")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceQuotes.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("ServiceRequestEmployees")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequestJobValuation", b =>
                {
                    b.HasOne("ServiceQuotes.Domain.Entities.Employee", "Employee")
                        .WithMany("ServiceRequestJobValuations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceQuotes.Domain.Entities.JobValuation", "JobValuation")
                        .WithMany("ServiceRequestJobValuations")
                        .HasForeignKey("JobValuationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceQuotes.Domain.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany("ServiceRequestJobValuations")
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("JobValuation");

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Address", b =>
                {
                    b.Navigation("CustomerAddresses");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Customer", b =>
                {
                    b.Navigation("CustomerAddresses");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.CustomerAddress", b =>
                {
                    b.Navigation("ServiceRequests");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Employee", b =>
                {
                    b.Navigation("EmployeeSpecializations");

                    b.Navigation("ServiceRequestEmployees");

                    b.Navigation("ServiceRequestJobValuations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.JobValuation", b =>
                {
                    b.Navigation("ServiceRequestJobValuations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Quote", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.ServiceRequest", b =>
                {
                    b.Navigation("Materials");

                    b.Navigation("ServiceRequestEmployees");

                    b.Navigation("ServiceRequestJobValuations");
                });

            modelBuilder.Entity("ServiceQuotes.Domain.Entities.Specialization", b =>
                {
                    b.Navigation("EmployeeSpecializations");
                });
#pragma warning restore 612, 618
        }
    }
}
