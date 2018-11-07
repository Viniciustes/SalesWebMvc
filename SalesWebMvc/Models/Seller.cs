using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public Seller() { }

        public Seller(string name, string email, double baseSalary, DateTime birthDate, Department department)
        {
            Name = name;
            Email = email;
            BaseSalary = baseSalary;
            BirthDate = birthDate;
            Department = department;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        [Display(Name = "Base Salary")]
        [Required(ErrorMessage = "{0} required")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Range(100.00, 15000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        public double BaseSalary { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        public Department Department { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public void AddSales(SalesRecord sale)
        {
            Sales.Add(sale);
        }

        public void RemoveSales(SalesRecord sale)
        {
            Sales.Remove(sale);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(x => x.Date.Date >= initial.Date && x.Date.Date <= final.Date).Sum(y => y.Amount);
        }
    }
}
