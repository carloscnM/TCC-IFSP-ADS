﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResearchData.Portal.Data.Contextos;

namespace ResearchData.Portal.Migrations.RDContextoLogMigrations
{
    [DbContext(typeof(RDContextoLog))]
    [Migration("20191204033931_LOGV2")]
    partial class LOGV2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ResearchData.Portal.Models.Negocio.Logs.AppLog", b =>
                {
                    b.Property<Guid>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("log_id");

                    b.Property<DateTime>("Captura")
                        .HasColumnName("log_captura");

                    b.Property<string>("Mensagem")
                        .HasColumnName("log_message");

                    b.HasKey("LogId");

                    b.ToTable("RESEARCHLOG");
                });
#pragma warning restore 612, 618
        }
    }
}