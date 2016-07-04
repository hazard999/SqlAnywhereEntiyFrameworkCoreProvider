
// Type: iAnywhere.Data.SQLAnywhere.SAProviderServices
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Common.CommandTrees;
using System.Data.Metadata.Edm;
using System.Reflection;
using System.Text;
using System.Xml;

namespace iAnywhere.Data.SQLAnywhere
{
    internal sealed class SAProviderServices : DbProviderServices
    {
        internal static readonly SAProviderServices Instance = new SAProviderServices();

        protected override DbCommandDefinition CreateDbCommandDefinition(DbProviderManifest manifest, DbCommandTree commandTree)
        {
            return this.CreateCommandDefinition(CreateCommand(manifest, commandTree));
        }

        private DbCommand CreateCommand(DbProviderManifest manifest, DbCommandTree commandTree)
        {
            SAUtility.CheckArgumentNull((object)manifest, "manifest");
            SAUtility.CheckArgumentNull((object)commandTree, "commandTree");
            SACommand saCommand = new SACommand();
            List<DbParameter> parameters;
            CommandType commandType;
            saCommand.CommandText = SqlGenerator.GenerateSql(commandTree, out parameters, out commandType);
            saCommand.CommandType = commandType;
            EdmFunction edmFunction = (EdmFunction)null;
            if (commandTree is DbFunctionCommandTree)
                edmFunction = ((DbFunctionCommandTree)commandTree).EdmFunction;
            foreach (KeyValuePair<string, TypeUsage> parameter in commandTree.Parameters)
            {
                FunctionParameter functionParameter;
                SAParameter saParameter = edmFunction == null || !edmFunction.Parameters.TryGetValue(parameter.Key, false, out functionParameter) ? SAProviderServices.CreateSAParameter(parameter.Key, parameter.Value, ParameterMode.In, (object)DBNull.Value) : SAProviderServices.CreateSAParameter(functionParameter.Name, functionParameter.TypeUsage, functionParameter.Mode, (object)DBNull.Value);
                saCommand.Parameters.Add(saParameter);
            }
            if (parameters != null && 0 < parameters.Count)
            {
                if (!(commandTree is DbInsertCommandTree) && !(commandTree is DbUpdateCommandTree) && !(commandTree is DbDeleteCommandTree))
                    throw new InvalidOperationException("SqlGenParametersNotPermitted");
                foreach (DbParameter dbParameter in parameters)
                {
                    if (dbParameter.ParameterName != null && dbParameter.ParameterName.Length > 0 && dbParameter.ParameterName[0] == 64)
                        dbParameter.ParameterName = dbParameter.ParameterName.Substring(1);
                    saCommand.Parameters.Add(dbParameter);
                }
            }
            return saCommand;
        }

        protected override string GetDbProviderManifestToken(DbConnection connection)
        {
            SAUtility.CheckArgumentNull(connection, "connection");
            SAConnection saConnection = connection as SAConnection;
            if (saConnection == null)
                throw new ArgumentException("The connection is not of type 'SAConnection'.");
            if (string.IsNullOrEmpty(saConnection.ConnectionString))
                throw new ArgumentException("Could not determine storage version; a valid storage connection or a version hint is required.");
            bool flag = false;
            try
            {
                if (saConnection.State != ConnectionState.Open)
                {
                    saConnection.Open();
                    flag = true;
                }
                return SAStoreVersionUtil.GetManifestToken(saConnection.ServerVersion);
            }
            finally
            {
                if (flag)
                    saConnection.Close();
            }
        }

        protected override DbProviderManifest GetDbProviderManifest(string manifestToken)
        {
            if (string.IsNullOrEmpty(manifestToken))
                throw new ArgumentException("Could not determine store version; a valid store connection or a version hint is required.");
            return (DbProviderManifest)new SAProviderManifest(manifestToken);
        }

        internal new static XmlReader GetXmlResource(string resourceName)
        {
            return XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
        }

