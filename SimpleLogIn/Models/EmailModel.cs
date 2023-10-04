using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SimpleLogIn.Models
{
    
    public class EmailModel
    {
        [Key]
        public int Id { get; set; }
        
       
        public string? FullName { get; set; }
        
         
        public  string? UserEmail { get; set; }
        public string? Password { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }

       

    }
}
