// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.EntityDtoProfile/MyProfile.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Application.Dto;
using Application.Dto.User;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.EntityDtoProfile
{
    public class MyProfile : Profile
    {
        #region Instance Methods

        protected override void Configure()
        {
            //UserProfile实体类
            var userView = Mapper.CreateMap<UserProfile, UserProfileModel>();
            userView.ReverseMap();

            var map = Mapper.CreateMap<UserProfile, MyInfo>();
        }

        #endregion
    }
}