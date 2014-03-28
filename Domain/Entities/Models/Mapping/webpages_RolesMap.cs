// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/webpages_RolesMap.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class webpages_RolesMap : EntityTypeConfiguration<webpages_Roles>
    {
        #region C'tors

        public webpages_RolesMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleId);

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("webpages_Roles");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
        }

        #endregion
    }
}