        internal static SAParameter CreateSAParameter(string name, TypeUsage type, ParameterMode mode, object value)
        {
            SAParameter saParameter = new SAParameter(name, value);
            ParameterDirection parameterDirection = MetadataHelpers.ParameterModeToParameterDirection(mode);
            if (saParameter.Direction != parameterDirection)
                saParameter.Direction = parameterDirection;
            bool isOutParam = mode != ParameterMode.In;
            int? size;
            SADbType saDbType = SAProviderServices.GetSADbType(type, isOutParam, out size);
            if (saParameter.SADbType != saDbType)
                saParameter.SADbType = saDbType;
            if (size.HasValue && (isOutParam || saParameter.Size != size.Value))
                saParameter.Size = size.Value;
            bool flag = MetadataHelpers.IsNullable(type);
            if (isOutParam || flag != saParameter.IsNullable)
                saParameter.IsNullable = flag;
            return saParameter;
        }

        private static SADbType GetSADbType(TypeUsage type, bool isOutParam, out int? size)
        {
            PrimitiveTypeKind primitiveTypeKind = MetadataHelpers.GetPrimitiveTypeKind(type);
            size = new int?();
            switch (primitiveTypeKind)
            {
                case PrimitiveTypeKind.Binary:
                    size = SAProviderServices.GetParameterSize(type, isOutParam);
                    return SAProviderServices.GetBinaryDbType(type);
                case PrimitiveTypeKind.Boolean:
                    return SADbType.Bit;
                case PrimitiveTypeKind.Byte:
                    return SADbType.TinyInt;
                case PrimitiveTypeKind.DateTime:
                    return SADbType.DateTime;
                case PrimitiveTypeKind.Decimal:
                    return SADbType.Decimal;
                case PrimitiveTypeKind.Double:
                    return SADbType.Float;
                case PrimitiveTypeKind.Guid:
                    return SADbType.UniqueIdentifier;
                case PrimitiveTypeKind.Single:
                    return SADbType.Real;
                case PrimitiveTypeKind.SByte:
                    return SADbType.SmallInt;
                case PrimitiveTypeKind.Int16:
                    return SADbType.SmallInt;
                case PrimitiveTypeKind.Int32:
                    return SADbType.Integer;
                case PrimitiveTypeKind.Int64:
                    return SADbType.BigInt;
                case PrimitiveTypeKind.String:
                    size = SAProviderServices.GetParameterSize(type, isOutParam);
                    return SAProviderServices.GetStringDbType(type);
                case PrimitiveTypeKind.Time:
                    return SADbType.DateTime;
                default:
                    return SADbType.Char;
            }
        }

        /// <summary>
        /// Determines preferred value for SqlParameter.Size. Returns null
        /// where there is no preference.
        /// </summary>
        private static int? GetParameterSize(TypeUsage type, bool isOutParam)
        {
            int maxLength;
            if (MetadataHelpers.TryGetMaxLength(type, out maxLength))
                return new int?(maxLength);
            if (isOutParam)
                return new int?(int.MaxValue);
            return new int?();
        }

        /// <summary>
        /// Chooses the appropriate SqlDbType for the given string type.
        /// </summary>
        private static SADbType GetStringDbType(TypeUsage type)
        {
            SADbType saDbType;
            if (type.EdmType.Name.ToLowerInvariant() == "xml")
            {
                saDbType = SADbType.Xml;
            }
            else
            {
                bool isFixedLength;
                if (!MetadataHelpers.TryGetIsFixedLength(type, out isFixedLength))
                    isFixedLength = false;
                bool isUnicode;
                if (!MetadataHelpers.TryGetIsUnicode(type, out isUnicode))
                    isUnicode = true;
                saDbType = !isFixedLength ? (isUnicode ? SADbType.NVarChar : SADbType.VarChar) : (isUnicode ? SADbType.NChar : SADbType.Char);
            }
            return saDbType;
        }

