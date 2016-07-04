
// Type: iAnywhere.Data.SQLAnywhere.SADefault
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  ///     <para>Represents a parameter with a default value.</para>
  /// </summary>
  /// <remarks>
  ///             <para>There is no constructor for SADefault.</para>
  ///             <code>SAParameter parm = new SAParameter();
  /// parm.Value = SADefault.Value;</code>
  /// 
  ///         </remarks>
  public sealed class SADefault
  {
    /// <summary>
    ///     <para>Gets the value for a default parameter. This field is read-only and static.</para>
    /// </summary>
    public static readonly SADefault Value = new SADefault();

    private SADefault()
    {
    }
  }
}
