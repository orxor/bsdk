using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace BinaryStudio.DiagnosticServices.Tracing
    {
    public class TraceScope : IDisposable
        {
        private static readonly ReaderWriterLockSlim o = new ReaderWriterLockSlim();
        private static readonly CultureInfo culture = CultureInfo.InvariantCulture;
        private static SQLiteConnection connection;
        [ThreadStatic] private static Int32 Level;
        [ThreadStatic] private static Stack<Int32> Stack;
        private Int32 Identity { get;set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            using (new WriteLockScope(o)) {
                using (var c = connection.CreateCommand()) {
                    c.CommandText = @"INSERT INTO TraceInfo([EntryTime],[LeavePoint]) VALUES (@EntryTime,@LeavePoint)";
                    c.Parameters.AddWithValue("@EntryTime", DateTime.Now.Ticks);
                    c.Parameters.AddWithValue("@LeavePoint", Identity);
                    c.ExecuteNonQuery();
                    Level--;
                    Stack.Pop();
                    }
                }
            }

        #region M:Dispose<T>({ref}T)
        private static void Dispose<T>(ref T o)
            where T: IDisposable
            {
            if (o != null) {
                o.Dispose();
                o = default;
                }
            }
        #endregion

        public TraceScope()
            :this(new StackTrace(true).GetFrame(1))
            {
            }

        public TraceScope(Int64 size)
            :this(new StackTrace(true).GetFrame(1), size)
            {
            }

        public TraceScope(StackFrame identity) {
            if (identity == null) { throw new ArgumentNullException(nameof(identity)); }
            Push(identity,null);
            }

        public TraceScope(StackFrame identity, Int64 size) {
            if (identity == null) { throw new ArgumentNullException(nameof(identity)); }
            Push(identity,size);
            }

        public TraceScope(String identity) {
            if (identity == null) { throw new ArgumentNullException(nameof(identity)); }
            if (String.IsNullOrWhiteSpace(identity)) { throw new ArgumentOutOfRangeException(nameof(identity)); }
            Push(identity,null);
            }

        public TraceScope(String LongName,String ShortName) {
            if (LongName == null) { throw new ArgumentNullException(nameof(LongName)); }
            if (ShortName == null) { throw new ArgumentNullException(nameof(LongName)); }
            if (String.IsNullOrWhiteSpace(LongName)) { throw new ArgumentOutOfRangeException(nameof(LongName)); }
            if (String.IsNullOrWhiteSpace(ShortName)) { throw new ArgumentOutOfRangeException(nameof(ShortName)); }
            Push(LongName,null);
            }

        public TraceScope(String identity,Int64 size) {
            if (identity == null) { throw new ArgumentNullException(nameof(identity)); }
            if (String.IsNullOrWhiteSpace(identity)) { throw new ArgumentOutOfRangeException(nameof(identity)); }
            Push(identity,size);
            }

        private static Int32 GetParentId() {
            return (Stack.Count == 0)
                ? 0
                : Stack.Peek();
            }

        #region M:Push(String,Int64?)
        private void Push(String identity, Int64? size) {
            EnsureConnection();
            EnsureStack();
            using (new WriteLockScope(o)) {
                using (var c = connection.CreateCommand()) {
                    Level++;
                    c.CommandText = @"INSERT INTO TraceInfo([ThreadId],[LongName],[EntryTime],[Size],[Level],[ParentId]) VALUES (@ThreadId,@LongName,@EntryTime,@Size,@Level,@ParentId)";
                    c.Parameters.AddWithValue("@ThreadId", Thread.CurrentThread.ManagedThreadId);
                    c.Parameters.AddWithValue("@LongName", identity);
                    c.Parameters.AddWithValue("@EntryTime", DateTime.Now.Ticks);
                    c.Parameters.AddWithValue("@Size", (Object)size ?? DBNull.Value);
                    c.Parameters.AddWithValue("@Level", Level);
                    c.Parameters.AddWithValue("@ParentId", GetParentId());
                    c.ExecuteNonQuery();
                    Identity = (Int32)connection.LastInsertRowId;
                    Stack.Push(Identity);
                    }
                }
            }
        #endregion
        #region M:Push(StackFrame,Int64?)
        private void Push(StackFrame identity, Int64? size) {
            Push(ToString(identity.GetMethod()),identity.GetMethod().Name,size);
            }
        #endregion
        #region M:Push(String,String,Int64?)
        private void Push(String LongName,String ShortName, Int64? size) {
            EnsureConnection();
            EnsureStack();
            using (new WriteLockScope(o)) {
                using (var c = connection.CreateCommand()) {
                    Level++;
                    c.CommandText = @"INSERT INTO TraceInfo([ThreadId],[LongName],[ShortName],[EntryTime],[Size],[Level],[ParentId]) VALUES (@ThreadId,@LongName,@ShortName,@EntryTime,@Size,@Level,@ParentId)";
                    c.Parameters.AddWithValue("@ThreadId", Thread.CurrentThread.ManagedThreadId);
                    c.Parameters.AddWithValue("@LongName", LongName);
                    c.Parameters.AddWithValue("@ShortName", ShortName);
                    c.Parameters.AddWithValue("@EntryTime", DateTime.Now.Ticks);
                    c.Parameters.AddWithValue("@Size", (Object)size ?? DBNull.Value);
                    c.Parameters.AddWithValue("@Level", Level);
                    c.Parameters.AddWithValue("@ParentId", GetParentId());
                    c.ExecuteNonQuery();
                    Identity = (Int32)connection.LastInsertRowId;
                    Stack.Push(Identity);
                    }
                }
            }
        #endregion

        private class ReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public ReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterReadLock();
                }

            public void Dispose()
                {
                o.ExitReadLock();
                o = null;
                }
            }

        private class UpgradeableReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public UpgradeableReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterUpgradeableReadLock();
                }

            public void Dispose()
                {
                o.ExitUpgradeableReadLock();
                o = null;
                }
            }

        private class WriteLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public WriteLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterWriteLock();
                }

            public void Dispose()
                {
                o.ExitWriteLock();
                o = null;
                }
            }

        protected static IDisposable ReadLock(ReaderWriterLockSlim o)            { return new ReadLockScope(o);            }
        protected static IDisposable WriteLock(ReaderWriterLockSlim o)           { return new WriteLockScope(o);           }
        protected static IDisposable UpgradeableReadLock(ReaderWriterLockSlim o) { return new UpgradeableReadLockScope(o); }

        #region M:GetShortName(String,IDictionary<String,String>)
        private static String GetShortName(String value, IDictionary<String,String> shortnames) {
            if (shortnames.TryGetValue(value, out var r)) {
                if (!String.IsNullOrWhiteSpace(r)) {
                    return r;
                    }
                }
            var values = value.Split(new Char[]{'.' }, StringSplitOptions.RemoveEmptyEntries);
            return (values.Length > 0)
                ? values[values.Length - 1]
                : value;
            }
        #endregion
        #region M:EnsureConnection
        private static void EnsureConnection() {
            lock(o) {
                if (connection == null) {
                    var target = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), ".sqlite3");
                    if (!Directory.Exists(target)) {
                        var folder = Directory.CreateDirectory(target);
                        folder.Attributes |= FileAttributes.Hidden;
                        }
                    var filename = Path.Combine(target, $"trace-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.db");
                    connection = new SQLiteConnection($"DataSource={filename}");
                    connection.Open();
                    using (var c = connection.CreateCommand()) {
                        c.CommandText = @"
                        CREATE TABLE [TraceInfo]
                            (
                            [Id]         [INTEGER] PRIMARY KEY AUTOINCREMENT,
                            [ThreadId]   [INTEGER] NULL,
                            [LongName]   [text] NULL,
                            [ShortName]  [text] NULL,
                            [EntryTime]  [bigint] NOT NULL,
                            [Size]       [bigint] NULL,
                            [LeavePoint] [INTEGER] NULL,
                            [Level]      [INTEGER] NULL,
                            [ParentId]   [INTEGER] NULL,
                            [Session]    [INTEGER] NULL
                            )";
                        c.ExecuteNonQuery();
                        }
                    }
                }
            }
        #endregion
        #region M:EnsureStack
        private static void EnsureStack() {
            lock(o) {
                if (Stack == null) {
                    Stack = new Stack<Int32>();
                    }
                }
            }
        #endregion
        #region M:ToString(MethodBase):String
        protected static String ToString(MethodBase mi)
            {
            var r = new StringBuilder();
            if (mi.DeclaringType != null) {
                r.Append(mi.DeclaringType.Name);
                r.Append(".");
                }
            r.Append(mi.Name);
            var args = mi.GetParameters();
            if (args.Length > 0) {
                r.Append("(");
                #if NET35
                r.Append(String.Join(",", args.Select(i => ToString(i.ParameterType)).ToArray()));
                #else
                r.Append(String.Join(",", args.Select(i => ToString(i.ParameterType))));
                #endif
                r.Append(")");
                }
            return r.ToString();
            }
        #endregion
        #region M:ToString(Type):String
        protected static String ToString(Type type) {
            var r = new StringBuilder();
            #if !NET40 && !NET35
            if (type.IsConstructedGenericType) {
                var j = type.Name.IndexOf("`");
                r.Append(type.Name.Substring(0, j));
                r.Append('<');
                r.Append(String.Join(",", type.GenericTypeArguments.Select(ToString)));
                r.Append('>');
                }
            #else
            if (!type.IsGenericTypeDefinition && type.IsGenericType) {
                var j = type.Name.IndexOf("`");
                r.Append(type.Name.Substring(0, j));
                r.Append('<');
                #if NET35
                r.Append(String.Join(",", type.GetGenericArguments().Select(ToString).ToArray()));
                #else
                r.Append(String.Join(",", type.GetGenericArguments().Select(ToString)));
                #endif
                r.Append('>');
                }
            #endif
            else
                {
                r.Append(type.Name);
                }
            return r.ToString();
            }
        #endregion
        #region M:WriteTo(TextWriter)
        public static void WriteTo(TextWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            EnsureConnection();
            var summary = new Dictionary<String, TraceSummaryInfo>();
            using (var c = connection.CreateCommand()) {
                c.CommandText = @"
                    WITH T AS
                      (
                      SELECT
                        [a].[LongName] [LongName]
                        ,SUM(([b].[EntryTime]-[a].[EntryTime])) [Duration]
                        ,COUNT([a].[Id]) [Count]
                        ,SUM([a].[Size]) [Size]
                      FROM [TraceInfo] [a]
                        INNER JOIN [TraceInfo] [b] ON ([b].[LeavePoint]=[a].[Id])
                      GROUP BY [a].[Session],[a].[LongName]
                      )
                    SELECT
                      ROW_NUMBER() OVER (ORDER BY [a].[LongName]) [OrderId],*
                    FROM T [a]
                    ";
                using (var reader = c.ExecuteReader()) {
                    while (reader.Read())
                        {
                        var i = new TraceSummaryInfo {
                            EntryPoint = (String)reader[1],
                            Duration = new TimeSpan((Int64)reader[2]),
                            Size = ToInt64(reader[4])
                            };
                        summary[i.EntryPoint] = i;
                        var r = new StringBuilder();
                        r.Append($"{reader[0]:D3}:[{i.EntryPoint}]:[{i.Duration}]:[{reader[3]}]");
                        if (i.Size != null) {
                            var velocity = i.Size.Value/(Double)i.Duration.TotalSeconds;
                            if (velocity > 1024) {
                                velocity /= 1024;
                                i.Velocity = $"{velocity:F2}KB/s";
                                }
                            else
                                {
                                i.Velocity = $"{velocity:F2}B/s";
                                }
                            r.Append($":[{i.Velocity}]");
                            }
                        writer.WriteLine(r.ToString());
                        }
                    }
                }
            var table = new Dictionary<Int64, TraceInfo>();
            using (var c = connection.CreateCommand()) {
                c.CommandText = @"
                      SELECT
                         [a].[Id]
                        ,[a].[LongName] [LongName]
                        ,([b].[EntryTime]-[a].[EntryTime]) [Duration]
                        ,[a].[ParentId]
                        ,[a].[ShortName]
                      FROM [TraceInfo] [a]
                        INNER JOIN [TraceInfo] [b] ON ([b].[LeavePoint]=[a].[Id])
                      ORDER BY [a].[Id]
                    ";
                using (var reader = c.ExecuteReader()) {
                    while (reader.Read()) {
                        var i = new TraceInfo {
                            Id = ToInt32(reader[0]).GetValueOrDefault(),
                            LongName = ToString(reader[1]),
                            Duration = new TimeSpan((Int64)reader[2]),
                            ParentId = ToInt32(reader[3]),
                            ShortName = ToString(reader[4])
                            };
                        table[i.Id] = i;
                        }
                    }
                }
            if (table.Count > 0) {
                writer.WriteLine("------------------------");
                WriteTo(writer, summary, table, 0, 1.0, -1, 0);
                }
            }
        #endregion
        #region M:WriteTo(TextWriter,Dictionary<String,TraceSummaryInfo>,Dictionary<Int64,TraceInfo>)
        private static void WriteTo(TextWriter writer, Dictionary<String, TraceSummaryInfo> summary, Dictionary<Int64, TraceInfo> table, Int32 level, Double K, Int64 timespan, params Int32[] parents) {
            var values = table.Where(i => parents.Any(j => j == i.Value.ParentId)).Select(i => i.Value).ToArray();
            var shortnames = values.Select(i => Tuple.Create(i.LongName, i.ShortName)).Distinct().ToDictionary(i => i.Item1, i=> i.Item2);
            var groups = values.GroupBy(i=>i.LongName).Select(i=>
                Tuple.Create(i,
                    i.Sum(j => j.Duration.Ticks),
                    i.Count())).ToArray();
            if (timespan < 0) {
                timespan = values.Sum(i => i.Duration.Ticks);
                }
            foreach (var j in groups.OrderByDescending(i => i.Item2)) {
                var builder = new StringBuilder();
                builder.Append(new String(' ', level*2));
                var ticks = j.Item2;
                var count = j.Item3;
                var P = (((Double)ticks)/timespan)*K;
                builder.AppendFormat(culture, "[{0:00.00}%]:", P*100.0);
                builder.AppendFormat(culture, "[{0}]:", GetShortName(j.Item1.Key, shortnames));
                builder.AppendFormat(culture, "[{0:F2} ms]:", (new TimeSpan(ticks)).TotalMilliseconds);
                builder.AppendFormat(culture, "[{0} hit]:", count);
                builder.AppendFormat(culture, "[{0}]:", j.Item1.Key);
                writer.WriteLine(builder);
                WriteTo(writer, summary, table, level + 1, P, j.Item2, j.Item1.Select(i => i.Id).ToArray());
                }
            }
        #endregion

        private class TraceSummaryInfo
            {
            public String EntryPoint { get;set; }
            public TimeSpan Duration { get;set; }
            public Int32 Count { get;set; }
            public String Velocity { get;set; }
            public Int64? Size { get;set; }
            }

        private class TraceInfo
            {
            public Int32 Id { get;set; }
            public String LongName { get;set; }
            public String ShortName { get;set; }
            public TimeSpan Duration { get;set; }
            public Int32? ParentId { get;set; }
            }

        #region M:ToDouble(Object):Double?
        private static Double? ToDouble(Object value) {
            if (value == null) { return null; }
            if (value is DBNull) { return null; }
            if (value is Double) { return (Double)value; }
            if (value is IConvertible c) { return c.ToDouble(null); }
            return Double.TryParse(value.ToString(), out var r)
                ? r
                : new Double?();
            }
        #endregion
        #region M:ToInt64(Object):Int64?
        private static Int64? ToInt64(Object value) {
            if (value == null) { return null; }
            if (value is DBNull) { return null; }
            if (value is Int64) { return (Int64)value; }
            if (value is IConvertible c) { return c.ToInt64(null); }
            return Int64.TryParse(value.ToString(), out var r)
                ? r
                : new Int64?();
            }
        #endregion
        #region M:ToInt32(Object):Int32?
        private static Int32? ToInt32(Object value) {
            if (value == null) { return null; }
            if (value is DBNull) { return null; }
            if (value is Int32) { return (Int32)value; }
            if (value is IConvertible c) { return c.ToInt32(null); }
            return Int32.TryParse(value.ToString(), out var r)
                ? r
                : new Int32?();
            }
        #endregion
        #region M:ToString(Object):String
        private static String ToString(Object value)
            {
            return ((value == null) || (value is DBNull))
                ? null
                : value.ToString();
            }
        #endregion
        }
    }   