using CoCSharp.Data.Csv;
using System;
using System.IO;

namespace CoCSharp.Data
{
    public abstract class Database
    {
        public Database(string path)
        {
            TablePath = path;
            Table = new CsvTable(path);
        }

        public string TablePath { get; set; }
        public virtual CsvTable Table { get; set; }

        public void ReloadDatabase()
        {
            Table = new CsvTable(TablePath);
            LoadDatabase();
        }

        public void LoadDatabase1(Type villageObjDataType)
        {
            var rows = Table.Rows;
            var typesRow = Table.TypesRow;
            var type = villageObjDataType;
            var properties = villageObjDataType.GetProperties();

            var parentData = Activator.CreateInstance(type);
            for (int k = 1; k < rows.Count; k++) // k = 1, because skipping TypesRow
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var propertyName = properties[i].Name;
                    var propertyAttributes = properties[i].GetCustomAttributes(false);
                    for (int j = 0; j < propertyAttributes.Length; j++) // checks the for CsvPropertyAttribute
                    {
                        if (propertyAttributes[j].GetType() == typeof(CsvPropertyAttribute))
                        {
                            propertyName = ((CsvPropertyAttribute)propertyAttributes[j]).PropertyName;
                            break;
                        }
                    }

                    var newValue = rows[k][propertyName];
                    var propertyType = typesRow[propertyName].ToString();
                    switch (propertyType)
                    {
                        case "String":
                            if (string.IsNullOrEmpty((string)newValue))
                                newValue = (string)null;
                            break;

                        case "int":
                            if (string.IsNullOrEmpty((string)newValue))
                            {
                                newValue = 0;
                                break;
                            }
                            newValue = Convert.ToInt32(newValue);
                            break;

                        case "Boolean":
                        case "boolean":
                            if (string.IsNullOrEmpty((string)newValue))
                            {
                                newValue = false;
                                break;
                            }
                            newValue = Convert.ToBoolean(newValue);
                            break;

                        default:
                            throw new InvalidDataException(string.Format("Unhandled data type '{0}'.", propertyType));
                    }
                    var parameters = new object[] { newValue };
                    properties[i].SetMethod.Invoke(parentData, parameters);
                }
            }
        }

        public abstract void LoadDatabase();
    }
}
