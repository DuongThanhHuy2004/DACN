﻿using System;
using System.Collections.Generic;

namespace DACN.Models;

public partial class TbCategory
{
    public int CategoryId { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? Description { get; set; }

    public int? Position { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? SeoKeywords { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<TbAccount> TbAccounts { get; set; } = new List<TbAccount>();

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();
}
