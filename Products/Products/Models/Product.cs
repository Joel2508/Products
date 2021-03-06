﻿namespace Products.Models
{
    using System;
    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastPurchase { get; set; }
        public double Sctock { get; set; }
        public string Remarks { get; set; }
        public string Image { get; set; }

        public string FullImage
        {
            get
            {
                return string.Format("http://products.somee.com/{0}", Image.Substring(1));
            }
        }
    }
}
