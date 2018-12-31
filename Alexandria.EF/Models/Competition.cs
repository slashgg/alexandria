using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alexandria.EF.Models
{
  public class Competition : BaseEntity
  {
    [MaxLength(100)]
    public string Name { get; set; }
    public string Slug { get; set; }
    [MaxLength(500)]
    public string Title { get; set; }
    public string Description { get; set; }
    public string TitleCardImageURL { get; set; }
    public bool Active { get; set; } = false;
    public int? MaxTeamSize { get; set; }
    public int MinTeamSize { get; set; } = 1;
    public string RulesSlug { get; set; }

    /* Foreign Keys */
    public Guid GameId { get; set; }
    public Guid? DefaultRoleId { get; set; }



    /* Relations */
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
    [ForeignKey("DefaultRoleId")]
    public virtual TeamRole DefaultRole { get; set; }
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    public virtual ICollection<TeamRole> TeamRoles { get; set; } = new List<TeamRole>();
  }
}
