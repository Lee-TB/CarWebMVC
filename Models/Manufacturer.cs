using System.ComponentModel;

namespace CarWebMVC.Models;

public class Manufacturer
{
    public int Id { get; set; }
    [DisplayName("Hãng xe")]
    public string Name { get; set; }
    [DisplayName("Quốc gia")]
    public string? Country { get; set; }
    [DisplayName("Năm thành lập")]
    public int? FoundedYear { get; set; }
}