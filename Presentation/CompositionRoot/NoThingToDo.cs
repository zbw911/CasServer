// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/CompositionRoot/NoThingToDo.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using Application.EntityDtoProfile;

namespace CompositionRoot
{
    /// <summary>
    ///   这个方法本身没有任何的意思,也不参与任何的操作,只是为了可以进行有效的编译,
    ///   如果把这个方法或这个类去掉,那在编译的时候,项目中总不能得到最新版本,这是为什么呢?
    ///   先记下,如果有人有更好的解决方法,请EMail给我,zbw911@gmail.com
    ///   实际上还应该有另一个解决方法, 在子项目中做一个 Attribute, Useage.Assembly ,
    ///   这样做也应该是可以的,
    ///   对于间接引用的项目,没有代码的引用,所以极有可能,VS.net的编译器将这个"多余的dll"优化掉了.
    ///   http://www.cnblogs.com/zbw911/archive/2013/02/27/2934461.html
    /// </summary>
    internal class NoThingToDo
    {
        #region C'tors

        public NoThingToDo()
        {
            HookNewAlway.Hookit();
        }

        #endregion
    }
}