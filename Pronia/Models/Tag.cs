﻿using Pronia.Models.Entity;

namespace Pronia.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public List<ProductTag> ProductTags { get; set; }

    }
}
