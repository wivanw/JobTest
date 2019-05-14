using System;
using System.Collections.Generic;

// <copyright file="Binder.cs" company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace Core.VBCM.Helper
{
    public static class EngineHelper
    {
        public static bool IsNull(this object obj)
        {
            return null == obj || obj.Equals(null);
        }
    }

    public sealed class WeakReferenceEqualityComparer : EqualityComparer<WeakReference>
    {
        public override bool Equals(WeakReference x, WeakReference y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            //=======================================

            var isGetX = x.IsAlive;
            var isGetY = y.IsAlive;
            
            var xTarget = x.Target;
            var yTarget = y.Target;

            if (ReferenceEquals(xTarget, yTarget)) return true;
            if (!isGetX || !isGetY) return false;
            if (xTarget.GetType() != yTarget.GetType()) return false;

            return xTarget.Equals(yTarget);
        }

        public override int GetHashCode(WeakReference obj)
        {
            if (obj == null)
                return 0;

            var isGet = obj.IsAlive;
            return isGet ? obj.Target.GetHashCode() : 0;
        }

        public static readonly EqualityComparer<WeakReference> BindsComparer = new WeakReferenceEqualityComparer();
    }
}