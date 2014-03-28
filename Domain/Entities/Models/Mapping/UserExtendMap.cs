// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/UserExtendMap.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class UserExtendMap : EntityTypeConfiguration<UserExtend>
    {
        #region C'tors

        public UserExtendMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Uid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasPrecision(11, 0);

            // Table & Column Mappings
            this.ToTable("UserExtend");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Uid).HasColumnName("Uid");
        }

        #endregion
    }
}