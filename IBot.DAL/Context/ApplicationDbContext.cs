using IBot.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IBot.DAL.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<ProductModel> Products { get; set; } = null!;
    public DbSet<TransactionModel> Transactions { get; set; } = null!;
    public DbSet<UserProductModel> UserProducts { get; set; } = null!;
    public DbSet<UserTransactionModel> UserTransactions { get; set; } = null!;
}