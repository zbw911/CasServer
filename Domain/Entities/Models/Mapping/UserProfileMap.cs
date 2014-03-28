// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Domain.Entities/UserProfileMap.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        #region C'tors

        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasMaxLength(20);

            this.Property(t => t.NickName)
                .HasMaxLength(20);

            this.Property(t => t.ProvinceName)
                .HasMaxLength(100);

            this.Property(t => t.CityName)
                .HasMaxLength(100);

            this.Property(t => t.PhonePasswordResetToken)
                .HasMaxLength(10);

            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.Avator)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.NickName).HasColumnName("NickName");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.ProvinceName).HasColumnName("ProvinceName");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.PhonePasswordResetToken).HasColumnName("PhonePasswordResetToken");
            this.Property(t => t.PhonePasswordResetTokenExpirationDate).HasColumnName(
                "PhonePasswordResetTokenExpirationDate");
            this.Property(t => t.PhonePasswordResendCount).HasColumnName("PhonePasswordResendCount");
            this.Property(t => t.LastPhonePasswordResetTokenTime).HasColumnName("LastPhonePasswordResetTokenTime");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Avator).HasColumnName("Avator");
            this.Property(t => t.IsConfirmEmail).HasColumnName("IsConfirmEmail");
            this.Property(t => t.IsConfirmPhone).HasColumnName("IsConfirmPhone");
        }

        #endregion
    }
}