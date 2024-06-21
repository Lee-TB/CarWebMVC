using System.ComponentModel;

namespace CarWebMVC.Models;

public class EngineType
{
    public int Id { get; set; }
    [DisplayName("Loại động cơ")]
    public string Name { get; set; }
}