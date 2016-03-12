using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public enum Render_Blend
    {
        BLEND_COLORMUL_ALPHAADD = 0,
        BLEND_COLORADD_ALPHAADD,
        BLEND_COLORMUL_ALPHABLEND,
        BLEND_COLORADD_ALPHABLEND,
    }

    public class Render_Line
    {
        //public Render_Blend blend_ = new Render_Blend();

        //public const int count_ = 2;
        //public Vertex[] vertex_ = Arrays.InitializeWithDefaultInstances<Vertex>(count_);

        //public Render_Texture texture_;

        //public Render_Line()
        //{
        //    blend_ = BLEND_COLORMUL_ALPHABLEND;
        //    texture_ = 0;
        //}
    }

    //----------------------------------------------------------------------------------------
    //	Copyright © 2006 - 2013 Tangible Software Solutions Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class provides the ability to initialize array elements with the default
    //	constructions for the array type.
    //----------------------------------------------------------------------------------------
    internal static class Arrays
    {
        internal static T[] InitializeWithDefaultInstances<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = new T();
            }
            return array;
        }
    }
}
