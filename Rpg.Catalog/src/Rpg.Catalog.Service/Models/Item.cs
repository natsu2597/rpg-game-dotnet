namespace Rpg.Catalog.Service.Models;

public class Item
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public Item() { }

    public Item(Guid id, string name, string description, decimal price, DateTimeOffset createdDate)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        CreatedDate = createdDate;
    }
}