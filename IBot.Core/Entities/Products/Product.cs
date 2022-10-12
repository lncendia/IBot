namespace IBot.Core.Entities.Products;

public class Product
{
    public Product(string previewId, string dataId, string name, decimal cost)
    {
        Id = Guid.NewGuid();
        PreviewId = previewId;
        DataId = dataId;
        Name = name;
        if(cost <= 0)
            throw new ArgumentException("Cost cannot be less than 0");
        Cost = cost;
    }

    public Guid Id { get; }
    public string PreviewId { get; }
    public string DataId { get; }
    public string Name { get; }
    public decimal Cost { get; }
}