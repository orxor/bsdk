// Copyright (C) Microsoft Corporation.  All rights reserved.

using System;

namespace BinaryStudio.PlatformUI.Extensions
    {
    internal class FloatUtil
        {
        internal static Single FLT_EPSILON = 1.192092896e-07F;
        internal static Single FLT_MAX_PRECISION = 0xffffff;
        internal static Single INVERSE_FLT_MAX_PRECISION = 1.0F / FLT_MAX_PRECISION;

        /// <summary>
        /// AreClose
        /// </summary>
        public static Boolean AreClose(Single a, Single b) {
            if (a == b) return true;
            // This computes (|a-b| / (|a| + |b| + 10.0f)) < FLT_EPSILON
            Single eps = ((Single)Math.Abs(a) + (Single)Math.Abs(b) + 10.0f) * FLT_EPSILON;
            Single delta = a - b;
            return (-eps < delta) && (eps > delta);
            }

        /// <summary>
        /// IsOne
        /// </summary>
        public static Boolean IsOne(Single a)
            {
            return (Single)Math.Abs(a - 1.0f) < 10.0f * FLT_EPSILON;
            }

        /// <summary>
        /// IsZero
        /// </summary>
        public static Boolean IsZero(Single a)
            {
            return (Single)Math.Abs(a) < 10.0f * FLT_EPSILON;
            }

        /// <summary>
        /// IsCloseToDivideByZero
        /// </summary>
        public static Boolean IsCloseToDivideByZero(Single numerator, Single denominator)
            {
            return Math.Abs(denominator) <= Math.Abs(numerator) * INVERSE_FLT_MAX_PRECISION;
            }
        }
    }