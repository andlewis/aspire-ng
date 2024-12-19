using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class Weather
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
