// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/FilterConfig.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web.Mvc;
using Dev.Comm.Web.Mvc.Filter;

namespace CASServer
{
    public class FilterConfig
    {
        #region Class Methods

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());

            filters.Add(new ErrorFilter());
        }

        #endregion
    }
}