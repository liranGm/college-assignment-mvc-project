﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using college_assignment_mvc_project.Models;

namespace college_assignment_mvc_project.Migrations
{
    [DbContext(typeof(college_assignment_mvc_projectContext))]
    [Migration("20181023191456_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("college_assignment_mvc_project.Models.Guide", b =>
                {
                    b.Property<int>("GuideID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("PricePerDay");

                    b.Property<int>("Rate");

                    b.HasKey("GuideID");

                    b.ToTable("Guide");
                });

            modelBuilder.Entity("college_assignment_mvc_project.Models.Track", b =>
                {
                    b.Property<int>("TrackID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Circular");

                    b.Property<int>("Difficulty");

                    b.Property<int>("Duration");

                    b.Property<int>("GuideId");

                    b.Property<bool>("Includes_Water");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<int>("TrackLenght");

                    b.HasKey("TrackID");

                    b.HasIndex("GuideId");

                    b.ToTable("Track");
                });

            modelBuilder.Entity("college_assignment_mvc_project.Models.Track", b =>
                {
                    b.HasOne("college_assignment_mvc_project.Models.Guide", "Guide")
                        .WithMany("Tracks")
                        .HasForeignKey("GuideId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}