        /// <summary>
        /// Chooses the appropriate SqlDbType for the given binary type.
        /// </summary>
        private static SADbType GetBinaryDbType(TypeUsage type)
        {
            bool isFixedLength;
            if (!MetadataHelpers.TryGetIsFixedLength(type, out isFixedLength))
                isFixedLength = false;
            return !isFixedLength ? SADbType.VarBinary : SADbType.Binary;
        }

        protected override void DbCreateDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            SAUtility.CheckArgumentNull(connection, "connection");
            SAUtility.CheckArgumentNull((object)storeItemCollection, "storeItemCollection");
            DbCommand cmd = null;
            OpenConnection(connection, out cmd);
            cmd.CommandText = GenerateDatabaseScript(storeItemCollection);
            cmd.ExecuteNonQuery();
        }

        protected override string DbCreateDatabaseScript(string providerManifestToken, StoreItemCollection storeItemCollection)
        {
            SAUtility.CheckArgumentNull(providerManifestToken, "providerManifestToken");
            SAUtility.CheckArgumentNull((object)storeItemCollection, "storeItemCollection");
            return GenerateDatabaseScript(storeItemCollection);
        }

        protected override bool DbDatabaseExists(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            SAUtility.CheckArgumentNull(connection, "connection");
            SAUtility.CheckArgumentNull((object)storeItemCollection, "storeItemCollection");
            List<string> tables = null;
            List<string> schemas = null;
            GetTables(storeItemCollection, out schemas, out tables);
            int num = 0;
            if (tables.Count > 0)
            {
                DbCommand cmd = null;
                OpenConnection(connection, out cmd);
                DataTable dataTable = new DataTable();
                new SADataAdapter("SELECT user_name, table_name FROM sys.systab JOIN sys.sysuser", connection as SAConnection).Fill(dataTable);
                for (int index1 = 0; index1 < tables.Count; ++index1)
                {
                    for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
                    {
                        if (SAUtility.CompareString(schemas[index1], dataTable.Rows[index2][0].ToString()) && SAUtility.CompareString(tables[index1], dataTable.Rows[index2][1].ToString()))
                        {
                            ++num;
                            break;
                        }
                    }
                }
            }
            return num == tables.Count;
        }

