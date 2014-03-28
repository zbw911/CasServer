// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/webpages_UsersInRolesMap.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class webpages_UsersInRolesMap : EntityTypeConfiguration<webpages_UsersInRoles>
    {
        #region C'tors

        public webpages_UsersInRolesMap()
        {
            // Primary Key
            this.HasKey(t => new {t.UserId, t.RoleId});

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("webpages_UsersInRoles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");

            // Relationships
            this.HasRequired(t => t.UserExtend)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.UserProfile)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.webpages_Roles)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.RoleId);
        }

        #endregion
    }
}