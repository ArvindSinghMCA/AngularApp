using System.Collections.Generic;
using AngularApp.Server.Models;
using Microsoft.EntityFrameworkCore;
namespace AngularApp.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
}

