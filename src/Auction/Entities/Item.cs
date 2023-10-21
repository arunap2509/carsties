namespace Auction.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }


    public Auctions Auctions { get; set; }
    public Guid AuctionsId { get; set; }
}
