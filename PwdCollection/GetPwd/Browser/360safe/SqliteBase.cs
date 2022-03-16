using System;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;


namespace Safe360pwd
{


    public class SQLiteBase
    {
        private IntPtr database;
        private const int SQL_OK = 0;
        private const int SQL_ROW = 100;
        private const int SQL_DONE = 101;


        static int hModule = LoadLibrary(common.DllPath);


        public SQLiteBase()
        {
            database = IntPtr.Zero;
        }
        public SQLiteBase(String baseName, String key)
        {
            OpenDatabase(baseName, key);
        }
        public void OpenDatabase(String baseName, String key)
        {

            IntPtr intPtr = GetProcAddress(hModule, "sqlite3_open");
            sqlite3_open opendb = (sqlite3_open)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sqlite3_open));
            if (opendb(StringToPointer(baseName), out database) != SQL_OK)
            {
                database = IntPtr.Zero;
                throw new Exception("Error with opening database " + baseName + "!");
            }
            byte[] passWord = null;
            int keyLength = 0;
            if (!string.IsNullOrEmpty(key))
            {
                passWord = Encoding.UTF8.GetBytes(key);
                keyLength = passWord.Length;
            }

            IntPtr intPtr2 = GetProcAddress(hModule, "sqlite3_key");
            sqlite3_key sqlite_key = (sqlite3_key)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(sqlite3_key));
            sqlite_key(database, passWord, keyLength);
        }

        private IntPtr StringToPointer(String str)
        {
            if (str == null)
            {
                return IntPtr.Zero;
            }
            else
            {
                Encoding encoding = Encoding.UTF8;
                Byte[] bytes = encoding.GetBytes(str);
                int length = bytes.Length + 1;
                IntPtr pointer = HeapAlloc(GetProcessHeap(), 0, (UInt32)length);
                Marshal.Copy(bytes, 0, pointer, bytes.Length);
                Marshal.WriteByte(pointer, bytes.Length, 0);
                return pointer;
            }
        }
        public void CloseDatabase()
        {
            IntPtr intPtr = GetProcAddress(hModule, "sqlite3_close");
            sqlite3_close closedb = (sqlite3_close)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sqlite3_close));
            if (database != IntPtr.Zero)
            {
                closedb(database);
            }
        }

        public ArrayList GetTables()
        {
            String query = "SELECT name FROM sqlite_master " +
                                        "WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%'" +
                                        "UNION ALL " +
                                        "SELECT name FROM sqlite_temp_master " +
                                        "WHERE type IN ('table','view') " +
                                        "ORDER BY 1";
            DataTable table = ExecuteQuery(query);

            ArrayList list = new ArrayList();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row.ItemArray[0].ToString());
            }
            return list;
        }
        public DataTable ExecuteQuery(String query)
        {
            IntPtr statement;
            IntPtr excessData;
            IntPtr intPtr = GetProcAddress(hModule, "sqlite3_prepare_v2");
            sqlite3_prepare_v2 sqlite_prepare_v2 = (sqlite3_prepare_v2)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sqlite3_prepare_v2));
            IntPtr intPtr2 = GetProcAddress(hModule, "sqlite3_finalize");
            sqlite3_finalize sqlite_finalize = (sqlite3_finalize)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(sqlite3_finalize));

            sqlite_prepare_v2(database, StringToPointer(query), GetPointerLenght(StringToPointer(query)), out statement, out excessData);
            DataTable table = new DataTable();
            int result = ReadFirstRow(statement, ref table);

            while (result == SQL_ROW)
                result = ReadNextRow(statement, ref table);

            sqlite_finalize(statement);
            return table;
        }

        public void ExecuteNonQuery(String query)
        {
            IntPtr error;
            IntPtr intPtr = GetProcAddress(hModule, "sqlite3_exec");
            sqlite3_exec execdb = (sqlite3_exec)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sqlite3_exec));
            IntPtr intPtr2 = GetProcAddress(hModule, "sqlite3_errmsg");
            sqlite3_errmsg sqlite_errmsg = (sqlite3_errmsg)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(sqlite3_errmsg));

            execdb(database, StringToPointer(query), IntPtr.Zero, IntPtr.Zero, out error);
            if (error != IntPtr.Zero)
                throw new Exception("Error with executing non-query: \"" + query + "\"!\n" + PointerToString(sqlite_errmsg(error)));
        }
        public enum SQLiteDataTypes
        {
            INT = 1,
            FLOAT,
            TEXT,
            BLOB,
            NULL
        };
        private int ReadFirstRow(IntPtr statement, ref DataTable table)
        {
            IntPtr intPtr = GetProcAddress(hModule, "sqlite3_step");
            sqlite3_step sqlite_step = (sqlite3_step)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sqlite3_step));
            IntPtr intPtr2 = GetProcAddress(hModule, "sqlite3_column_count");
            sqlite3_column_count sqlite_column_count = (sqlite3_column_count)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(sqlite3_column_count));
            IntPtr intPtr3 = GetProcAddress(hModule, "sqlite3_column_name");
            sqlite3_column_name sqlite_column_name = (sqlite3_column_name)Marshal.GetDelegateForFunctionPointer(intPtr3, typeof(sqlite3_column_name));
            table = new DataTable("resultTable");
            IntPtr intPtr4 = GetProcAddress(hModule, "sqlite3_column_type");
            sqlite3_column_type sqlite_column_type = (sqlite3_column_type)Marshal.GetDelegateForFunctionPointer(intPtr4, typeof(sqlite3_column_type));
            int resultType = sqlite_step(statement);
            IntPtr intPtr5 = GetProcAddress(hModule, "sqlite3_column_int");
            sqlite3_column_int sqlite_column_int = (sqlite3_column_int)Marshal.GetDelegateForFunctionPointer(intPtr5, typeof(sqlite3_column_int));
            IntPtr intPtr6 = GetProcAddress(hModule, "sqlite3_column_double");
            sqlite3_column_double sqlite_column_double = (sqlite3_column_double)Marshal.GetDelegateForFunctionPointer(intPtr6, typeof(sqlite3_column_double));
            IntPtr intPtr7 = GetProcAddress(hModule, "sqlite3_column_text");
            sqlite3_column_text sqlite_column_text = (sqlite3_column_text)Marshal.GetDelegateForFunctionPointer(intPtr7, typeof(sqlite3_column_text));
            IntPtr intPtr8 = GetProcAddress(hModule, "sqlite3_column_blob");
            sqlite3_column_blob sqlite_column_blob = (sqlite3_column_blob)Marshal.GetDelegateForFunctionPointer(intPtr8, typeof(sqlite3_column_blob));

            if (resultType == SQL_ROW)
            {
                int columnCount = sqlite_column_count(statement);
                String columnName = "";
                int columnType = 0;
                object[] columnValues = new object[columnCount];

                for (int i = 0; i < columnCount; i++)
                {
                    columnName = PointerToString(sqlite_column_name(statement, i));
                    columnType = sqlite_column_type(statement, i);

                    switch (columnType)
                    {
                        case (int)SQLiteDataTypes.INT:
                            {
                                table.Columns.Add(columnName, Type.GetType("System.Int32"));
                                columnValues[i] = sqlite_column_int(statement, i);
                                break;
                            }
                        case (int)SQLiteDataTypes.FLOAT:
                            {
                                table.Columns.Add(columnName, Type.GetType("System.Single"));
                                columnValues[i] = sqlite_column_double(statement, i);
                                break;
                            }
                        case (int)SQLiteDataTypes.TEXT:
                            {
                                table.Columns.Add(columnName, Type.GetType("System.String"));
                                columnValues[i] = PointerToString(sqlite_column_text(statement, i));
                                break;
                            }
                        case (int)SQLiteDataTypes.BLOB:
                            {
                                table.Columns.Add(columnName, Type.GetType("System.String"));
                                columnValues[i] = PointerToString(sqlite_column_blob(statement, i));
                                break;
                            }
                        default:
                            {
                                table.Columns.Add(columnName, Type.GetType("System.String"));
                                columnValues[i] = "";
                                break;
                            }
                    }
                }

                table.Rows.Add(columnValues);
            }
            return sqlite_step(statement);
        }
        private int ReadNextRow(IntPtr statement, ref DataTable table)
        {
            IntPtr intPtr2 = GetProcAddress(hModule, "sqlite3_column_count");
            sqlite3_column_count sqlite_column_count = (sqlite3_column_count)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(sqlite3_column_count));
            int columnCount = sqlite_column_count(statement);

            int columnType = 0;

            IntPtr intPtr = GetProcAddress(hModule, "sqlite3_step");
            sqlite3_step sqlite_step = (sqlite3_step)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sqlite3_step));
            IntPtr intPtr3 = GetProcAddress(hModule, "sqlite3_column_type");
            sqlite3_column_type sqlite_column_type = (sqlite3_column_type)Marshal.GetDelegateForFunctionPointer(intPtr3, typeof(sqlite3_column_type));
            IntPtr intPtr4 = GetProcAddress(hModule, "sqlite3_column_int");
            sqlite3_column_int sqlite_column_int = (sqlite3_column_int)Marshal.GetDelegateForFunctionPointer(intPtr4, typeof(sqlite3_column_int));
            IntPtr intPtr5 = GetProcAddress(hModule, "sqlite3_column_double");
            sqlite3_column_double sqlite_column_double = (sqlite3_column_double)Marshal.GetDelegateForFunctionPointer(intPtr5, typeof(sqlite3_column_double));
            IntPtr intPtr6 = GetProcAddress(hModule, "sqlite3_column_text");
            sqlite3_column_text sqlite_column_text = (sqlite3_column_text)Marshal.GetDelegateForFunctionPointer(intPtr6, typeof(sqlite3_column_text));
            IntPtr intPtr7 = GetProcAddress(hModule, "sqlite3_column_blob");
            sqlite3_column_blob sqlite_column_blob = (sqlite3_column_blob)Marshal.GetDelegateForFunctionPointer(intPtr7, typeof(sqlite3_column_blob));
            object[] columnValues = new object[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                columnType = sqlite_column_type(statement, i);

                switch (columnType)
                {
                    case (int)SQLiteDataTypes.INT:
                        {
                            columnValues[i] = sqlite_column_int(statement, i);
                            break;
                        }
                    case (int)SQLiteDataTypes.FLOAT:
                        {
                            columnValues[i] = sqlite_column_double(statement, i);
                            break;
                        }
                    case (int)SQLiteDataTypes.TEXT:
                        {
                            columnValues[i] = PointerToString(sqlite_column_text(statement, i));
                            break;
                        }
                    case (int)SQLiteDataTypes.BLOB:
                        {
                            columnValues[i] = PointerToString(sqlite_column_blob(statement, i));
                            break;
                        }
                    default:
                        {
                            columnValues[i] = "";
                            break;
                        }
                }
            }
            table.Rows.Add(columnValues);
            return sqlite_step(statement);
        }
        private String PointerToString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            Encoding encoding = Encoding.UTF8;

            int length = GetPointerLenght(ptr);
            Byte[] bytes = new Byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return encoding.GetString(bytes, 0, length);
        }

        private int GetPointerLenght(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return 0;
            return lstrlen(ptr);
        }
        #region DLL_Active
        [DllImport("kernel32")]
        private extern static IntPtr HeapAlloc(IntPtr heap, UInt32 flags, UInt32 bytes);

        [DllImport("kernel32")]
        private extern static IntPtr GetProcessHeap();

        [DllImport("kernel32")]
        private extern static int lstrlen(IntPtr str);

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        public static extern int LoadLibrary(
            [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        public static extern IntPtr GetProcAddress(int hModule,
            [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        public static extern bool FreeLibrary(int hModule);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        delegate int sqlite3_key(IntPtr db, byte[] key, int keylen);
        delegate int sqlite3_open(IntPtr fileName, out IntPtr database);
        delegate IntPtr sqlite3_errmsg(IntPtr database);
        delegate int sqlite3_close(IntPtr database);
        delegate int sqlite3_exec(IntPtr database, IntPtr query, IntPtr callback, IntPtr arguments, out IntPtr error);
        delegate int sqlite3_step(IntPtr statement);
        delegate int sqlite3_prepare_v2(IntPtr database, IntPtr query, int length, out IntPtr statement, out IntPtr tail);
        delegate int sqlite3_column_count(IntPtr statement);
        delegate IntPtr sqlite3_column_name(IntPtr statement, int columnNumber);
        delegate int sqlite3_column_type(IntPtr statement, int columnNumber);
        delegate int sqlite3_column_int(IntPtr statement, int columnNumber);
        delegate double sqlite3_column_double(IntPtr statement, int columnNumber);
        delegate IntPtr sqlite3_column_text(IntPtr statement, int columnNumber);
        delegate IntPtr sqlite3_column_blob(IntPtr statement, int columnNumber);
        delegate int sqlite3_finalize(IntPtr handle);
        #endregion DLL_Active
    }
}