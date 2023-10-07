using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace AnyoneForTennis.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {


        // 添加其他 DbSet 配置
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<Coach> Coaches { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Schedule)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 添加 Seed Data 部分，这里包含用户和课程表的数据
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "user1-id",
                    UserName = "user1",
                    Email = "user1@example.com",
                    FirstName = "User1",
                    LastName = "LastName1",
                    Biography = "Bio for User 1"
                },
                new ApplicationUser
                {
                    Id = "user2-id",
                    UserName = "user2",
                    Email = "user2@example.com",
                    FirstName = "User2",
                    LastName = "LastName2",
                    Biography = "Bio for User 2"
                },
                new ApplicationUser
                {
                    Id = "user3-id",
                    UserName = "user3",
                    Email = "user3@example.com",
                    FirstName = "User3",
                    LastName = "LastName3",
                    Biography = "Bio for User 3"
                }
            );

            modelBuilder.Entity<Schedules>().HasData(
                new Schedules
                {
                    ScheduleId = 1,
                    EventName = "Tennis Event A",
                    CoachId = "user1-id",
                    Date = new DateTime(2023, 11, 30),
                    Location = "Location A",
                },
                new Schedules
                {
                    ScheduleId = 2,
                    EventName = "Tennis Event B",
                    CoachId = "user2-id",
                    Date = new DateTime(2023, 12, 30),
                    Location = "Location B",
                },
                new Schedules
                {
                    ScheduleId = 3,
                    EventName = "Tennis Event C",
                    CoachId = "user3-id",
                    Date = new DateTime(2023, 10, 30),
                    Location = "Location C",
                }
            );
            modelBuilder.Entity<Coach>().HasData(
                new Coach
                {
                    CoachId = "user1-id",
                    FirstName = "Coach1FirstName",
                    LastName = "Coach1LastName",
                    Biography = "Bio for Coach 1"
                },
                new Coach
                {
                    CoachId = "user2-id",
                    FirstName = "Coach2FirstName",
                    LastName = "Coach2LastName",
                    Biography = "Bio for Coach 2"
                },
                new Coach
                {
                    CoachId = "user3-id",
                    FirstName = "Coach3FirstName",
                    LastName = "Coach3LastName",
                    Biography = "Bio for Coach 3"
                }
                // 继续添加更多 Coach 数据
            );

        }
    }
}
