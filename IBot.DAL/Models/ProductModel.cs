namespace IBot.DAL.Models;

public class ProductModel
{
    public Guid Id { get; set; }
    public string PreviewId { get; set;} = null!;
    public string DataId { get; set;} = null!;
    public string Name { get; set;} = null!;
    public decimal Cost { get; set;}
}