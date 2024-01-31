﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQ.WatermarkWebApp.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    [Column(TypeName ="decimal(18,2)")]
    public decimal Price { get; set; }
    [Range(0,100)]
    public int Stock { get; set; }
    public string ImageName { get; set; }
}
