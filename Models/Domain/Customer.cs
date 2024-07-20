using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;

public class Customer
{
    public int Id { get; set; }
    [DisplayName("Tên khách hàng")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Tên khách hàng phải từ 2 đến 50 ký tự")]
    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    public string Name { get; set; }
    [DisplayName("Số điện thoại")]
    [StringLength(20, ErrorMessage = "Số điện thoại không được quá 20 ký tự")]
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string PhoneNumber { get; set; }
    [DisplayName("Email")]
    [StringLength(50, ErrorMessage = "Email không được quá 50 ký tự")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string? Email { get; set; }
    [DisplayName("Địa chỉ")]
    [StringLength(100, ErrorMessage = "Địa chỉ không được quá 100 ký tự")]
    public string? Address { get; set; }
}