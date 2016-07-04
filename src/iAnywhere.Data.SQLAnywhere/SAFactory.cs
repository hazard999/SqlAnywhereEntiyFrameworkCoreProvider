
// Type: iAnywhere.Data.SQLAnywhere.SAFactory
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace iAnywhere.Data.SQLAnywhere
{
    /// <summary>
    ///     <para>Represents a set of methods for creating instances of the iAnywhere.Data.SQLAnywhere provider's implementation of the data source classes.</para>
    /// </summary>
    /// <remarks>
    ///             <para>There is no constructor for SAFactory.</para>
    ///             <para>ADO.NET 2.0 adds two new classes, DbProviderFactories and DbProviderFactory, to make provider independent code easier to write. To use them with SQL Anywhere specify iAnywhere.Data.SQLAnywhere as the provider invariant name passed to GetFactory. For example:</para>
    ///             <code>' Visual Basic
    /// Dim factory As DbProviderFactory = _
    ///   DbProviderFactories.GetFactory( "iAnywhere.Data.SQLAnywhere" )
    /// Dim conn As DbConnection = _
    ///   factory.CreateConnection()
    /// 
    /// // C#
    /// DbProviderFactory factory =
    /// 		DbProviderFactories.GetFactory("iAnywhere.Data.SQLAnywhere" );
    /// DbConnection conn = factory.CreateConnection();</code>
    ///             <para>In this example, conn is created as an SAConnection object.</para>
    ///             <para>For an explanation of provider factories and generic programming in ADO.NET 2.0, see <a href="http://msdn2.microsoft.com/en-us/library/ms379620.aspx"></a>.</para>
    ///             <para><b>Restrictions:</b> The SAFactory class is not available in the .NET Compact Framework 2.0.</para>
    ///             <para><b>Inherits: </b> <see cref="T:System.Data.Common.DbProviderFactory" /></para>
    /// 
    ///         </remarks>
    public sealed class SAFactory : DbProviderFactory, IServiceProvider
    {
        /// <summary>
        ///     <para>Represents the singleton instance of the SAFactory class.</para>
        /// </summary>
        /// <remarks>
        ///     <para>SAFactory is a singleton class, which means only this instance of this class can exist.</para>
        ///     <para>Normally you would not use this field directly. Instead, you get a reference to this instance of SAFactory using <see cref="M:System.Data.Common.DbProviderFactories.GetFactory(System.String)" />. For an example, see the SAFactory description.</para>
        ///     <para><b>Restrictions: </b> The SAFactory class is not available in the .NET Compact Framework 2.0.</para>
        /// </remarks>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SAFactory" />
        public static readonly SAFactory Instance = new SAFactory();

        /// <summary>
        ///     <para>Always returns true, which indicates that an SADataSourceEnumerator object can be created.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SADataSourceEnumerator" />
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override bool CanCreateDataSourceEnumerator
        {
            get
            {
                return true;
            }
        }

        private SAFactory()
        {
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbCommand" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbCommand CreateCommand()
        {
            return new SACommand();
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbConnection" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbConnection CreateConnection()
        {
            return new SAConnection();
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbCommandBuilder" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return (DbCommandBuilder)new SACommandBuilder();
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbDataAdapter" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbDataAdapter CreateDataAdapter()
        {
            return (DbDataAdapter)new SADataAdapter();
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbParameter" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbParameter CreateParameter()
        {
            return new SAParameter();
        }

        /// <summary>
        ///     <para>Returns a strongly-typed CodeAccessPermission instance.</para>
        /// </summary>
        /// <param name="state">
        ///     A member of the <see cref="T:System.Security.Permissions.PermissionState" /> enumeration.
        /// </param>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override CodeAccessPermission CreatePermission(PermissionState state)
        {
            return (CodeAccessPermission)new SAPermission(state);
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbConnectionStringBuilder" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new SAConnectionStringBuilder();
        }

        /// <summary>
        ///     <para>Returns a strongly typed <see cref="T:System.Data.Common.DbDataSourceEnumerator" /> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new SACommand object typed as DbCommand.</para>
        ///    </returns>
        /// <seealso cref="T:iAnywhere.Data.SQLAnywhere.SACommand" />
        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return (DbDataSourceEnumerator)new SADataSourceEnumerator();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(DbProviderServices))
                return SAProviderServices.Instance;
            return null;
        }
    }
}
