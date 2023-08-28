﻿// <auto-generated />
using System;
using GIC.BANKACCOUNT.DATA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GIC.BANKACCOUNT.DATA.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230827173039_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GIC.BANKACCOUNT.DATA.Entities.Account", b =>
                {
                    b.Property<int>("AcccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AcccountId"));

                    b.Property<string>("AcccountNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("AcccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("GIC.BANKACCOUNT.DATA.Entities.IntrestRule", b =>
                {
                    b.Property<int>("IntrestRuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IntrestRuleId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("Rate")
                        .HasPrecision(38, 2)
                        .HasColumnType("decimal(38,2)");

                    b.Property<string>("RuleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IntrestRuleId");

                    b.HasIndex("RuleId")
                        .IsUnique();

                    b.ToTable("IntrestRules");
                });

            modelBuilder.Entity("GIC.BANKACCOUNT.DATA.Entities.RunningNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DateStr")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DateStr")
                        .IsUnique();

                    b.ToTable("RunningNumbers");
                });

            modelBuilder.Entity("GIC.BANKACCOUNT.DATA.Entities.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasPrecision(38, 2)
                        .HasColumnType("decimal(38,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime");

                    b.Property<string>("TransactionNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength();

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("TransactionNo")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("GIC.BANKACCOUNT.DATA.Entities.Transaction", b =>
                {
                    b.HasOne("GIC.BANKACCOUNT.DATA.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GIC.BANKACCOUNT.DATA.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}