namespace Backend.Model
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contian {1} characteres length")]
        [Index("Product_Description_Index", IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]        
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        
        public decimal Price { get; set; }

        [Display(Name = "Is Active?")]
        [JsonIgnore]
        public bool IsActive { get; set; }

        [Display(Name = "Last Purchase")]
        [DataType(DataType.Date)]        
        public DateTime LastPurchase { get; set; }

        
        public double Sctock { get; set; }


        [DataType(DataType.MultilineText)]
        
        public string Remarks { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
