﻿using IntivePatronageLibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IntivePatronageLibraryAPI.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        //Forgot a 's' D:
        public DbSet<Author> Author { get; set; }

    }
}
