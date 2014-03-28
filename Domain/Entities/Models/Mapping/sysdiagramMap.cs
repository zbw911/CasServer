// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/sysdiagramMap.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class sysdiagramMap : EntityTypeConfiguration<sysdiagram>
    {
        #region C'tors

        public sysdiagramMap()
        {
            // Primary Key
            this.HasKey(t => t.diagram_id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("sysdiagrams");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.principal_id).HasColumnName("principal_id");
            this.Property(t => t.diagram_id).HasColumnName("diagram_id");
            this.Property(t => t.version).HasColumnName("version");
            this.Property(t => t.definition).HasColumnName("definition");
        }

        #endregion
    }
}