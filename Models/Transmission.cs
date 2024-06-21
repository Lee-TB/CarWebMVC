using System.ComponentModel;

namespace CarWebMVC.Models;

public class Transmission
{
    public int Id { get; set; }
    [DisplayName("Loại hộp số")]
    public string Name { get; set; }
    [DisplayName("Mô tả")]
    public string? Description { get; set; }
}