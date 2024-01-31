using System;
using System.Collections.Generic;

namespace RabbitMQ.FileExcelCreateService.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ContentSummary { get; set; } = null!;
        public string ContentMain { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public string? Picture { get; set; }
        public int CategoryId { get; set; }
        public int ViewCount { get; set; }
    }
}
