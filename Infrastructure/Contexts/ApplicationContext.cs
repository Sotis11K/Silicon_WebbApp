﻿using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<SavedCourseEntity> SavedCourses { get; set; }
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<RegisteredCoursesEntity> RegisteredCourses { get; set; }





}