        protected override void DbDeleteDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            SAUtility.CheckArgumentNull(connection, "connection");
            SAUtility.CheckArgumentNull((object)storeItemCollection, "storeItemCollection");
            List<string> tables = null;
            List<string> schemas = null;
            GetTables(storeItemCollection, out schemas, out tables);
            DbCommand cmd = null;
            OpenConnection(connection, out cmd);
            while (tables.Count > 0)
            {
                for (int index = tables.Count - 1; index >= 0; --index)
                {
                    cmd.CommandText = "DROP TABLE IF EXISTS " + GetQuotedName(schemas[index], tables[index]);
                    cmd.ExecuteNonQuery();
                    tables.RemoveAt(index);
                    schemas.RemoveAt(index);
                }
            }
        }

        private void OpenConnection(DbConnection connection, out DbCommand cmd)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd = connection.CreateCommand();
        }

        private void GetTables(StoreItemCollection storeItemCollection, out List<string> schemas, out List<string> tables)
        {
            tables = new List<string>();
            schemas = new List<string>();
            foreach (EntitySetBase baseEntitySet in FindEntityContainer(storeItemCollection).BaseEntitySets)
            {
                if (!baseEntitySet.ElementType.Abstract && baseEntitySet is EntitySet)
                {
                    EntitySet entitySet = baseEntitySet as EntitySet;
                    string table = null;
                    string schema = null;
                    GetEntitySetProperties((EntitySetBase)entitySet, out table, out schema);
                    tables.Add(table);
                    schemas.Add(schema);
                }
            }
        }

        private string GenerateDatabaseScript(StoreItemCollection storeItemCollection)
        {
            List<EntitySet> entitySetList = new List<EntitySet>();
            List<AssociationSet> associationSetList = new List<AssociationSet>();
            Dictionary<EntityType, string> dictionary1 = new Dictionary<EntityType, string>();
            Dictionary<EntityType, string> dictionary2 = new Dictionary<EntityType, string>();
            foreach (EntitySetBase baseEntitySet in FindEntityContainer(storeItemCollection).BaseEntitySets)
            {
                if (!baseEntitySet.ElementType.Abstract)
                {
                    if (baseEntitySet is EntitySet)
                        entitySetList.Add(baseEntitySet as EntitySet);
                    else if (baseEntitySet is AssociationSet)
                        associationSetList.Add(baseEntitySet as AssociationSet);
                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (EntitySet entitySet in entitySetList)
            {
                string table = null;
                string schema = null;
                GetEntitySetProperties((EntitySetBase)entitySet, out table, out schema);
                dictionary1.Add(entitySet.ElementType, table);
                dictionary2.Add(entitySet.ElementType, schema);
                string quotedName = GetQuotedName(schema, table);
                stringBuilder.Append("DROP TABLE IF EXISTS " + quotedName + ";\n");
                stringBuilder.Append("CREATE TABLE " + quotedName + "( ");
                bool flag1 = true;
                foreach (EdmProperty property in entitySet.ElementType.Properties)
                {
                    if (flag1)
                        flag1 = false;
                    else
                        stringBuilder.Append(", ");
                    stringBuilder.Append(GetColumnDef(property));
                }
                if (entitySet.ElementType.KeyMembers.Count > 0)
                {
                    bool flag2 = true;
                    stringBuilder.Append(", PRIMARY KEY( ");
                    foreach (EdmMember keyMember in entitySet.ElementType.KeyMembers)
                    {
                        if (flag2)
                            flag2 = false;
                        else
                            stringBuilder.Append(", ");
                        stringBuilder.Append(keyMember.Name);
                    }
                    stringBuilder.Append(" )");
                }
                stringBuilder.Append(" );\n");
            }
            List<string> fkNames = new List<string>();
            foreach (AssociationSet associationSet in associationSetList)
            {
                ReferentialConstraint referentialConstraint = (associationSet.ElementType.MetadataProperties["ReferentialConstraints"].Value as ReadOnlyMetadataCollection<ReferentialConstraint>)[0];
                ReadOnlyMetadataCollection<EdmProperty> fromProperties = referentialConstraint.FromProperties;
                RelationshipEndMember fromRole = referentialConstraint.FromRole;
                string str1 = dictionary2[fromRole.GetEntityType()];
                string str2 = dictionary1[fromRole.GetEntityType()];
                ReadOnlyMetadataCollection<EdmProperty> toProperties = referentialConstraint.ToProperties;
                RelationshipEndMember toRole = referentialConstraint.ToRole;
                string str3 = dictionary2[toRole.GetEntityType()];
                string str4 = dictionary1[toRole.GetEntityType()];
                string fkName = GetFKName(fkNames, str1, str2, str3, str4);
                string quotedName1 = GetQuotedName(str1, str2);
                string keyColumns1 = GetKeyColumns(fromProperties);
                string quotedName2 = GetQuotedName(str3, str4);
                string keyColumns2 = GetKeyColumns(toProperties);
                bool flag = false;
                foreach (EdmProperty prop in toProperties)
                {
                    if (!IsNullable(prop))
                    {
                        flag = true;
                        break;
                    }
                }
                string format = "ALTER TABLE {0} ADD FOREIGN KEY {1}{2} REFERENCES {3}{4}";
                stringBuilder.AppendFormat(format, quotedName2, fkName, keyColumns2, quotedName1, keyColumns1);
                foreach (AssociationSetEnd associationSetEnd in associationSet.AssociationSetEnds)
                {
                    AssociationEndMember associationEndMember = associationSetEnd.CorrespondingAssociationEndMember;
                    if (associationEndMember.GetEntityType().Equals((object)toRole.GetEntityType()))
                    {
                        if (associationEndMember.DeleteBehavior == OperationAction.Cascade || flag)
                            stringBuilder.Append(" ON DELETE CASCADE");
                        else if (associationEndMember.DeleteBehavior == OperationAction.Restrict)
                        {
                            stringBuilder.Append(" ON DELETE RESTRICT");
                        }
                        else
                        {
                            int num = (int)associationEndMember.DeleteBehavior;
                        }
                    }
                }
                stringBuilder.Append(";\n");
            }
            return stringBuilder.ToString();
        }

        private string GetFKName(List<string> fkNames, string fromSchema, string fromTable, string toSchema, string toTable)
        {
            string str = string.Format("FK_{0}_{1}_{2}_{3}", toSchema, toTable, fromSchema, fromTable);
            if (!fkNames.Contains(str))
            {
                fkNames.Add(str);
            }
            else
            {
                int num = 2;
                do
                {
                    str = string.Format("{0}_{1}", str, num++);
                }
                while (fkNames.Contains(str));
                fkNames.Add(str);
            }
            return str;
        }

        private string GetKeyColumns(ReadOnlyMetadataCollection<EdmProperty> props)
        {
            StringBuilder stringBuilder = new StringBuilder("( ");
            bool flag = true;
            foreach (EdmProperty prop in props)
            {
                if (flag)
                    flag = false;
                else
                    stringBuilder.Append(", ");
                stringBuilder.Append(prop.Name);
            }
            stringBuilder.Append(" )");
            return stringBuilder.ToString();
        }

        private EntityContainer FindEntityContainer(StoreItemCollection storeItemCollection)
        {
            foreach (GlobalItem storeItem in (ReadOnlyMetadataCollection<GlobalItem>)storeItemCollection)
            {
                if (storeItem is EntityContainer)
                    return storeItem as EntityContainer;
            }
            return (EntityContainer)null;
        }

        private void GetEntitySetProperties(EntitySetBase entitySet, out string table, out string schema)
        {
            table = null;
            schema = null;
            foreach (MetadataProperty metadataProperty in entitySet.MetadataProperties)
            {
                if (SAUtility.CompareString(metadataProperty.Name, "Table"))
                    table = metadataProperty.Value as string;
                if (SAUtility.CompareString(metadataProperty.Name, "Schema"))
                    schema = metadataProperty.Value as string;
            }
        }

        private string GetQuotedName(string schema, string table)
        {
            return string.Format("[{0}].[{1}]", schema, table);
        }

        private string GetColumnDef(EdmProperty prop)
        {
            EdmType edmType = prop.TypeUsage.EdmType;
            string str = string.Empty;
            if (SAUtility.CompareString(edmType.Name, "bigint"))
            {
                str = prop.Name + " BIGINT";
                if (IsAutoGenerated(prop))
                    str += " DEFAULT AUTOINCREMENT";
            }
            else if (SAUtility.CompareString(edmType.Name, "bit"))
                str = prop.Name + " BIT";
            else if (SAUtility.CompareString(edmType.Name, "datetime"))
                str = prop.Name + " DATETIME";
            else if (SAUtility.CompareString(edmType.Name, "datetimeoffset"))
                str = prop.Name + " DATETIMEOFFSET";
            else if (SAUtility.CompareString(edmType.Name, "decimal"))
            {
                string precision = null;
                string scale = null;
                GetPrecisionScale(prop, out precision, out scale);
                str = string.IsNullOrWhiteSpace(precision) || string.IsNullOrWhiteSpace(scale) ? prop.Name + " DECIMAL" : string.Format("{0} DECIMAL( {1}, {2} )", (object)prop.Name, precision, scale);
            }
            else if (SAUtility.CompareString(edmType.Name, "float"))
                str = prop.Name + " DOUBLE";
            else if (SAUtility.CompareString(edmType.Name, "int"))
            {
                str = prop.Name + " INT";
                if (IsAutoGenerated(prop))
                    str += " DEFAULT AUTOINCREMENT";
            }
            else if (SAUtility.CompareString(edmType.Name, "nvarchar"))
                str = prop.Name + " NVARCHAR" + GetLength(prop, "MaxLength");
            else if (SAUtility.CompareString(edmType.Name, "nvarchar(max)"))
                str = prop.Name + " LONG NVARCHAR";
            else if (SAUtility.CompareString(edmType.Name, "real"))
                str = prop.Name + " FLOAT";
            else if (SAUtility.CompareString(edmType.Name, "smallint"))
            {
                str = prop.Name + " SMALLINT";
                if (IsAutoGenerated(prop))
                    str += " DEFAULT AUTOINCREMENT";
            }
            else if (SAUtility.CompareString(edmType.Name, "time"))
                str = prop.Name + " TIME";
            else if (SAUtility.CompareString(edmType.Name, "tinyint"))
            {
                str = prop.Name + " TINYINT";
                if (IsAutoGenerated(prop))
                    str += " DEFAULT AUTOINCREMENT";
            }
            else if (SAUtility.CompareString(edmType.Name, "uniqueidentifier"))
                str = prop.Name + " UNIQUEIDENTIFIER";
            else if (SAUtility.CompareString(edmType.Name, "varbinary"))
                str = prop.Name + " VARBINARY" + GetLength(prop, "MaxLength");
            else if (SAUtility.CompareString(edmType.Name, "varbinary(max)"))
                str = prop.Name + " LONG BINARY";
            else if (SAUtility.CompareString(edmType.Name, "varchar"))
                str = prop.Name + " VARCHAR" + GetLength(prop, "MaxLength");
            else if (SAUtility.CompareString(edmType.Name, "varchar(max)"))
                str = prop.Name + " LONG VARCHAR";
            if (!IsNullable(prop))
                str += " NOT NULL";
            string defaultValue = GetDefaultValue(prop);
            if (!string.IsNullOrWhiteSpace(defaultValue) && !IsAutoGenerated(prop))
                str = str + " DEFAULT " + defaultValue;
            return str;
        }

        private bool IsAutoGenerated(EdmProperty prop)
        {
            foreach (Facet facet in prop.TypeUsage.Facets)
            {
                if (SAUtility.CompareString(facet.Name, "StoreGeneratedPattern") && facet.Value.Equals((object)StoreGeneratedPattern.Identity))
                    return true;
            }
            return false;
        }

        private bool IsNullable(EdmProperty prop)
        {
            foreach (Facet facet in prop.TypeUsage.Facets)
            {
                if (SAUtility.CompareString(facet.Name, "Nullable"))
                    return (bool)facet.Value;
            }
            return true;
        }

        private string GetLength(EdmProperty prop, string facetName)
        {
            int? intValue = GetIntValue(prop, facetName);
            if (!intValue.HasValue)
                return string.Empty;
            return string.Format("( {0} )", intValue);
        }

        private bool? GetBoolValue(EdmProperty prop, string facetName)
        {
            foreach (Facet facet in prop.TypeUsage.Facets)
            {
                if (SAUtility.CompareString(facet.Name, facetName))
                    return (bool?)facet.Value;
            }
            return new bool?();
        }

        private int? GetIntValue(EdmProperty prop, string facetName)
        {
            foreach (Facet facet in prop.TypeUsage.Facets)
            {
                if (SAUtility.CompareString(facet.Name, facetName))
                    return (int?)facet.Value;
            }
            return new int?();
        }

        private string GetDefaultValue(EdmProperty prop)
        {
            foreach (Facet facet in prop.TypeUsage.Facets)
            {
                if (SAUtility.CompareString(facet.Name, "DefaultValue"))
                {
                    if (facet.Value == null)
                        return null;
                    return facet.Value.ToString();
                }
            }
            return null;
        }

        private void GetPrecisionScale(EdmProperty prop, out string precision, out string scale)
        {
            precision = null;
            scale = null;
            foreach (Facet facet in prop.TypeUsage.Facets)
            {
                if (SAUtility.CompareString(facet.Name, "Precision"))
                {
                    if (facet.Value != null)
                        precision = facet.Value.ToString();
                }
                else if (SAUtility.CompareString(facet.Name, "scale") && facet.Value != null)
                    scale = facet.Value.ToString();
            }
        }
    }
}
