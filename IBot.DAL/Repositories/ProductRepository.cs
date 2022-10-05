using System.Reflection;
using AutoMapper;
using IBot.Core.Entities.Products;
using IBot.Core.Interfaces.Repositories;
using IBot.DAL.Context;
using IBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IBot.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Product Map(ProductModel model)
    {
        var product = _mapper.Map<Product>(model);
        var x = product.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(product, model.Id);

        return product;
    }

    public async Task<Product?> GetAsync(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(productModel => productModel.Id == id);
        return product is null ? null : Map(product);
    }

    public async Task AddAsync(Product entity)
    {
        var product = new ProductModel();
        _mapper.Map(entity, product);
        await _context.Products.AddAsync(product);
    }

    public Task DeleteAsync(Product entity)
    {
        var product = _context.Products.First(productModel => productModel.Id == entity.Id);
        _context.Products.Remove(product);
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(Product entity)
    {
        var product = await _context.Products.FirstAsync(productModel => productModel.Id == entity.Id);
        _mapper.Map(entity, product);
    }

    public async Task<List<Product>> Find(int skip, int take)
    {
        var products = await _context.Products.OrderBy(x => x.Id).Skip(skip).Take(take).ToListAsync();
        return products.Select(Map).ToList();
    }

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<Product, ProductModel>().ReverseMap();
    }));
}