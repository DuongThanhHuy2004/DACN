using System;
using System.Collections.Generic;

namespace DACN.Models;

public partial class TbAccount
{
    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public string? LastLogin { get; set; }

    public bool IsActive { get; set; }

    public int? BlogManagerId { get; set; }

    public virtual TbCategory? BlogManager { get; set; }

    public virtual TbRole? Role { get; set; }

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();
}
