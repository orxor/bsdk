﻿using System;
using System.ComponentModel;
using System.Windows;

namespace BinaryStudio.PlatformUI
    {
    public interface ITypeEditor
        {
        Object EditValue(ITypeDescriptorContext context, DependencyObject owner, Object value);
        }
    }