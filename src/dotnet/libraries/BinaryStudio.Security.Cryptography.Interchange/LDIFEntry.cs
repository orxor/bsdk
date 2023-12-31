﻿using System;
using System.Text;

namespace BinaryStudio.Security.Cryptography.Interchange
    {
    public class LDIFEntry
        {
        public String Name { get; }
        public Object Value { get;internal set; }
        public Int64 LineNumber { get;internal set; }

        public LDIFEntry(String name) {
            Name = name;
            }

        public LDIFEntry() {
            }

        public static readonly LDIFEntry Empty = new LDIFEntry();

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            var r = new StringBuilder();
            r.Append(Name);
            r.Append(":");
            r.Append(Value);
            return r.ToString();
            }
        }
    }