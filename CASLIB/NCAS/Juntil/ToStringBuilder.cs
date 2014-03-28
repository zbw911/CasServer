using System;
using System.Linq.Expressions;
using System.Text;

namespace NCAS.Juntil
{
    // from :http://www.google.com.hk/url?sa=t&rct=j&q=&esrc=s&frm=1&source=web&cd=4&ved=0CEYQFjAD&url=%68%74%74%70%3a%2f%2f%73%74%61%63%6b%6f%76%65%72%66%6c%6f%77%2e%63%6f%6d%2f%71%75%65%73%74%69%6f%6e%73%2f%32%34%31%37%36%34%37%2f%69%73%2d%74%68%65%72%65%2d%61%6e%2d%65%71%75%69%76%61%6c%65%6e%74%2d%74%6f%2d%6a%61%76%61%73%2d%74%6f%73%74%72%69%6e%67%62%75%69%6c%64%65%72%2d%66%6f%72%2d%63%2d%77%68%61%74%2d%77%6f%75%6c%64%2d%61%2d%67%6f%6f%64%2d%63%2d%73%68%61&ei=z9LCUYDiL4SAkwWDyIHgCw&usg=AFQjCNH1-2fmzgoQCqsSygQ7isMYzEMqBQ&sig2=74Lq9nFPNA3njTz5axefNg&bvm=bv.48175248,d.dGI&cad=rjt
    // forgive the mangled code; I hate horizontal scrolling
    public sealed class ToStringBuilder<T>
    {
        private T _obj;
        private Type _objType;
        private StringBuilder _innerSb;

        public ToStringBuilder(T obj)
        {
            this._obj = obj;
            this._objType = obj.GetType();
            this._innerSb = new StringBuilder();
        }

        public ToStringBuilder<T> Append<TProperty>
        (Expression<Func<T, TProperty>> expression)
        {

            string propertyName;
            if (!TryGetPropertyName(expression, out propertyName))
                throw new ArgumentException(
                    "Expression must be a simple property expression."
                );

            Func<T, TProperty> func = expression.Compile();

            if (this._innerSb.Length < 1)
                this._innerSb.Append(
                    propertyName + ": " + func(this._obj).ToString()
                );
            else
                this._innerSb.Append(
                    ", " + propertyName + ": " + func(this._obj).ToString()
                );

            return this;
        }

        private static bool TryGetPropertyName<TProperty>
        (Expression<Func<T, TProperty>> expression, out string propertyName)
        {

            propertyName = default(string);

            var propertyExpression = expression.Body as MemberExpression;
            if (propertyExpression == null)
                return false;

            propertyName = propertyExpression.Member.Name;
            return true;
        }

        public override string ToString()
        {
            return this._objType.Name + "{" + this._innerSb.ToString() + "}";
        }
    }
}
